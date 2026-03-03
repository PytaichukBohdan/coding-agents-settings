# UnityPy API Reference

Quick reference for the [UnityPy](https://github.com/K0lb3/UnityPy) library (v1.20+).

## Loading Assets

```python
import UnityPy

# From file path, bytes, or stream
env = UnityPy.load("path/to/file")
env = UnityPy.load(b"raw_bytes")

import io
env = UnityPy.load(io.BytesIO(data))
```

**Supported inputs:** `.assets`, `.bundle`, `.unity3d`, `.apk`, directories containing asset files.

## Iterating Objects

```python
# All objects across all loaded files
for obj in env.objects:
    print(obj.type.name, obj.path_id)

# Container-mapped objects (path -> object)
for path, obj in env.container.items():
    print(path, "->", obj.type.name)
```

## Parsing Objects

```python
# As typed Python class (properties like .m_Name, .m_Width, etc.)
instance = obj.parse_as_object()

# As dictionary (all fields as dict)
data = obj.parse_as_dict()

# Fast name peek (no full parse)
name = obj.peek_name()
```

## Supported Types

### Texture2D
```python
tex = obj.parse_as_object()
tex.m_Name       # str
tex.m_Width       # int
tex.m_Height      # int
tex.image         # PIL.Image
tex.image.save("output.png")
```

### Sprite
```python
sprite = obj.parse_as_object()
sprite.m_Name     # str
sprite.image      # PIL.Image (cropped to sprite rect)
sprite.image.save("output.png")
```

### AudioClip
```python
clip = obj.parse_as_object()
clip.m_Name       # str
clip.samples      # dict[str, bytes] — name: audio_data
for name, data in clip.samples.items():
    with open(name, "wb") as f:
        f.write(data)
```

### Font
```python
font = obj.parse_as_object()
font.m_Name       # str
font.m_FontData   # bytes (.ttf or .otf)
ext = ".otf" if font.m_FontData[:4] == b"OTTO" else ".ttf"
```

### TextAsset
```python
txt = obj.parse_as_object()
txt.m_Name        # str
txt.m_Script      # str (may contain binary as surrogateescape)
# For binary:
binary = txt.m_Script.encode("utf-8", "surrogateescape")
```

### MonoBehaviour
```python
data = obj.parse_as_dict()
# Returns full typetree as dict — structure depends on the script
```

### Mesh
```python
mesh = obj.parse_as_object()
mesh.m_Name       # str
# Use parse_as_dict() for vertex/triangle data
data = obj.parse_as_dict()
```

### Shader
```python
shader = obj.parse_as_object()
shader.m_Name     # str
# Raw shader data available via parse_as_dict()
```

## Modifying Assets

```python
# Via dictionary
raw = obj.parse_as_dict()
raw["m_Name"] = "NewName"
obj.patch(raw)

# Via parsed object
instance = obj.parse_as_object()
instance.m_Name = "NewName"
obj.patch(instance)

# Replace texture image
from PIL import Image
tex = obj.parse_as_object()
tex.image = Image.open("new_image.png")
tex.save()
```

## Saving Modified Files

```python
# Save single file
with open("modified.bundle", "wb") as f:
    f.write(env.file.save())

# Save with packing
with open("output.bundle", "wb") as f:
    f.write(env.file.save(packer="original"))
```

## Tips

- Use `obj.type.name` to check asset type before parsing
- `env.container` is empty for raw `.assets` files (no path mapping)
- `peek_name()` is much faster than full `parse_as_object()` for filtering
- Audio extraction requires FMOD support for some formats — install `pyfmodex` if needed
- Large bundles: iterate `env.objects` lazily, don't collect all into a list
