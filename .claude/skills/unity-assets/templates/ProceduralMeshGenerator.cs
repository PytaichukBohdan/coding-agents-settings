using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor tool for generating procedural meshes and saving them as assets.
/// Place in Assets/Editor/ to use via Tools menu.
/// </summary>
public class ProceduralMeshGenerator : EditorWindow
{
    enum MeshType { Plane, Cube, Cylinder, Sphere, Custom }

    MeshType meshType = MeshType.Plane;
    int resolution = 10;
    float size = 1f;
    string assetName = "GeneratedMesh";

    [MenuItem("Tools/Procedural Mesh Generator")]
    static void Init()
    {
        var window = GetWindow<ProceduralMeshGenerator>("Mesh Generator");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Procedural Mesh Generator", EditorStyles.boldLabel);

        meshType = (MeshType)EditorGUILayout.EnumPopup("Mesh Type", meshType);
        resolution = EditorGUILayout.IntSlider("Resolution", resolution, 2, 256);
        size = EditorGUILayout.FloatField("Size", size);
        assetName = EditorGUILayout.TextField("Asset Name", assetName);

        if (GUILayout.Button("Generate & Save"))
        {
            Mesh mesh = GenerateMesh();
            if (mesh != null)
            {
                SaveMeshAsset(mesh);
            }
        }

        if (GUILayout.Button("Generate in Scene"))
        {
            Mesh mesh = GenerateMesh();
            if (mesh != null)
            {
                CreateGameObject(mesh);
            }
        }
    }

    Mesh GenerateMesh()
    {
        return meshType switch
        {
            MeshType.Plane => GeneratePlane(resolution, size),
            MeshType.Cube => GenerateCube(size),
            MeshType.Cylinder => GenerateCylinder(resolution, size, size * 2f),
            MeshType.Sphere => GenerateSphere(resolution, size),
            _ => null
        };
    }

    // ─── Plane ───────────────────────────────────────────────

