using UnityEngine;
using UnityEditor;

/// <summary>
/// Automatically processes assets on import.
/// Place in Assets/Editor/ — runs automatically when assets are imported.
/// Customize the rules below to match your project's needs.
/// </summary>
public class CustomAssetPostprocessor : AssetPostprocessor
{
    // ─── Texture Processing ──────────────────────────────────

    void OnPreprocessTexture()
    {
        var importer = (TextureImporter)assetImporter;

        // Auto-set sprites for textures in Sprites folder
        if (assetPath.Contains("/Sprites/"))
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 100;
            importer.filterMode = FilterMode.Point; // Pixel art friendly
            importer.textureCompression = TextureImporterCompression.Uncompressed;
        }

        // Auto-set normal maps
        if (assetPath.Contains("_Normal") || assetPath.Contains("_normal") || assetPath.Contains("_N."))
        {
            importer.textureType = TextureImporterType.NormalMap;
        }

        // Cap max texture size for UI elements
        if (assetPath.Contains("/UI/"))
        {
            importer.maxTextureSize = 1024;
            importer.textureCompression = TextureImporterCompression.Compressed;
        }

        // High-res for environment textures
        if (assetPath.Contains("/Environment/"))
        {
            importer.maxTextureSize = 4096;
            importer.mipmapEnabled = true;
            importer.streamingMipmaps = true;
        }
    }

    // ─── Model Processing ────────────────────────────────────

    void OnPreprocessModel()
    {
        var importer = (ModelImporter)assetImporter;

        // Standard import settings
        importer.importCameras = false;
        importer.importLights = false;
        importer.isReadable = false;

        // Enable mesh compression for non-character models
        if (!assetPath.Contains("/Characters/"))
        {
            importer.meshCompression = ModelImporterMeshCompression.Medium;
        }

        // Character-specific settings
        if (assetPath.Contains("/Characters/"))
        {
            importer.animationType = ModelImporterAnimationType.Human;
            importer.isReadable = true; // Needed for skinned mesh manipulation
        }

        // Props — no animation
        if (assetPath.Contains("/Props/"))
        {
            importer.animationType = ModelImporterAnimationType.None;
            importer.importAnimation = false;
            importer.generateSecondaryUV = true; // For lightmapping
        }
    }

    // ─── Audio Processing ────────────────────────────────────

    void OnPreprocessAudio()
    {
        var importer = (AudioImporter)assetImporter;

        // Short SFX: decompress on load
        if (assetPath.Contains("/SFX/"))
        {
            var settings = importer.defaultSampleSettings;
            settings.loadType = AudioClipLoadType.DecompressOnLoad;
            settings.compressionFormat = AudioCompressionFormat.PCM;
            importer.defaultSampleSettings = settings;
        }

        // Music: stream from disk
        if (assetPath.Contains("/Music/"))
        {
            var settings = importer.defaultSampleSettings;
            settings.loadType = AudioClipLoadType.Streaming;
            settings.compressionFormat = AudioCompressionFormat.Vorbis;
            settings.quality = 0.7f;
            importer.defaultSampleSettings = settings;
        }
    }

    // ─── Post-import Logging ─────────────────────────────────

    static void OnPostprocessAllAssets(
        string[] imported, string[] deleted, string[] moved, string[] movedFrom)
    {
        if (imported.Length > 0)
        {
            Debug.Log($"[AssetPostprocessor] Processed {imported.Length} imported assets");
        }
    }
}
