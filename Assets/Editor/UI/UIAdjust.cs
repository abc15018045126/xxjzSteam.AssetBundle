using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class UIAdjust
{
    public const string gameuiPath = "Assets/ABWork/ui/gameui.prefab";

    public static void AdjustGameUITransform()
    {
        var go = AssetDatabase.LoadAssetAtPath<GameObject>(gameuiPath);
        if (go == null)
        {
            return;
        }
        var curScene = EditorSceneManager.GetActiveScene();
        if (curScene.name != "test")
        {
            return;
        }
        var np = PrefabUtility.InstantiatePrefab(go) as GameObject;
        var ts = np.GetComponentsInChildren<RectTransform>(true);
        var hadChanged = false;
        for (var i = 0; i < ts.Length; i++)
        {
            var t = ts[i];
            var changed = false;
            if (AdjustLocalScale(t))
            {
                changed = true;
            }
            if (AdjustLocalPosition(t))
            {
                changed = true;
            }
            if (AdjustLocalRotation(t))
            {
                changed = true;
            }
            if (changed)
            {
                Debug.Log(AnimationUtility.CalculateTransformPath(t, null));
                hadChanged = true;
            }
        }
        if (hadChanged)
        {
            //PrefabUtility.SaveAsPrefabAsset(go, gameuiPath);
            EditorUtility.DisplayDialog("adjust gameui transform finish!", "Finish", "Close");
        }
        else
        {
            EditorUtility.DisplayDialog("finish!", "Finish", "Close");
        }
    }

    public static bool Approximately(float a, float b)
    {
        var v = Mathf.Abs(a - b);
        return v < 0.001f;
    }

    private static bool Adjust(float value, out float ret)
    {
        if (value != 0 && Approximately(value, 0))
        {
            ret = 0;
            return true;
        }
        if (value != 1 && Approximately(value, 1))
        {
            ret = 1;
            return true;
        }
        ret = 0;
        return false;
    }

    private static bool AdjustLocalPosition(RectTransform rt)
    {
        var p = rt.localPosition;

        var hadChanged = false;

        if (Adjust(p.x, out float nv))
        {
            hadChanged = true;
            p.x = nv;
        }
        if (Adjust(p.y, out nv))
        {
            hadChanged = true;
            p.y = nv;
        }
        if (Adjust(p.z, out nv))
        {
            hadChanged = true;
            p.z = nv;
        }
        if (hadChanged)
        {
            rt.localPosition = p;
        }

        return hadChanged;
    }

    private static bool AdjustLocalScale(RectTransform rt)
    {
        var p = rt.localScale;
        var hadChanged = false;

        if (Adjust(p.x, out float nv))
        {
            hadChanged = true;
            p.x = nv;
        }
        if (Adjust(p.y, out nv))
        {
            hadChanged = true;
            p.y = nv;
        }
        if (Adjust(p.z, out nv))
        {
            hadChanged = true;
            p.z = nv;
        }
        if (hadChanged)
        {
            rt.localScale = p;
        }

        return hadChanged;
    }

    private static bool AdjustLocalRotation(RectTransform rt)
    {
        return false;
    }

}