    static Mesh GeneratePlane(int res, float size)
    {
        var mesh = new Mesh { name = "ProceduralPlane" };

        int vertCount = (res + 1) * (res + 1);
        var vertices = new Vector3[vertCount];
        var normals = new Vector3[vertCount];
        var uvs = new Vector2[vertCount];

        float step = size / res;
        float half = size / 2f;

        for (int z = 0; z <= res; z++)
        {
            for (int x = 0; x <= res; x++)
            {
                int i = z * (res + 1) + x;
                vertices[i] = new Vector3(x * step - half, 0f, z * step - half);
                normals[i] = Vector3.up;
                uvs[i] = new Vector2((float)x / res, (float)z / res);
            }
        }

        int[] triangles = new int[res * res * 6];
        int t = 0;
        for (int z = 0; z < res; z++)
        {
            for (int x = 0; x < res; x++)
            {
                int i = z * (res + 1) + x;
                triangles[t++] = i;
                triangles[t++] = i + res + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + res + 1;
                triangles[t++] = i + res + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        return mesh;
    }

    // ─── Cube ────────────────────────────────────────────────

    static Mesh GenerateCube(float size)
    {
        var mesh = new Mesh { name = "ProceduralCube" };
        float h = size / 2f;

        mesh.vertices = new Vector3[]
        {
            // Front
            new(-h, -h, -h), new(-h, h, -h), new(h, h, -h), new(h, -h, -h),
            // Back
            new(h, -h, h), new(h, h, h), new(-h, h, h), new(-h, -h, h),
            // Top
            new(-h, h, -h), new(-h, h, h), new(h, h, h), new(h, h, -h),
            // Bottom
            new(-h, -h, h), new(-h, -h, -h), new(h, -h, -h), new(h, -h, h),
            // Left
            new(-h, -h, h), new(-h, h, h), new(-h, h, -h), new(-h, -h, -h),
            // Right
            new(h, -h, -h), new(h, h, -h), new(h, h, h), new(h, -h, h),
        };

        mesh.normals = new Vector3[]
        {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back,
            Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,
            Vector3.up, Vector3.up, Vector3.up, Vector3.up,
            Vector3.down, Vector3.down, Vector3.down, Vector3.down,
            Vector3.left, Vector3.left, Vector3.left, Vector3.left,
            Vector3.right, Vector3.right, Vector3.right, Vector3.right,
        };

        mesh.uv = new Vector2[]
        {
            new(0,0), new(0,1), new(1,1), new(1,0),
            new(0,0), new(0,1), new(1,1), new(1,0),
            new(0,0), new(0,1), new(1,1), new(1,0),
            new(0,0), new(0,1), new(1,1), new(1,0),
            new(0,0), new(0,1), new(1,1), new(1,0),
            new(0,0), new(0,1), new(1,1), new(1,0),
        };

        mesh.triangles = new int[]
        {
            0,1,2, 0,2,3,
            4,5,6, 4,6,7,
            8,9,10, 8,10,11,
            12,13,14, 12,14,15,
            16,17,18, 16,18,19,
            20,21,22, 20,22,23,
        };

        mesh.RecalculateBounds();
        return mesh;
    }

    // ─── Cylinder ────────────────────────────────────────────

    static Mesh GenerateCylinder(int segments, float radius, float height)
    {
        var mesh = new Mesh { name = "ProceduralCylinder" };
        // Simplified — side faces only
        int vertCount = (segments + 1) * 2;
        var vertices = new Vector3[vertCount];
        var normals = new Vector3[vertCount];
        var uvs = new Vector2[vertCount];

        float halfH = height / 2f;

        for (int i = 0; i <= segments; i++)
        {
            float angle = 2f * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 normal = new Vector3(x, 0, z).normalized;

            vertices[i * 2] = new Vector3(x, -halfH, z);
            vertices[i * 2 + 1] = new Vector3(x, halfH, z);
            normals[i * 2] = normal;
            normals[i * 2 + 1] = normal;
            uvs[i * 2] = new Vector2((float)i / segments, 0);
            uvs[i * 2 + 1] = new Vector2((float)i / segments, 1);
        }

        int[] triangles = new int[segments * 6];
        for (int i = 0; i < segments; i++)
        {
            int t = i * 6;
            int bl = i * 2;
            int tl = bl + 1;
            int br = bl + 2;
            int tr = bl + 3;
            triangles[t] = bl; triangles[t + 1] = tl; triangles[t + 2] = br;
            triangles[t + 3] = br; triangles[t + 4] = tl; triangles[t + 5] = tr;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        return mesh;
    }

    // ─── Sphere ──────────────────────────────────────────────

    static Mesh GenerateSphere(int res, float radius)
    {
        var mesh = new Mesh { name = "ProceduralSphere" };

        int vertCount = (res + 1) * (res + 1);
        var vertices = new Vector3[vertCount];
        var normals = new Vector3[vertCount];
        var uvs = new Vector2[vertCount];

        for (int lat = 0; lat <= res; lat++)
        {
            float theta = Mathf.PI * lat / res;
            for (int lon = 0; lon <= res; lon++)
            {
                float phi = 2f * Mathf.PI * lon / res;
                int i = lat * (res + 1) + lon;

                float x = Mathf.Sin(theta) * Mathf.Cos(phi);
                float y = Mathf.Cos(theta);
                float z = Mathf.Sin(theta) * Mathf.Sin(phi);

                vertices[i] = new Vector3(x, y, z) * radius;
                normals[i] = new Vector3(x, y, z);
                uvs[i] = new Vector2((float)lon / res, (float)lat / res);
            }
        }

        int[] triangles = new int[res * res * 6];
        int t = 0;
        for (int lat = 0; lat < res; lat++)
        {
            for (int lon = 0; lon < res; lon++)
            {
                int i = lat * (res + 1) + lon;
                triangles[t++] = i;
                triangles[t++] = i + res + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + res + 1;
                triangles[t++] = i + res + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        return mesh;
    }

    // ─── Helpers ─────────────────────────────────────────────

    void SaveMeshAsset(Mesh mesh)
    {
        string dir = "Assets/Generated/Meshes";
        if (!AssetDatabase.IsValidFolder("Assets/Generated"))
            AssetDatabase.CreateFolder("Assets", "Generated");
        if (!AssetDatabase.IsValidFolder(dir))
            AssetDatabase.CreateFolder("Assets/Generated", "Meshes");

        string path = $"{dir}/{assetName}.asset";
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = mesh;
        Debug.Log($"Mesh saved to {path}");
    }

    void CreateGameObject(Mesh mesh)
    {
        var go = new GameObject(assetName);
        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();
        mf.sharedMesh = mesh;
        mr.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
        Selection.activeGameObject = go;
        Undo.RegisterCreatedObjectUndo(go, "Create Procedural Mesh");
    }
}
