using UnityEngine;

public class HotFixBehaviour : MonoBehaviour
{
    [SerializeField]
    private string AttachHotFixClassFullName = null;

    private HotFixScriptObject hotFixScriptObject;

    private void Awake()
    {
        hotFixScriptObject = new HotFixScriptObject(AttachHotFixClassFullName, gameObject);
    }

    private void Start()
    {
        hotFixScriptObject.InvokeScriptOnStart();
    }

    private void OnEnable()
    {
        hotFixScriptObject.InvokeScriptOnEnable();
    }

    private void OnDisable()
    {
        hotFixScriptObject.InvokeScriptOnDisable();
    }

    public HotFixScriptObject GetHotFixScriptObject()
    {
        return hotFixScriptObject;
    }

    public void InvokeMethodWithoutParam(string hotMethodName)
    {
        hotFixScriptObject.InvokeMethodWithoutParam(hotMethodName);
    }

    public void InvokeHotMethod(HotFixMethod m)
    {
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

    private void OnDestroy()
    {
        hotFixScriptObject.InvokeScriptOnDestroy();
        hotFixScriptObject = null;
    }

}
