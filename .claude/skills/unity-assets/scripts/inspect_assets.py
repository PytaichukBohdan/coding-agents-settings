# /// script
# requires-python = ">=3.8"
# dependencies = ["UnityPy>=1.20"]
# ///
"""Inspect Unity asset files/bundles and print a summary of contents."""

import sys
import os
from collections import Counter
from pathlib import Path

import UnityPy


def format_size(size_bytes: int) -> str:
    for unit in ("B", "KB", "MB", "GB"):
        if size_bytes < 1024:
            return f"{size_bytes:.1f} {unit}"
        size_bytes /= 1024
    return f"{size_bytes:.1f} TB"


def inspect(input_path: str) -> None:
    path = Path(input_path)
    if not path.exists():
        print(f"Error: '{input_path}' does not exist")
        sys.exit(1)

    print(f"Loading: {input_path}")
    env = UnityPy.load(input_path)

    type_counts: Counter = Counter()
    type_names: dict[str, list[str]] = {}
    total_objects = 0

    for obj in env.objects:
        total_objects += 1
        type_name = obj.type.name
        type_counts[type_name] += 1

        try:
            name = obj.peek_name()
        except Exception:
            name = "<unnamed>"

        if type_name not in type_names:
            type_names[type_name] = []
        if len(type_names[type_name]) < 10:
            type_names[type_name].append(name)

    print(f"\nTotal objects: {total_objects}")
    print(f"Unique types:  {len(type_counts)}")
    print()

    print("=" * 60)
    print(f"{'Type':<25} {'Count':>8}")
    print("=" * 60)
    for type_name, count in type_counts.most_common():
        print(f"{type_name:<25} {count:>8}")
    print("=" * 60)

    # Container paths
    container = env.container
    if container:
        print(f"\nContainer entries: {len(container)}")
        print("-" * 60)
        for i, (cpath, obj) in enumerate(container.items()):
            if i >= 20:
                print(f"  ... and {len(container) - 20} more")
                break
            print(f"  {cpath} -> {obj.type.name}")

    # Sample names per type
    print("\nSample names per type:")
    print("-" * 60)
    for type_name in sorted(type_names.keys()):
        names = type_names[type_name]
        total = type_counts[type_name]
        print(f"\n  {type_name} ({total} total):")
        for name in names:
            print(f"    - {name}")
        if total > len(names):
            print(f"    ... and {total - len(names)} more")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: inspect_assets.py <path_to_asset_or_bundle_or_directory>")
        print()
        print("Supports: .assets, .bundle, .unity3d, asset bundle directories")
        sys.exit(1)

    inspect(sys.argv[1])
