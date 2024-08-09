#define ENABLE_2D

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class CustomToolsMenu
{

#if ENABLE_3D

    [MenuItem("CustomTools/Create all Monster AnimatorController", false, 60)]
    static void CreateAllMonsterAnimatorController()
    {
        var allAssets = AssetDatabase.GetAllAssetPaths();
        var monsterFbxList = ArrayUtility.FindAll(allAssets, (e) =>
        {
            if (!e.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (e.StartsWith("Assets/Models/Monsters") || e.StartsWith("Assets/Models/Monsters2") || e.StartsWith("Assets/Models/npc"))
            {
                if (int.TryParse(Path.GetFileNameWithoutExtension(e), out int id))
                {
                    return id >= 3000 && id < 10000;
                }
            }
            return false;
        });
        for (int i = 0, len = monsterFbxList.Count; i < len; i++)
        {
            var mf = monsterFbxList[i];
            var id = Path.GetFileNameWithoutExtension(mf);
            EditorUtility.DisplayProgressBar("Clear All AB Names...", id, Mathf.InverseLerp(0, len, i));
            CQ3DAnimTools.CreateAnimatorController(mf);
        }
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("AnimatorController", "Create all Monster AnimatorController Done!", "Close");
    }

    #region Create AnimatorController

    [MenuItem("Assets/Create AnimatorController", false)]
    static void CreateAnimatorController()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeGameObject);
        CQ3DAnimTools.CreateAnimatorController(path);
    }

    [MenuItem("Assets/Create AnimatorController", true)]
    static bool ValidateAnimatorControllerSelection()
    {
        if (Selection.activeGameObject != null)
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeGameObject);
            if (path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    [MenuItem("CustomTools/Export NavMesh")]
    static void ExportNavMesh()
    {
        NavMeshExport.Export();
    }

#endif

    [MenuItem("CustomTools/Export NavMesh To Tilemap")]
    static void ExportNavMesh2Tilemap()
    {
        NavMeshExport.ExportTilemapData();
    }

#if ENABLE_2D

    [MenuItem("GameObject/Custom Map Tools/Load TileMapData", priority = 0)]
    static void LoadTileMap()
    {
        if (Selection.activeGameObject == null)
        {
            return;
        }
        var scn = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0);
        if (scn == null)
        {
            return;
        }
        var gtm = Selection.activeGameObject.GetComponent<GameTilemap>();
        if (gtm == null)
        {
            return;
        }
        if (gtm.transform.childCount > 0)
        {
            return;
        }
        var workspaceDir = Directory.GetParent(Directory.GetCurrentDirectory());
        var binFileRoot = Path.Combine(workspaceDir.FullName, @"TilemapExport/output");
        var binDataName = string.Format("{0}.bin", scn.name);
        var binDataPath = Path.Combine(binFileRoot, binDataName);
        if (File.Exists(binDataPath))
        {
            CQ2DTilemapLoader.Load(binDataPath, gtm, scn.name);
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scn);
        }
        else
        {
            EditorUtility.DisplayDialog("ERROR", "未找到数据!" + binDataName, "Close");
        }
    }

    [MenuItem("GameObject/Custom Map Tools/Load TileMapData-DataOnly", priority = 0)]
    static void LoadTileMapDataLayer()
    {
        if (Selection.activeGameObject == null)
        {
            return;
        }
        var scn = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0);
        if (scn == null)
        {
            return;
        }
        var gtm = Selection.activeGameObject.GetComponent<GameTilemap>();
        if (gtm == null)
        {
            return;
        }
        var workspaceDir = Directory.GetParent(Directory.GetCurrentDirectory());
        var binFileRoot = Path.Combine(workspaceDir.FullName, @"TilemapExport/output");
        var binDataName = string.Format("{0}.bin", scn.name);
        var binDataPath = Path.Combine(binFileRoot, binDataName);
        if (File.Exists(binDataPath))
        {
            CQ2DTilemapLoader.LoadDataOnly(binDataPath, gtm, scn.name);
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scn);
        }
        else
        {
            EditorUtility.DisplayDialog("ERROR", "未找到数据!" + binDataName, "Close");
        }
    }
