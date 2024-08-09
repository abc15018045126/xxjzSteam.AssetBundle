using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class CQ3DAnimTools
{
    static AnimationClip TryFindMatchIdleClip(List<AnimationClip> cl)
    {
        AnimationClip secondResult = null;
        foreach (var c in cl)
        {
            var lowerName = c.name.ToLower();
            if (lowerName == "stand" || lowerName == "Idle")
            {
                return c;
            }
            if (secondResult != null)
            {
                continue;
            }
            if (lowerName.Contains("stand") || lowerName.Contains("idle"))
            {
                secondResult = c;
            }
        }
        return secondResult;
    }

    static AnimationClip TryFindMatchRelaxClip(List<AnimationClip> cl)
    {
        AnimationClip secondResult = null;
        foreach (var c in cl)
        {
            var lowerName = c.name.ToLower();
            if (lowerName == "relax" || lowerName == "idle2")
            {
                return c;
            }
            if (secondResult != null)
            {
                continue;
            }
            if (lowerName.Contains("relax") || lowerName.Contains("idle2"))
            {
                secondResult = c;
            }
        }
        return TryFindMatchIdleClip(cl);
    }

    static AnimationClip TryFindMatchRunClip(List<AnimationClip> cl)
    {
        AnimationClip secondResult = null;
        foreach (var c in cl)
        {
            var lowerName = c.name.ToLower();
            if (lowerName == "run" || lowerName == "move" || lowerName == "walk")
            {
                return c;
            }
            if (secondResult != null)
            {
                continue;
            }
            if (lowerName.Contains("run") || lowerName.Contains("move") || lowerName.Contains("walk"))
            {
                secondResult = c;
            }
        }
        return secondResult;
    }

    static AnimationClip TryFindMatchHurtClip(List<AnimationClip> cl)
    {
        AnimationClip secondResult = null;
        foreach (var c in cl)
        {
            var lowerName = c.name.ToLower();
            if (lowerName == "hurt" || lowerName == "injury" || lowerName == "hit01" || lowerName == "hit")
            {
                return c;
            }
            if (secondResult != null)
            {
                continue;
            }
            if (lowerName.Contains("hurt") || lowerName.Contains("injury") || lowerName.Contains("hit01") || lowerName.Contains("hit"))
            {
                secondResult = c;
            }
        }
        return secondResult;
    }

    static AnimationClip TryFindMatchDeadClip(List<AnimationClip> cl)
    {
        AnimationClip secondResult = null;
        foreach (var c in cl)
        {
            var lowerName = c.name.ToLower();
            if (lowerName == "die" || lowerName == "dead" || lowerName == "death")
            {
                return c;
            }
            if (secondResult != null)
            {
                continue;
            }
            if (lowerName.Contains("die") || lowerName.Contains("dead") || lowerName.Contains("death"))
            {
                secondResult = c;
            }
        }
        return secondResult;
    }


    static AnimationClip TryFindMatchAttackClip(List<AnimationClip> cl)
    {
        AnimationClip secondResult = null;
        foreach (var c in cl)
        {
            var lowerName = c.name.ToLower();
            if (lowerName == "attack" || lowerName == "attack01")
            {
                return c;
            }
            if (secondResult != null)
            {
                continue;
            }
            if (lowerName.Contains("attack"))
            {
                secondResult = c;
            }
        }
        return secondResult;
    }

    public static void CreateAnimatorController(string fbxPath)
    {
        var ass = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        var clipList = new List<AnimationClip>();
        foreach (var e in ass)
        {
            if (e is AnimationClip clip && !e.name.StartsWith("__preview__"))
            {
                clipList.Add(clip);
            }
        }

        var fbxName = Path.GetFileNameWithoutExtension(fbxPath);
        var fbxDir = Path.GetDirectoryName(fbxPath);

        var ac_filePath = Path.Combine(fbxDir, fbxName + "_ac.controller");
        var ac = AnimatorController.CreateAnimatorControllerAtPath(ac_filePath);
        ac.AddParameter("st0", AnimatorControllerParameterType.Int);

        var baseLayer = ac.layers[0];
        var asm = baseLayer.stateMachine;

        var stIdle = asm.AddState("idle", new Vector3(25, 220, 0));
        stIdle.motion = TryFindMatchIdleClip(clipList);

        var cp = new Vector3(300, 90, 0);
        var _h = new Vector3(0, 80, 0);
        var stRelax = asm.AddState("relax", cp);
        stRelax.motion = TryFindMatchRelaxClip(clipList);
        cp += _h;

        var stRun = asm.AddState("run", cp);
        stRun.motion = TryFindMatchRunClip(clipList);
        cp += _h;

        var stHurt = asm.AddState("hurt", cp);
        stHurt.motion = TryFindMatchHurtClip(clipList);
        cp += _h;

        var stDead = asm.AddState("dead", cp);
        stDead.motion = TryFindMatchDeadClip(clipList);
        cp += _h;

        var stAttack = asm.AddState("attack", new Vector3(560, 220, 0));
        stAttack.motion = TryFindMatchAttackClip(clipList);
        cp += _h;

        var idle2Relax = stIdle.AddTransition(stRelax);
        idle2Relax.AddCondition(AnimatorConditionMode.Equals, 1, "st0");
        var relax2Idle = stRelax.AddTransition(stIdle);
        relax2Idle.AddCondition(AnimatorConditionMode.NotEqual, 1, "st0");

        var idle2Run = stIdle.AddTransition(stRun);
        idle2Run.AddCondition(AnimatorConditionMode.Equals, 2, "st0");
        var run2Idle = stRun.AddTransition(stIdle);
        run2Idle.AddCondition(AnimatorConditionMode.NotEqual, 2, "st0");

        var idle2Hurt = stIdle.AddTransition(stHurt);
        idle2Hurt.AddCondition(AnimatorConditionMode.Equals, 3, "st0");
        var hurt2Idle = stHurt.AddTransition(stIdle);
        hurt2Idle.AddCondition(AnimatorConditionMode.NotEqual, 3, "st0");

        var idle2Dead = stIdle.AddTransition(stDead);
        idle2Dead.AddCondition(AnimatorConditionMode.Equals, 4, "st0");
        var dead2Idle = stDead.AddTransition(stIdle);
        dead2Idle.AddCondition(AnimatorConditionMode.NotEqual, 4, "st0");

        var attack2Idle = stAttack.AddTransition(stIdle);
        attack2Idle.hasExitTime = true;

        AssetDatabase.SaveAssets();
    }
}