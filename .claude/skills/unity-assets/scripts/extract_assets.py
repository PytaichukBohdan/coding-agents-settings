# /// script
# requires-python = ">=3.8"
# dependencies = ["UnityPy>=1.20", "Pillow>=9.0"]
# ///
"""Extract assets from Unity files/bundles to standard formats."""

import argparse
import json
import struct
import sys
from collections import Counter
from pathlib import Path

import UnityPy


def extract_texture2d(obj, output_dir: Path) -> str | None:
    tex = obj.parse_as_object()
    name = tex.m_Name or "unnamed_texture"
    out_path = output_dir / "Texture2D" / f"{name}.png"
    out_path.parent.mkdir(parents=True, exist_ok=True)
    try:
        tex.image.save(str(out_path))
        return str(out_path)
    except Exception as e:
        print(f"  Warning: Could not export texture '{name}': {e}")
        return None


def extract_sprite(obj, output_dir: Path) -> str | None:
    sprite = obj.parse_as_object()
    name = sprite.m_Name or "unnamed_sprite"
    out_path = output_dir / "Sprite" / f"{name}.png"
    out_path.parent.mkdir(parents=True, exist_ok=True)
    try:
        sprite.image.save(str(out_path))
        return str(out_path)
    except Exception as e:
        print(f"  Warning: Could not export sprite '{name}': {e}")
        return None


def extract_audioclip(obj, output_dir: Path) -> list[str]:
    clip = obj.parse_as_object()
    name = clip.m_Name or "unnamed_audio"
    audio_dir = output_dir / "AudioClip"
    audio_dir.mkdir(parents=True, exist_ok=True)
    exported = []
    try:
        for sample_name, data in clip.samples.items():
            out_path = audio_dir / sample_name
            with open(out_path, "wb") as f:
                f.write(data)
            exported.append(str(out_path))
    except Exception as e:
        print(f"  Warning: Could not export audio '{name}': {e}")
    return exported


def extract_font(obj, output_dir: Path) -> str | None:
    font = obj.parse_as_object()
    name = font.m_Name or "unnamed_font"
    font_dir = output_dir / "Font"
    font_dir.mkdir(parents=True, exist_ok=True)
    try:
        data = font.m_FontData
        ext = ".otf" if data[:4] == b"OTTO" else ".ttf"
        out_path = font_dir / f"{name}{ext}"
        with open(out_path, "wb") as f:
            f.write(data)
        return str(out_path)
    except Exception as e:
        print(f"  Warning: Could not export font '{name}': {e}")
        return None


def extract_textasset(obj, output_dir: Path) -> str | None:
    txt = obj.parse_as_object()
    name = txt.m_Name or "unnamed_text"
    text_dir = output_dir / "TextAsset"
    text_dir.mkdir(parents=True, exist_ok=True)
    try:
        data = txt.m_Script
        if isinstance(data, str):
            out_path = text_dir / f"{name}.txt"
            with open(out_path, "w", encoding="utf-8") as f:
                f.write(data)
        else:
            raw = data.encode("utf-8", "surrogateescape") if isinstance(data, str) else data
            out_path = text_dir / f"{name}.bytes"
            with open(out_path, "wb") as f:
                f.write(raw)
        return str(out_path)
    except Exception as e:
        print(f"  Warning: Could not export text asset '{name}': {e}")
        return None


def extract_monobehaviour(obj, output_dir: Path) -> str | None:
    mono_dir = output_dir / "MonoBehaviour"
    mono_dir.mkdir(parents=True, exist_ok=True)
    try:
        data = obj.parse_as_dict()
        name = data.get("m_Name", "") or f"mono_{obj.path_id}"
        out_path = mono_dir / f"{name}.json"
        with open(out_path, "w", encoding="utf-8") as f:
            json.dump(data, f, indent=2, default=str)
        return str(out_path)
    except Exception as e:
        print(f"  Warning: Could not export MonoBehaviour: {e}")
        return None


def extract_mesh(obj, output_dir: Path) -> str | None:
    mesh_dir = output_dir / "Mesh"
    mesh_dir.mkdir(parents=True, exist_ok=True)
    try:
        mesh = obj.parse_as_object()
        name = mesh.m_Name or "unnamed_mesh"
        out_path = mesh_dir / f"{name}.obj"

        # Export as OBJ using parsed dict for vertex data
        data = obj.parse_as_dict()
        with open(out_path, "w") as f:
            f.write(f"# Exported from Unity asset: {name}\n")
            f.write(f"o {name}\n")

            # Try to export basic mesh info
            if hasattr(mesh, "export"):
                f.write(mesh.export())
            else:
                f.write(f"# Mesh '{name}' — raw data saved as JSON fallback\n")
                json_path = mesh_dir / f"{name}.json"
                with open(json_path, "w") as jf:
                    json.dump(data, jf, indent=2, default=str)
                return str(json_path)

        return str(out_path)
    except Exception as e:
        print(f"  Warning: Could not export mesh: {e}")
        return None


EXTRACTORS = {
    "Texture2D": extract_texture2d,
    "Sprite": extract_sprite,
    "AudioClip": extract_audioclip,
    "Font": extract_font,
    "TextAsset": extract_textasset,
    "MonoBehaviour": extract_monobehaviour,
    "Mesh": extract_mesh,
}

ALL_TYPES = set(EXTRACTORS.keys())


def main() -> None:
    parser = argparse.ArgumentParser(description="Extract Unity assets to standard formats")
    parser.add_argument("input", help="Path to asset file, bundle, or directory")
    parser.add_argument("output", help="Output directory")
    parser.add_argument(
        "--types",
        default=",".join(sorted(ALL_TYPES)),
        help=f"Comma-separated asset types to extract (default: all). Available: {', '.join(sorted(ALL_TYPES))}",
    )
    args = parser.parse_args()

    input_path = Path(args.input)
    output_dir = Path(args.output)

    if not input_path.exists():
        print(f"Error: '{args.input}' does not exist")
        sys.exit(1)

    requested_types = set(t.strip() for t in args.types.split(","))
    unknown = requested_types - ALL_TYPES
    if unknown:
        print(f"Warning: Unknown types ignored: {', '.join(unknown)}")
    requested_types &= ALL_TYPES

    print(f"Loading: {args.input}")
    env = UnityPy.load(str(input_path))

    output_dir.mkdir(parents=True, exist_ok=True)

    stats: Counter = Counter()
    errors: Counter = Counter()

    for obj in env.objects:
        type_name = obj.type.name
        if type_name not in requested_types:
            continue

        extractor = EXTRACTORS.get(type_name)
        if not extractor:
            continue

        result = extractor(obj, output_dir)
        if result:
            if isinstance(result, list):
                stats[type_name] += len(result)
            else:
                stats[type_name] += 1
        else:
            errors[type_name] += 1

    print(f"\nExtraction complete -> {output_dir}")
    print("=" * 50)
    for type_name in sorted(stats.keys()):
        print(f"  {type_name}: {stats[type_name]} exported")
    if errors:
        print()
        for type_name in sorted(errors.keys()):
            print(f"  {type_name}: {errors[type_name]} failed")
    total = sum(stats.values())
    total_err = sum(errors.values())
    print(f"\nTotal: {total} exported, {total_err} failed")


if __name__ == "__main__":
    main()
