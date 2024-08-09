using System;
using System.IO;
using UnityEditor;
using UnityEngine;

static partial class CQ3DMapTools
{
    [Serializable]
    public class SceneGroundCombineConfig
    {
        public static SceneGroundCombineConfig Load(string fp)
        {
            var txt = File.ReadAllText(fp);
            if (!string.IsNullOrEmpty(txt))
            {
                return JsonUtility.FromJson<SceneGroundCombineConfig>(txt);
            }
            return null;
        }

        public string GenMeshName = null;

        public string GroundCombinedTex = null;

        public int GroundCombinedStride = 2;

        public string[] GroundTexCombineInfo = null;

        public int FindGroundTexIndexInCombined(string texName)
        {
            if (GroundTexCombineInfo != null)
            {
                return ArrayUtility.FindIndex(GroundTexCombineInfo, e => e.Equals(texName, StringComparison.OrdinalIgnoreCase));
            }
            return -1;
        }

        public int RandomGroundTexIndex()
        {
            return UnityEngine.Random.Range(0, GroundCombinedStride * GroundCombinedStride);
        }

        public Material LoadGroundCombinedMaterial()
        {
            var mat = AssetDatabase.LoadAssetAtPath<Material>(GroundCombinedTex);
            return mat;
        }
    }

#if ENABLE_3D

