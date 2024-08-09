using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Intepreter;
using System;
using UnityEngine;

public class HotFixScriptObject
{
    public readonly ILTypeInstance scriptAttachObject;
    private readonly IMethod scriptOnStart;
    private readonly IMethod scriptOnEnable;
    private readonly IMethod scriptOnDisable;
    private readonly IMethod scriptOnDestroy;

    private object[] selfInvokeMethodParam;

    public HotFixScriptObject(string attachHotFixClassFullName, GameObject go)
    {
        try
        {
            if (scriptAttachObject == null)
            {
                scriptAttachObject = HotFixImpl.Instance.CreateHotFixScriptObject(attachHotFixClassFullName, go);
            }
            if (scriptAttachObject == null)
            {
                return;
            }
            this.scriptOnStart = scriptAttachObject.Type.GetMethod("OnStart", 0);
            this.scriptOnEnable = scriptAttachObject.Type.GetMethod("OnEnable", 0);
            this.scriptOnDisable = scriptAttachObject.Type.GetMethod("OnDisable", 0);
            this.scriptOnDestroy = scriptAttachObject.Type.GetMethod("OnDestroy", 0);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void InvokeScriptOnStart()
    {
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain != null && scriptAttachObject != null && scriptOnStart != null)
        {
            appdomain.Invoke(scriptOnStart, scriptAttachObject, null);
        }
    }

    public void InvokeScriptOnEnable()
    {
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain != null && scriptAttachObject != null && scriptOnEnable != null)
        {
            appdomain.Invoke(scriptOnEnable, scriptAttachObject, null);
        }
    }

    public void InvokeScriptOnDisable()
    {
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain != null && scriptAttachObject != null && scriptOnDisable != null)
        {
            appdomain.Invoke(scriptOnDisable, scriptAttachObject, null);
        }
    }

    public void InvokeScriptOnDestroy()
    {
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain != null && scriptAttachObject != null && scriptOnDestroy != null)
        {
            appdomain.Invoke(scriptOnDestroy, scriptAttachObject, null);
        }
    }

    public void InvokeMethodWithoutParam(string hotMethodName)
    {
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain == null || scriptAttachObject == null && string.IsNullOrEmpty(hotMethodName))
        {
            return;
        }
        var m = scriptAttachObject.Type.GetMethod(hotMethodName, 0);
        if (m != null)
        {
            appdomain.Invoke(m, scriptAttachObject, null);
        }
    }

    public void InvokeMethod(string hotMethodName, GameObject go)
    {
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain == null || scriptAttachObject == null && string.IsNullOrEmpty(hotMethodName))
        {
            return;
        }
        var m = scriptAttachObject.Type.GetMethod(hotMethodName, 1);
        if (m == null)
        {
            Debug.LogErrorFormat("InvokeMethod failed!, can't find method[{0}] in class[{1}]", hotMethodName, scriptAttachObject.Type);
            return;
        }
        if (selfInvokeMethodParam == null)
        {
            selfInvokeMethodParam = new object[1];
        }
        selfInvokeMethodParam[0] = go;
        appdomain.Invoke(m, scriptAttachObject, selfInvokeMethodParam);
    }
}