#endif

    [MenuItem("CustomTools/BMFont", false, 12)]
    private static void BMFontTools()
    {
        BMFont bmFont = new BMFont();
        bmFont.Show();
    }


    [MenuItem("CustomTools/Adjust gameui Transform", false, 12)]
    static void AdjustGameUITransform()
    {
        UIAdjust.AdjustGameUITransform();
    }



    [MenuItem("CustomTools/导出地图阻挡数据", false, 60)]
    static void ExportBlockData()
    {
        EditorUtility.DisplayDialog("ERROR", "还未实现!", "Close");
    }


    //[MenuItem("CustomTools/Build 2D frame Animaiton", false, 60)]
    //static void Build2DAniamtion()
    //{
    //    var currentSelects = Selection.assetGUIDs;
    //    if (currentSelects == null || currentSelects.Length == 0)
    //    {
    //        return;
    //    }
    //    var path = AssetDatabase.GUIDToAssetPath(currentSelects[0]);
    //    CQ2DAnimTools.BuildAniamtion(path);
    //}

    //[MenuItem("CustomTools/Build 2D frame Animaiton", true)]
    //static bool Validate2DAnimaPathSelection()
    //{
    //    var currentSelects = Selection.assetGUIDs;
    //    if (currentSelects == null || currentSelects.Length == 0)
    //    {
    //        return false;
    //    }
    //    var path = AssetDatabase.GUIDToAssetPath(currentSelects[0]);
    //    return CQ2DAnimTools.IsMatchAniamtionPath(path);
    //}

    //[MenuItem("CustomTools/Build 2D frame Animaiton SpriteSheet(TP)", false, 60)]
    //static void Build2DAniamtionTP()
    //{
    //    var currentSelects = Selection.assetGUIDs;
    //    if (currentSelects == null || currentSelects.Length == 0)
    //    {
    //        return;
    //    }
    //    var path = AssetDatabase.GUIDToAssetPath(currentSelects[0]);
    //    CQ2DAnimTools.BuildAniamtionTP(path);
    //}


    [MenuItem("CustomTools/Build 2D frame Animaiton SpriteSheet(TP)", true)]
    static bool Validate2DTPAnimaPathSelection()
    {
        var currentSelects = Selection.assetGUIDs;
        if (currentSelects == null || currentSelects.Length == 0)
        {
            return false;
        }
        var path = AssetDatabase.GUIDToAssetPath(currentSelects[0]);
        return CQ2DAnimTools.IsMatchAniamtionPathTP(path);
    }

    [MenuItem("CustomTools/Setup SpriteSheet(TP)", false, 60)]
    static void SetupSpriteSheetTP()
    {
        var currentSelects = Selection.assetGUIDs;
        if (currentSelects == null || currentSelects.Length == 0)
        {
            return;
        }
        var path = AssetDatabase.GUIDToAssetPath(currentSelects[0]);
        TPAtlasSet.CreateFromJsonFile(path);
    }


    [MenuItem("CustomTools/Setup SpriteSheet(TP)", true)]
    static bool ValidateSpriteSheetTPPathSelection()
    {
        var currentSelects = Selection.assetGUIDs;
        if (currentSelects == null || currentSelects.Length == 0)
        {
            return false;
        }
        var path = AssetDatabase.GUIDToAssetPath(currentSelects[0]);
        return path.EndsWith(".json");
    }

    private static Mesh CreateXZSlicedMesh()
    {

        var mesh = new Mesh();
        var vertices = new Vector3[16];

        vertices[0] = new Vector3(-20, 0, -30);
        vertices[1] = new Vector3(-10, 0, -30);
        vertices[2] = new Vector3(+10, 0, -30);
        vertices[3] = new Vector3(+20, 0, -30);

        vertices[4] = new Vector3(-20, 0, -10);
        vertices[5] = new Vector3(-10, 0, -10);
        vertices[6] = new Vector3(+10, 0, -10);
        vertices[7] = new Vector3(+20, 0, -10);

        vertices[8] = new Vector3(-20, 0, +10);
        vertices[9] = new Vector3(-10, 0, +10);
        vertices[10] = new Vector3(+10, 0, +10);
        vertices[11] = new Vector3(+20, 0, +10);

        vertices[12] = new Vector3(-20, 0, +30);
        vertices[13] = new Vector3(-10, 0, +30);
        vertices[14] = new Vector3(+10, 0, +30);
        vertices[15] = new Vector3(+20, 0, +30);
        mesh.vertices = vertices;

        var ts = new int[18 * 3];
        for (var c = 0; c < 3; c++)
        {
            for (var r = 0; r < 3; r++)
            {
                var q_idx = (c + r * 3) * 6;

                ts[q_idx + 0] = c + r * 4;
                ts[q_idx + 1] = ts[q_idx + 0] + 4;
                ts[q_idx + 2] = ts[q_idx + 1] + 1;

                ts[q_idx + 3] = ts[q_idx + 0];
                ts[q_idx + 4] = ts[q_idx + 2];
                ts[q_idx + 5] = ts[q_idx + 0] + 1;
            }
        }
        mesh.triangles = ts;

        var uv = new Vector2[16];
        uv[0] = new Vector2(0.0f, 0.0f);
        uv[1] = new Vector2(0.1f, 0.0f);
        uv[2] = new Vector2(0.9f, 0.0f);
        uv[3] = new Vector2(1.0f, 0.0f);

        uv[4] = new Vector2(0.0f, 0.1f);
        uv[5] = new Vector2(0.1f, 0.1f);
        uv[6] = new Vector2(0.9f, 0.1f);
        uv[7] = new Vector2(1.0f, 0.1f);

        uv[8] = new Vector2(0.0f, 0.9f);
        uv[9] = new Vector2(0.1f, 0.9f);
        uv[10] = new Vector2(0.9f, 0.9f);
        uv[11] = new Vector2(1.0f, 0.9f);

        uv[12] = new Vector2(0.0f, 1.0f);
        uv[13] = new Vector2(0.1f, 1.0f);
        uv[14] = new Vector2(0.9f, 1.0f);
        uv[15] = new Vector2(1.0f, 1.0f);
        mesh.uv = uv;

        return mesh;
    }

    [MenuItem("CustomTools/Export XZSlicedMesh", false)]
    public static void ExportXZSlicedMesh()
    {
        var mesh = CreateXZSlicedMesh();
        AssetDatabase.CreateAsset(mesh, "Assets/xzSlicedMesh.asset");
        EditorUtility.DisplayDialog("ExportNavMesh Success", "Finish", "Close");
    }
}