    [MenuItem("GameObject/Custom Map Tools/Combine Grid Ground", priority = 0)]
    static void CombineGround()
    {
        if (Selection.activeGameObject != null)
        {
            string scenePath = System.IO.Path.GetDirectoryName(UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).path);
            var cfg = SceneGroundCombineConfig.Load(scenePath + "/GroundCombine.json");
            if (cfg != null)
            {
                CombineGridMesh(Selection.activeGameObject, cfg);
            }
            else
            {
               
            }
        }
    }
    
    [MenuItem("GameObject/Custom Map Tools/Combine BuLou", priority = 0)]
    static void CombineBuLou()
    {
        if (Selection.activeGameObject != null)
        {
            CombineGridMesh(Selection.activeGameObject, false, null, 1, "_bulou.asset");
        }
    }
    
    [MenuItem("GameObject/Custom Map Tools/Zero Node", priority = 0)]
    static void Remove()
    {
        if (Selection.activeGameObject != null)
        {
            var t = Selection.activeGameObject.transform;
            var offset = t.localPosition;
            for (var i = t.childCount - 1; i >= 0; i--)
            {
                t.GetChild(i).localPosition += offset;
            }
            t.localPosition = Vector3.zero;
        }
    }


    #region utils

    static void SortGridGroundObject()
    {
        if (Selection.activeGameObject != null)
        {
            SortGridGroundObject(Selection.activeGameObject);
        }
    }

    public static int SortGridMeshRenderer(MeshRenderer x, MeshRenderer y)
    {
        return SortVector(x.transform.position, y.transform.position);
    }

    public static int SortVector(Vector3 x, Vector3 y)
    {
        var p1 = CalculateWeight(x);
        var p2 = CalculateWeight(y);
        var v = p1 - p2;
        if (v > int.MaxValue)
        {
            return int.MaxValue;
        }
        if (v < int.MinValue)
        {
            return int.MinValue;
        }
        return (int)v;
    }


    static List<MeshRenderer> SortGridGroundObject(GameObject go)
    {
        var mrs = new List<MeshRenderer>();
        go.GetComponentsInChildren(mrs);
        mrs.Sort(SortGridMeshRenderer);
        for (var i = 0; i < mrs.Count; i++)
        {
            mrs[i].name = i.ToString();
            mrs[i].transform.SetSiblingIndex(i);
        }
        return mrs;
    }

    static long CalculateWeight(Vector3 v)
    {
        long xv = Mathf.RoundToInt(v.x * 100);
        long zv = Mathf.RoundToInt(v.z * 100);
        long yv = Mathf.RoundToInt(v.y * 100);
        return (xv << 42) + (zv << 20) + yv;
    }


    static Vector3 Adjust000(Vector3 v)
    {
        var x = Mathf.RoundToInt(v.x * 100) * 0.01f;
        var y = Mathf.RoundToInt(v.y * 100) * 0.01f;
        var z = Mathf.RoundToInt(v.z * 100) * 0.01f;
        return new Vector3(x, y, z);
    }

    static void AddTrig(Vector3 p0, Vector3 p1, Vector3 p2, List<Vector3> verts, List<int> trigs, Dictionary<long, int> w_dic)
    {
        p0 = Adjust000(p0);
        p1 = Adjust000(p1);
        p2 = Adjust000(p2);

        var weight0 = CalculateWeight(p0);
        if (!w_dic.TryGetValue(weight0, out int idx0))
        {
            idx0 = verts.Count;
            verts.Add(p0);
            w_dic.Add(weight0, idx0);
        }

        var weight1 = CalculateWeight(p1);
        if (!w_dic.TryGetValue(weight1, out int idx1))
        {
            idx1 = verts.Count;
            verts.Add(p1);
            w_dic.Add(weight1, idx1);
        }

        var weight2 = CalculateWeight(p2);
        if (!w_dic.TryGetValue(weight2, out int idx2))
        {
            idx2 = verts.Count;
            verts.Add(p2);
            w_dic.Add(weight2, idx2);
        }

        trigs.Add(idx0);
        trigs.Add(idx1);
        trigs.Add(idx2);
    }


    static void AddQuad(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int uvTexIndex, int uvTexSplit, List<Vector3> verts, List<int> trigs, List<Vector2> uvs)
    {
        var idx0 = verts.Count;

        verts.Add(Adjust000(p0));
        verts.Add(Adjust000(p1));
        verts.Add(Adjust000(p2));
        verts.Add(Adjust000(p3));

        trigs.Add(idx0 + 0);
        trigs.Add(idx0 + 1);
        trigs.Add(idx0 + 2);

        trigs.Add(idx0 + 0);
        trigs.Add(idx0 + 2);
        trigs.Add(idx0 + 3);

        var uv_cellSize = 1.0f / uvTexSplit;
        var uv_cellx = uvTexIndex % uvTexSplit;
        var uv_celly = uvTexIndex / uvTexSplit;

        var uvMinX = uv_cellx * uv_cellSize;
        var uvMinY = uv_celly * uv_cellSize;
        var uvMaxX = (uv_cellx + 1) * uv_cellSize;
        var uvMaxY = (uv_celly + 1) * uv_cellSize;

        uvs.Add(new Vector2(uvMaxX, uvMinY));
        uvs.Add(new Vector2(uvMinX, uvMinY));
        uvs.Add(new Vector2(uvMinX, uvMaxY));
        uvs.Add(new Vector2(uvMaxX, uvMaxY));
    }

    #endregion

    static void CombineGridMesh(GameObject go, SceneGroundCombineConfig cfg)
    {
        var mrs = new List<MeshRenderer>();
        go.GetComponentsInChildren(mrs);
        var len = mrs.Count;
        var verts = new List<Vector3>(len * 4);
        var trigs = new List<int>(len * 2);
        var uvs = new List<Vector2>(len * 4);
        var norms = new List<Vector3>(len * 4);
        for (int i = 0; i < len; i++)
        {
            EditorUtility.DisplayProgressBar("正在生成...", mrs[i].name, Mathf.InverseLerp(0, len - 1, i));
            var _r = mrs[i];
            var _t = _r.transform;
            var _mainTex = _r.sharedMaterial.mainTexture;
            if (_mainTex == null)
            {
                continue;
            }
            var _uvTexIdx = cfg.FindGroundTexIndexInCombined(_mainTex.name);
            if (_uvTexIdx < 0)
            {
                _uvTexIdx = cfg.RandomGroundTexIndex();
            }
            var _m = _t.GetComponent<MeshFilter>().sharedMesh;
            if (_m == null)
            {
                continue;
            }

            var __verts = _m.vertices;
            if (__verts.Length == 4)
            {
                var p0 = _t.TransformPoint(__verts[0]);
                var p1 = _t.TransformPoint(__verts[1]);
                var p2 = _t.TransformPoint(__verts[2]);
                var p3 = _t.TransformPoint(__verts[3]);

                AddQuad(p0, p1, p2, p3, _uvTexIdx, cfg.GroundCombinedStride, verts, trigs, uvs);

                norms.Add(new Vector3(0, 1, 0));
                norms.Add(new Vector3(0, 1, 0));
                norms.Add(new Vector3(0, 1, 0));
                norms.Add(new Vector3(0, 1, 0));
            }
        }

        EditorUtility.ClearProgressBar();
        if (trigs.Count > 0)
        {
            var mesh = new Mesh();
            mesh.name = cfg.GenMeshName;
            mesh.SetVertices(verts);
            mesh.triangles = trigs.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.normals = norms.ToArray();

            var colors = new Color[verts.Count];
            for(var i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.white;
            }
            mesh.colors = colors;

            var g_go = new GameObject(cfg.GenMeshName);
            g_go.layer = LayerMask.NameToLayer("Ground");
            var g_mr = g_go.AddComponent<MeshRenderer>();
            g_mr.material = cfg.LoadGroundCombinedMaterial();

            var g_mf = g_go.AddComponent<MeshFilter>();
            g_mf.mesh = mesh;

            var suffix = "_" + cfg.GenMeshName + ".fbx";
            string fbx_path = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).path.Replace(".unity", suffix);
            UnityFBXExporter.FBXExporter.ExportGameObjToFBX(g_go, fbx_path);
            AssetDatabase.Refresh();
            g_mf.mesh = AssetDatabase.LoadAssetAtPath<Mesh>(fbx_path);
        }
    }


    static void CombineGridMesh(GameObject go, bool isNormal, Dictionary<string, int> uvTexInfo, int uvTexStride, string suffix)
    {
        var mrs = new List<MeshRenderer>();
        go.GetComponentsInChildren(mrs);
        var len = mrs.Count;
        var verts = new List<Vector3>(len * 4);
        var trigs = new List<int>(len * 2);
        var uvs = new List<Vector2>(len * 4);
        var norms = new List<Vector3>(len * 4);
        for (int i = 0; i < len; i++)
        {
            EditorUtility.DisplayProgressBar("正在生成...", mrs[i].name, Mathf.InverseLerp(0, len - 1, i));
            var _r = mrs[i];
            var _t = _r.transform;
            var _mainTex = _r.sharedMaterial.mainTexture;
            if (_mainTex == null)
            {
                continue;
            }
            if (uvTexInfo == null || !uvTexInfo.TryGetValue(_mainTex.name, out int _uvTexIdx))
            {
                _uvTexIdx = UnityEngine.Random.Range(0, uvTexStride);
            }

            var _m = _t.GetComponent<MeshFilter>().sharedMesh;
            if (_m == null)
            {
                continue;
            }

            var __verts = _m.vertices;
            if (__verts.Length == 4)
            {
                var p0 = _t.TransformPoint(__verts[0]);
                var p1 = _t.TransformPoint(__verts[1]);
                var p2 = _t.TransformPoint(__verts[2]);
                var p3 = _t.TransformPoint(__verts[3]);
                AddQuad(p0, p1, p2, p3, _uvTexIdx, uvTexStride, verts, trigs, uvs);
                norms.Add(new Vector3(0, 1, 0));
                norms.Add(new Vector3(0, 1, 0));
                norms.Add(new Vector3(0, 1, 0));
                norms.Add(new Vector3(0, 1, 0));
            }
        }

        EditorUtility.ClearProgressBar();
        if (trigs.Count > 0)
        {
            var mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.triangles = trigs.ToArray();
            mesh.uv = uvs.ToArray();

            if (isNormal)
            {
                mesh.normals = norms.ToArray();
            }
            var g_go = new GameObject(suffix.Substring(0, suffix.IndexOf('.')));
            g_go.layer = LayerMask.NameToLayer("Ground");
            var g_mr = g_go.AddComponent<MeshRenderer>();
            var g_mf = g_go.AddComponent<MeshFilter>();
            g_mf.mesh = mesh;

            string scenePath = UnityEditor.SceneManagement.EditorSceneManager.GetSceneAt(0).path;
            string meshScenePath = scenePath.Replace(".unity", suffix);
            AssetDatabase.CreateAsset(mesh, meshScenePath);
        }
    }
#endif

}