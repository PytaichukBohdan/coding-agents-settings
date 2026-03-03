---
name: Unity Asset Tools
description: Extract, convert, and create Unity assets. Use when the user wants to extract assets from Unity games/projects (textures, models, audio, fonts, scripts), convert Unity assets to standard formats (PNG, WAV, FBX, OBJ), inspect Unity asset bundles, or generate C# editor scripts for procedural asset creation in Unity. Keywords - Unity, asset, extract, texture, mesh, AudioClip, bundle, prefab, procedural, editor script.
---

# Unity Asset Tools

## Instructions

This skill provides two main workflows:
1. **Extract & Convert** — pull assets out of Unity bundles/files into standard formats using Python (UnityPy)
2. **Create & Generate** — produce C# editor scripts that create Unity elements programmatically

### Prerequisites

- Python 3.8+ with `uv` (for extraction scripts)
- UnityPy library: `uv pip install UnityPy Pillow`
- For audio extraction with FMOD: `uv pip install pyfmodex` (optional)
- For C# generation: no dependencies (produces `.cs` files for Unity Editor)

---

## Workflow 1: Inspect Unity Assets

Before extracting, inspect what's inside an asset file/bundle.

**Use the inspection script:**
```bash
uv run .claude/skills/unity-assets/scripts/inspect_assets.py <path_to_asset_or_bundle>
```

This prints a summary: object counts by type, names, sizes, and container paths.

---

## Workflow 2: Extract Assets

### Quick Extraction (All Supported Types)

```bash
uv run .claude/skills/unity-assets/scripts/extract_assets.py <input_path> <output_dir> [--types Texture2D,AudioClip,Mesh,Sprite,Font,TextAsset,MonoBehaviour]
```

### Supported Asset Types and Output Formats

| Unity Type      | Output Format         | Notes                                    |
|----------------|-----------------------|------------------------------------------|
| Texture2D      | `.png`                | Full resolution, alpha preserved         |
| Sprite         | `.png`                | Cropped to sprite rect                   |
| AudioClip      | `.wav` / original     | Converted via samples dict               |
| Mesh           | `.obj`                | Vertices, normals, UVs, triangles        |
| Font           | `.ttf` / `.otf`       | Auto-detected by header                  |
| TextAsset      | `.txt` / `.bytes`     | Binary or text, auto-detected            |
| MonoBehaviour  | `.json`               | Serialized via typetree                  |
| Shader         | `.shader`             | Raw shader text                          |
| AnimationClip  | `.json`               | Keyframe data as JSON                    |

### Manual Extraction (Python API)

For custom extraction logic, use UnityPy directly:

```python
import UnityPy

env = UnityPy.load("path/to/assets")

# Iterate all objects
for obj in env.objects:
    if obj.type.name == "Texture2D":
        tex = obj.parse_as_object()
        tex.image.save(f"output/{tex.m_Name}.png")

# Or use container paths
for path, obj in env.container.items():
    print(f"{path} -> {obj.type.name}")
```

See [docs/unitypy_api.md](docs/unitypy_api.md) for the full API reference.

---

## Workflow 3: Edit / Replace Assets

UnityPy supports modifying assets in-place:

```python
import UnityPy
from PIL import Image

env = UnityPy.load("original.bundle")

for obj in env.objects:
    if obj.type.name == "Texture2D":
        tex = obj.parse_as_object()
        if tex.m_Name == "target_texture":
            tex.image = Image.open("replacement.png")
            tex.save()

# Save the modified bundle
with open("modified.bundle", "wb") as f:
    f.write(env.file.save())
```

---

## Workflow 4: Create Unity Elements via C# Editor Scripts

When the user wants to create Unity elements programmatically, generate C# scripts that run inside Unity Editor.

### Available Templates

Reference these templates and adapt them to the user's needs:

- [templates/ProceduralMeshGenerator.cs](templates/ProceduralMeshGenerator.cs) — Create meshes (plane, cube, cylinder, custom) with vertices, normals, UVs
- [templates/TextureGenerator.cs](templates/TextureGenerator.cs) — Generate textures procedurally (gradients, noise, patterns)
- [templates/PrefabFactory.cs](templates/PrefabFactory.cs) — Batch-create prefabs from imported assets
- [templates/AssetPostprocessor.cs](templates/AssetPostprocessor.cs) — Auto-process assets on import (resize textures, set compression, etc.)

### Generating a C# Script

1. Ask the user what they want to create (mesh type, texture pattern, prefab structure)
2. Read the relevant template from `templates/`
3. Adapt the template to the specific requirements
4. Save to the user's Unity project under `Assets/Editor/` (for editor scripts) or `Assets/Scripts/` (for runtime)

### Key Unity Editor APIs

```csharp
// Save a generated mesh as an asset
AssetDatabase.CreateAsset(mesh, "Assets/Generated/MyMesh.asset");

// Create a prefab from a GameObject
PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/Prefabs/MyPrefab.prefab");

// Generate a texture and save as PNG
File.WriteAllBytes("Assets/Textures/Generated.png", texture.EncodeToPNG());
AssetDatabase.Refresh();
```

---

## Decision Guide

| User wants to...                              | Use Workflow |
|-----------------------------------------------|-------------|
| See what's in a `.assets` / `.bundle` file    | 1 (Inspect) |
| Export textures/audio/models from Unity game   | 2 (Extract) |
| Replace a texture in an existing bundle        | 3 (Edit)    |
| Generate meshes/textures in Unity via code     | 4 (Create)  |
| Batch-process Unity project assets             | 4 (Create)  |

---

## Examples

### Example 1: Extract All Textures from a Game

User request: "Extract all textures from this Unity game"

1. Identify the asset files (usually in `<Game>_Data/` folder)
2. Run inspection:
   ```bash
   uv run .claude/skills/unity-assets/scripts/inspect_assets.py ~/Games/MyGame/MyGame_Data/
   ```
3. Extract textures:
   ```bash
   uv run .claude/skills/unity-assets/scripts/extract_assets.py ~/Games/MyGame/MyGame_Data/ ./extracted --types Texture2D,Sprite
   ```
4. Report results: number of files, total size, any errors

### Example 2: Convert Audio Assets to WAV

User request: "Get all audio files from this asset bundle"

1. Run:
   ```bash
   uv run .claude/skills/unity-assets/scripts/extract_assets.py game.bundle ./audio_out --types AudioClip
   ```
2. All audio clips are saved as `.wav` files in `./audio_out/`

### Example 3: Generate a Procedural Terrain Mesh

User request: "Create a C# script that generates a terrain mesh in Unity"

1. Read the mesh template: [templates/ProceduralMeshGenerator.cs](templates/ProceduralMeshGenerator.cs)
2. Adapt for heightmap-based terrain with configurable resolution
3. Save to `Assets/Editor/TerrainGenerator.cs`
4. Include menu item `Tools > Generate Terrain` for easy access

### Example 4: Replace a Texture in an Asset Bundle

User request: "Replace the logo texture in this bundle with my new image"

1. Inspect the bundle to find the texture name
2. Write a Python script using Workflow 3
3. Run it to produce the modified bundle
4. Verify by re-inspecting the modified bundle
