using ILRuntime.CLR.Method;
using UnityEngine;

public class HotFixView : GameView
{
    [SerializeField]
    private string AttachHotFixClassFullName = null;

    [SerializeField]
    private string[] filterEventTypes = null;


    private HotFixScriptObject hotFixScriptObject = null;

    private IMethod openBeforeMethod;
    private bool openBeforeMethodGot;

    private IMethod onViewEventMethod;
    private bool onViewEventMethodGot;

    private void Awake()
    {
        if (hotFixScriptObject == null)
        {
            hotFixScriptObject = new HotFixScriptObject(AttachHotFixClassFullName, gameObject);
        }
    }

    private void Start()
    {
        hotFixScriptObject.InvokeScriptOnStart();
    }

    protected override void OnOpend()
    {
        hotFixScriptObject.InvokeScriptOnEnable();
    }

    protected override void OnClosed()
    {
        hotFixScriptObject.InvokeScriptOnDisable();
    }

    public void InvokeMethodWithoutParam(string hotMethodName)
    {
        hotFixScriptObject.InvokeMethodWithoutParam(hotMethodName);
    }

    public HotFixScriptObject GetHotFixScriptObject()
    {
        return hotFixScriptObject;
    }

    public void InvokeHotMethod(HotFixMethod m)
    {
        if (hotFixScriptObject == null)
        {
            hotFixScriptObject = new HotFixScriptObject(AttachHotFixClassFullName, gameObject);
        }
        if (m != null)
        {
            m._invokeBy(hotFixScriptObject);
        }
    }

    public void InvokeHotMethod(string hotMethodName, GameObject go)
    {
        if (hotFixScriptObject == null)
        {
            hotFixScriptObject = new HotFixScriptObject(AttachHotFixClassFullName, gameObject);
        }
        hotFixScriptObject.InvokeMethod(hotMethodName, go);
    }

    private object[] beforeOpenParam = null;
    protected override bool OnBeforeOpen(object param)
    {
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain == null)
        {
            return true;
        }
        if (hotFixScriptObject == null)
        {
            hotFixScriptObject = new HotFixScriptObject(AttachHotFixClassFullName, gameObject);
        }
        if (hotFixScriptObject.scriptAttachObject == null)
        {
            return true;
        }
        if (!openBeforeMethodGot)
        {
            openBeforeMethodGot = true;
            openBeforeMethod = hotFixScriptObject.scriptAttachObject.Type.GetMethod("OnBeforeOpen", 1);
        }
        if (openBeforeMethod != null)
        {
            if (beforeOpenParam == null)
            {
                beforeOpenParam = new object[1];
            }
            beforeOpenParam[0] = param;
            var ret = appdomain.Invoke(openBeforeMethod, hotFixScriptObject.scriptAttachObject, beforeOpenParam);
            return (bool)ret;
        }
        return true;
    }

    #region OnViewEvent

    private object[] onViewEventParam = null;
    public override bool OnViewEvent(string eventType, object param)
    {
        if (filterEventTypes != null && System.Array.IndexOf(filterEventTypes, eventType) >= 0)
        {
            var appdomain = HotFixImpl.Instance.appdomain;
            if (appdomain == null || hotFixScriptObject == null || hotFixScriptObject.scriptAttachObject == null)
            {
                return false;
            }
            if (!onViewEventMethodGot)
            {
                onViewEventMethodGot = true;
                onViewEventMethod = hotFixScriptObject.scriptAttachObject.Type.GetMethod("OnViewEvent", 2);
            }
            if (onViewEventMethod != null)
            {
                if (onViewEventParam == null)
                {
                    onViewEventParam = new object[2];
                }
                onViewEventParam[0] = eventType;
                onViewEventParam[1] = param;

                var ret = appdomain.Invoke(onViewEventMethod, hotFixScriptObject.scriptAttachObject, onViewEventParam);
                return (bool)ret;
            }
        }
        return false;
    }

    #endregion

    private void OnDestroy()
    {
        hotFixScriptObject.InvokeScriptOnDestroy();
        hotFixScriptObject = null;
    }
}