using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class CQ2DAnimTools : Editor
{
    #region FrameInfo

    public class FrameInfo
    {
        public static FrameInfo CreateFromImagePath(int seq, string frameLocation, string imgPath)
        {
            var sp = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(imgPath));
            var ret = new FrameInfo(seq, frameLocation, sp);
            return ret;
        }

        public readonly string frameLocation;
        public readonly int seq;
        public readonly Sprite sprite;

        public FrameInfo(int seq, string frameLocation, Sprite sp)
        {
            this.seq = seq;
            this.frameLocation = frameLocation;
            this.sprite = sp;
        }

        public FrameInfo Clone4Interpolation(int seq)
        {
            return new FrameInfo(seq, this.frameLocation, sprite);
        }

    }

    #endregion

    static readonly Regex UnitAnimMatcher = new Regex(@"^Assets/UnitsAnim/(?<unitname>(\d{4}))$");

    static readonly Regex WeaponAnimMatcher = new Regex(@"^Assets/WeaponAnim/(?<weaponname>(\d{4}))$");


    static readonly Regex UnitAnimMatcherTP = new Regex(@"^Assets/UnitsAnim/(?<unitname>(\d{4}))_0.json");

    static readonly Regex WeaponAnimMatcherTP = new Regex(@"^Assets/WeaponAnim/(?<weaponname>(\d{4}))_0.json");


    //生成出的AnimationController的路径
    static string AutoGenAnimationControllerPath = "Assets/AutoGen/AnimationControllers";
    //生成出的Animation的路径
    static string AutoGenAnimationPath = "Assets/AutoGen/Animations";

    static string AutoGenPrefabPath = "Assets/AutoGen/UnitsPrefab";

    const int defaultFrameRate = 10;

    //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
    const float defaultFrameTime = 1 / 10f;

    static readonly Regex AnimImageNameMatch = new Regex(@"^(?<id>(\d{4}))?(?<dir>(\d{1}))(?<ani>(\d{2}))(?<seq>(\d{2}))$");

    public static readonly string[] LoopAnimsNamePrefix =
    {
        "idle",
        "run",
        "walk"
    };


    public static readonly string[] AnimsNames =
    {
        "idle",         // --00
        "fight_idle",   // --01
        "attack",       // --02
        "cast",         // --03
        "hurt",         // --04
        "dead",         // --05
        "run",          // --06
        "walk",         // --07
        "attack2",      // --08
        "relax"         // --09
    };

    public const byte DIR_NUM = 8;

    public static bool IsMatchAniamtionPath(string path)
    {
        return UnitAnimMatcher.IsMatch(path) || WeaponAnimMatcher.IsMatch(path);
    }

    public static void BuildAniamtion(string path)
    {
        var m = UnitAnimMatcher.Match(path);
        if (m.Success)
        {
            var uas = SplitUnitAnimtionImages(path);
            if (uas != null && uas.Count > 0)
            {
                TryBuildUnitAnimation(uas, m.Groups["unitname"].Value);
            }
        }
        m = WeaponAnimMatcher.Match(path);
        if (m.Success)
        {
            var uas = SplitUnitAnimtionImages(path);
            if (uas != null && uas.Count > 0)
            {
                TryBuildUnitAnimation(uas, m.Groups["weaponname"].Value);
            }
        }
    }

    static Dictionary<string, List<FrameInfo>> SplitUnitAnimtionImages(string path)
    {
        var images = Directory.GetFiles(path, "*.png");
        if (images == null || images.Length == 0)
        {
            return null;
        }
        var dic = new Dictionary<string, List<FrameInfo>>();
        foreach (var imgPath in images)
        {
            var imgName = Path.GetFileNameWithoutExtension(imgPath);
            // 同一张图片可以用于多个帧
            var frameLocations = imgName.Split('_');
            for (var i = 0; i < frameLocations.Length; i++)
            {
                var frameLocation = frameLocations[i];
                var mr = AnimImageNameMatch.Match(frameLocation);
                if (!mr.Success)
                {
                    continue;
                }
                var dir_str = mr.Groups["dir"].Value;
                if (!int.TryParse(dir_str, out int dir))
                {
                    continue;
                }
                var ani_str = mr.Groups["ani"].Value;
                if (!int.TryParse(ani_str, out int ani))
                {
                    continue;
                }
                var seq_str = mr.Groups["seq"].Value;
                if (!int.TryParse(seq_str, out int seq) || seq == 0)
                {
                    continue;
                }
                //2021-1-18 修改 帧序从1开始
                seq -= 1;

                var ani_name = AnimsNames[ani];
                if (string.IsNullOrEmpty(ani_name))
                {
                    Debug.LogError("图片命名规则适配错误:" + imgName);
                    continue;
                }

                var ani_full_name = string.Format("{0}_{1:D1}", ani_name, dir);
                var _f = FrameInfo.CreateFromImagePath(seq, frameLocation, imgPath);
                if (dic.TryGetValue(ani_full_name, out List<FrameInfo> frameList))
                {
                    frameList.Add(_f);
                }
                else
                {
                    frameList = new List<FrameInfo> { _f };
                    dic.Add(ani_full_name, frameList);
                }
            }
        }
        foreach (var iter in dic)
        {
            var frameList = iter.Value;
            frameList.Sort(ComparisonFrameInfo);
            // 向前补帧
            if (frameList[0].seq != 0)
            {
                frameList.Insert(0, frameList[0].Clone4Interpolation(0));
            }
        }
        return dic;
    }


    public static bool IsMatchAniamtionPathTP(string path)
    {
        return UnitAnimMatcherTP.IsMatch(path) || WeaponAnimMatcherTP.IsMatch(path);
    }


    internal static void BuildAniamtionTP(string path)
    {
        var m = UnitAnimMatcherTP.Match(path);
        if (m.Success)
        {
            var uas = SplitUnitAnimtionImagesTP(path);
            if (uas != null && uas.Count > 0)
            {
                TryBuildUnitAnimation(uas, m.Groups["unitname"].Value);
            }
        }
        m = WeaponAnimMatcherTP.Match(path);
        if (m.Success)
        {
            var uas = SplitUnitAnimtionImagesTP(path);
            if (uas != null && uas.Count > 0)
            {
                TryBuildUnitAnimation(uas, m.Groups["weaponname"].Value);
            }
        }
    }

    static Dictionary<string, List<FrameInfo>> SplitUnitAnimtionImagesTP(string path)
    {
        var tpAltasSet = TPAtlasSet.CreateFromJsonFile(path);
        if (tpAltasSet == null || tpAltasSet.atlasList.Count == 0)
        {
            return null;
        }

        var dic = new Dictionary<string, List<FrameInfo>>();

        foreach (var tpAltas in tpAltasSet.atlasList)
        {
            foreach (var kv in tpAltas.frames)
            {
                var imgName = Path.GetFileNameWithoutExtension(kv.Key);
                var _sp = tpAltasSet.GetSprite(kv.Key);
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(_sp, out string guid, out long localId))
                {
                    Debug.Log(localId);
                }

                var frameLocations = imgName.Split('_');
                for (var i = 0; i < frameLocations.Length; i++)
                {
                    var frameLocation = frameLocations[i];
                    var mr = AnimImageNameMatch.Match(frameLocation);
                    if (!mr.Success)
                    {
                        continue;
                    }
                    var dir_str = mr.Groups["dir"].Value;
                    if (!int.TryParse(dir_str, out int dir))
                    {
                        continue;
                    }
                    var ani_str = mr.Groups["ani"].Value;
                    if (!int.TryParse(ani_str, out int ani))
                    {
                        continue;
                    }
                    var seq_str = mr.Groups["seq"].Value;
                    if (!int.TryParse(seq_str, out int seq) || seq == 0)
                    {
                        continue;
                    }
                    //2021-1-18 修改 帧序从1开始
                    seq -= 1;

                    var ani_name = AnimsNames[ani];
                    if (string.IsNullOrEmpty(ani_name))
                    {
                        Debug.LogError("图片命名规则适配错误:" + imgName);
                        continue;
                    }

                    var ani_full_name = string.Format("{0}_{1:D1}", ani_name, dir);
                    var _f = new FrameInfo(seq, frameLocation, _sp);
                    if (dic.TryGetValue(ani_full_name, out List<FrameInfo> frameList))
                    {
                        frameList.Add(_f);
                    }
                    else
                    {
                        frameList = new List<FrameInfo> { _f };
                        dic.Add(ani_full_name, frameList);
                    }
                }
            }
        }

        foreach (var iter in dic)
        {
            var frameList = iter.Value;
            frameList.Sort(ComparisonFrameInfo);
            // 向前补帧
            if (frameList[0].seq != 0)
            {
                if (frameList[0].sprite != null)
                {
                    frameList.Insert(0, frameList[0].Clone4Interpolation(0));
                }
            }
        }
        return dic;
    }



    public static int ComparisonFrameInfo(FrameInfo x, FrameInfo y)
    {
        if (x.frameLocation == y.frameLocation)
        {
            return x.seq.CompareTo(y.seq);
        }
        else
        {
            return x.frameLocation.CompareTo(y.frameLocation);
        }
    }

    static void TryBuildUnitAnimation(IDictionary<string, List<FrameInfo>> animsFrames, string unitName)
    {
        var animClips = new List<AnimationClip>();

        Sprite coverSprite = null;
        foreach (var iter in animsFrames)
        {
            var clip = BuildAnimationClip(iter.Value, unitName, iter.Key, defaultFrameRate, defaultFrameTime, ref coverSprite);
            if (clip != null)
            {
                animClips.Add(clip);
            }
        }

        if (!Directory.Exists(AutoGenAnimationControllerPath))
        {
            Directory.CreateDirectory(AutoGenAnimationControllerPath);
        }

        var ac = BuildAnimationController(animClips, AutoGenAnimationControllerPath, unitName);
        if (!Directory.Exists(AutoGenPrefabPath))
        {
            Directory.CreateDirectory(AutoGenPrefabPath);
        }

        BuildPrefab(AutoGenPrefabPath, unitName, coverSprite, ac);
    }


    static AnimationClip BuildAnimationClip(List<FrameInfo> frameInfos, string unitName, string animName, int frameRate, float frameTime, ref Sprite coverSprite)
    {
        var clip = new AnimationClip();
        var curveBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };
        var keyFrames = new ObjectReferenceKeyframe[frameInfos.Count];
        for (int i = 0; i < keyFrames.Length; i++)
        {
            var sprite = frameInfos[i].sprite;
            if (coverSprite == null)
            {
                coverSprite = sprite;
            }
            keyFrames[i] = new ObjectReferenceKeyframe
            {
                time = frameTime * frameInfos[i].seq,
                value = sprite
            };
        }
        clip.frameRate = frameRate;
        //设置循环
        if (IsNeedSetLoopAnim(animName))
        {
            var serializedClip = new SerializedObject(clip);
            var clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"))
            {
                loopTime = true
            };
            serializedClip.ApplyModifiedProperties();
        }
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);


        var saveDirPath = Path.Combine(AutoGenAnimationPath, unitName);
        if (!Directory.Exists(saveDirPath))
        {
            Directory.CreateDirectory(saveDirPath);
        }

        var savePath = Path.Combine(saveDirPath, animName + ".anim");

        AssetDatabase.CreateAsset(clip, savePath);
        AssetDatabase.SaveAssets();
        return clip;
    }

    static AnimatorController BuildAnimationController(List<AnimationClip> clips, string saveDirPath, string name)
    {
        var savePath = Path.Combine(saveDirPath, name + ".controller");
        var ac = AnimatorController.CreateAnimatorControllerAtPath(savePath);

        ac.AddParameter("st0", AnimatorControllerParameterType.Int);
        ac.AddParameter("dir", AnimatorControllerParameterType.Int);

        var layer = ac.layers[0];
        var sm = layer.stateMachine;

        var dirStsDic = new Dictionary<int, AnimatorState[]>();
        foreach (var clip in clips)
        {
            var st = sm.AddState(clip.name);
            st.motion = clip;
            if (GetAnimTransitionParam(clip.name, out int paramSt, out int paramDir))
            {
                if (dirStsDic.TryGetValue(paramSt, out AnimatorState[] dirSts))
                {
                    dirSts[paramDir] = st;
                }
                else
                {
                    dirSts = new AnimatorState[DIR_NUM];
                    dirSts[paramDir] = st;
                    dirStsDic.Add(paramSt, dirSts);
                }
            }
        }
        //-- 链接 fight_idle -> idle
        dirStsDic.TryGetValue(0, out AnimatorState[] idle_dir_sts);
        dirStsDic.TryGetValue(1, out AnimatorState[] fight_idle_dir_sts);
        LinkAnimatorState(fight_idle_dir_sts, idle_dir_sts, null);
        //-- 链接 attack -> fight_idle / idle
        dirStsDic.TryGetValue(2, out AnimatorState[] attack_dir_sts);
        LinkAnimatorState(attack_dir_sts, fight_idle_dir_sts, idle_dir_sts);
        //-- 链接 cast -> fight_idle / idle
        dirStsDic.TryGetValue(3, out AnimatorState[] cast_dir_sts);
        LinkAnimatorState(cast_dir_sts, fight_idle_dir_sts, idle_dir_sts);
        //-- 链接 attack2 -> fight_idle / idle
        dirStsDic.TryGetValue(8, out AnimatorState[] attack2_dir_sts);
        LinkAnimatorState(cast_dir_sts, fight_idle_dir_sts, idle_dir_sts);
        //-- 链接 relax -> idle
        dirStsDic.TryGetValue(9, out AnimatorState[] relax_dir_sts);
        LinkAnimatorState(relax_dir_sts, idle_dir_sts, null);

        AssetDatabase.SaveAssets();
        return ac;
    }

    static AnimatorStateTransition[] LinkAnimatorState(AnimatorState[] src_dir_sts, AnimatorState[] dst_dir_sts, AnimatorState[] second_dir_sts)
    {
        if (src_dir_sts == null)
        {
            return null;
        }
        var ret = new AnimatorStateTransition[DIR_NUM];
        AnimatorState src_st, dst_st;
        for (var i = 0; i < DIR_NUM; i++)
        {
            src_st = src_dir_sts[i];
            if (src_st == null)
            {
                continue;
            }
            dst_st = dst_dir_sts?[i];
            if (dst_st == null)
            {
                dst_st = second_dir_sts?[i];
                if (dst_st == null)
                {
                    continue;
                }
            }
            ret[i] = src_st.AddTransition(dst_st);
            ret[i].hasExitTime = true;
            ret[i].canTransitionToSelf = false;
        }
        return ret;
    }

    static readonly Regex AnimTransitionParamMatch = new Regex(@"^(?<name>(\w+))_(?<dir>(\d{1}))$");
    static bool GetAnimTransitionParam(string animName, out int st, out int dir)
    {
        var mr = AnimTransitionParamMatch.Match(animName);
        if (mr.Success)
        {
            if (int.TryParse(mr.Groups["dir"].Value, out dir) && dir < DIR_NUM)
            {
                var an = mr.Groups["name"].Value;
                var idx = ArrayUtility.IndexOf(AnimsNames, an);
                if (idx >= 0)
                {
                    st = idx;
                    return true;
                }
            }
        }
        st = 0;
        dir = 0;
        return false;
    }


    static void BuildPrefab(string saveDirPath, string prefabName, Sprite coverSprite, AnimatorController ac)
    {
        var go = new GameObject(prefabName);
        var spriteRender = go.AddComponent<SpriteRenderer>();
        spriteRender.sprite = coverSprite;
        Animator animator = go.AddComponent<Animator>();
        animator.runtimeAnimatorController = ac;

        var savePath = Path.Combine(saveDirPath, prefabName + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(go, savePath);
        DestroyImmediate(go);
    }

    public static string DataPathToAssetPath(string path)
    {
        var idx = path.IndexOf("Assets/");
        if (idx >= 0)
        {
            return path.Substring(idx);
        }
        idx = path.IndexOf("Assets\\");
        if (idx >= 0)
        {
            return path.Substring(idx);
        }
        return null;
    }

    private static bool IsNeedSetLoopAnim(string animName)
    {
        for (var i = 0; i < LoopAnimsNamePrefix.Length; i++)
        {
            if (animName.IndexOf(LoopAnimsNamePrefix[i]) >= 0)
            {
                return true;
            }
        }
        return false;
    }

    #region class AnimationClipSettings

    class AnimationClipSettings
    {
        SerializedProperty m_Property;

        private SerializedProperty Get(string property) { return m_Property.FindPropertyRelative(property); }

        public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }

        public float startTime { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
        public float stopTime { get { return Get("m_StopTime").floatValue; } set { Get("m_StopTime").floatValue = value; } }
        public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
        public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
        public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }

        public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
        public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
        public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
        public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
        public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
        public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
        public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
        public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
        public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
        public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
    }

    #endregion

}