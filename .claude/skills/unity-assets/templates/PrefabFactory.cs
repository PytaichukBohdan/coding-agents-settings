using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Editor tool for batch-creating prefabs from meshes, textures, or other assets.
/// Place in Assets/Editor/ to use via Tools menu.
/// </summary>
public class PrefabFactory : EditorWindow
{
    string inputFolder = "Assets/Models";
    string outputFolder = "Assets/Prefabs/Generated";
    Material defaultMaterial;
    bool addCollider = true;
    bool addRigidbody = false;

    [MenuItem("Tools/Prefab Factory")]
    static void Init()
    {
        var window = GetWindow<PrefabFactory>("Prefab Factory");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Batch Prefab Creator", EditorStyles.boldLabel);

        inputFolder = EditorGUILayout.TextField("Input Folder (meshes)", inputFolder);
        outputFolder = EditorGUILayout.TextField("Output Folder (prefabs)", outputFolder);
        defaultMaterial = (Material)EditorGUILayout.ObjectField("Default Material", defaultMaterial, typeof(Material), false);
        addCollider = EditorGUILayout.Toggle("Add MeshCollider", addCollider);
        addRigidbody = EditorGUILayout.Toggle("Add Rigidbody", addRigidbody);

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Prefabs from All Meshes"))
        {
            CreatePrefabsFromMeshes();
        }

        if (GUILayout.Button("Create Prefab from Selection"))
        {
            CreatePrefabFromSelection();
        }
    }

    void CreatePrefabsFromMeshes()
    {
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string[] guids = AssetDatabase.FindAssets("t:Mesh", new[] { inputFolder });
        int created = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (mesh == null) continue;

            GameObject go = CreateGameObjectFromMesh(mesh);
            string prefabPath = $"{outputFolder}/{mesh.name}.prefab";
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            DestroyImmediate(go);
            created++;
        }

        AssetDatabase.Refresh();
        Debug.Log($"Created {created} prefabs in {outputFolder}");
    }

    void CreatePrefabFromSelection()
    {
        if (Selection.activeGameObject == null)
        {
            EditorUtility.DisplayDialog("Error", "Select a GameObject in the scene first.", "OK");
            return;
        }

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        var go = Selection.activeGameObject;
        string prefabPath = $"{outputFolder}/{go.name}.prefab";
        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        AssetDatabase.Refresh();
        Debug.Log($"Prefab saved to {prefabPath}");
    }

    GameObject CreateGameObjectFromMesh(Mesh mesh)
    {
        var go = new GameObject(mesh.name);
        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();
        mf.sharedMesh = mesh;

        if (defaultMaterial != null)
            mr.sharedMaterial = defaultMaterial;
        else
            mr.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

        if (addCollider)
        {
            var col = go.AddComponent<MeshCollider>();
            col.sharedMesh = mesh;
            col.convex = addRigidbody;
        }

        if (addRigidbody)
            go.AddComponent<Rigidbody>();

        return go;
    }
}
