using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Editor tool for generating procedural textures.
/// Place in Assets/Editor/ to use via Tools menu.
/// </summary>
public class TextureGenerator : EditorWindow
{
    enum PatternType { SolidColor, Gradient, Checkerboard, PerlinNoise, Stripes, Radial }

    PatternType pattern = PatternType.PerlinNoise;
    int width = 512;
    int height = 512;
    Color colorA = Color.white;
    Color colorB = Color.black;
    float scale = 10f;
    string textureName = "GeneratedTexture";
    Texture2D preview;

    [MenuItem("Tools/Texture Generator")]
    static void Init()
    {
        var window = GetWindow<TextureGenerator>("Texture Generator");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Procedural Texture Generator", EditorStyles.boldLabel);

        pattern = (PatternType)EditorGUILayout.EnumPopup("Pattern", pattern);
        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        colorA = EditorGUILayout.ColorField("Color A", colorA);
        colorB = EditorGUILayout.ColorField("Color B", colorB);
        scale = EditorGUILayout.Slider("Scale / Frequency", scale, 0.1f, 100f);
        textureName = EditorGUILayout.TextField("Texture Name", textureName);

        EditorGUILayout.Space();

        if (GUILayout.Button("Preview"))
        {
            preview = Generate();
        }

        if (GUILayout.Button("Generate & Save as PNG"))
        {
            var tex = Generate();
            SaveTexture(tex);
        }

        if (preview != null)
        {
            EditorGUILayout.Space();
            GUILayout.Label("Preview:");
            float previewSize = Mathf.Min(position.width - 20, 256);
            GUILayout.Label(preview, GUILayout.Width(previewSize), GUILayout.Height(previewSize));
        }
    }

    Texture2D Generate()
    {
        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float u = (float)x / width;
                float v = (float)y / height;
                Color c = pattern switch
                {
                    PatternType.SolidColor => colorA,
                    PatternType.Gradient => Color.Lerp(colorA, colorB, u),
                    PatternType.Checkerboard => GenerateCheckerboard(x, y),
                    PatternType.PerlinNoise => GeneratePerlin(u, v),
                    PatternType.Stripes => GenerateStripes(x),
                    PatternType.Radial => GenerateRadial(u, v),
                    _ => Color.magenta
                };
                tex.SetPixel(x, y, c);
            }
        }

        tex.Apply();
        return tex;
    }

    Color GenerateCheckerboard(int x, int y)
    {
        int cellSize = Mathf.Max(1, (int)(width / scale));
        bool isA = ((x / cellSize) + (y / cellSize)) % 2 == 0;
        return isA ? colorA : colorB;
    }

    Color GeneratePerlin(float u, float v)
    {
        float noise = Mathf.PerlinNoise(u * scale, v * scale);
        return Color.Lerp(colorA, colorB, noise);
    }

    Color GenerateStripes(int x)
    {
        int cellSize = Mathf.Max(1, (int)(width / scale));
        return (x / cellSize) % 2 == 0 ? colorA : colorB;
    }

    Color GenerateRadial(float u, float v)
    {
        float dx = u - 0.5f;
        float dy = v - 0.5f;
        float dist = Mathf.Sqrt(dx * dx + dy * dy) * scale;
        float t = Mathf.PingPong(dist, 1f);
        return Color.Lerp(colorA, colorB, t);
    }

    void SaveTexture(Texture2D tex)
    {
        string dir = "Assets/Generated/Textures";
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string path = $"{dir}/{textureName}.png";
        byte[] png = tex.EncodeToPNG();
        File.WriteAllBytes(path, png);
        AssetDatabase.Refresh();

        // Set import settings
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Default;
            importer.SaveAndReimport();
        }

        Debug.Log($"Texture saved to {path} ({width}x{height})");
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
