using ILRuntime.CLR.Method;
using UnityEngine;


public class HotFixMethod : MonoBehaviour
{
    [SerializeField]
    public string methodName = null;

    private HotFixScriptObject _so = null;
    private IMethod _m = null;

    private object[] selfParam = null;

    public void _invokeBy(HotFixScriptObject hso)
    {
        if (hso == null || hso.scriptAttachObject == null)
        {
            return;
        }
        var sobj = hso.scriptAttachObject;
        if (_so != hso)
        {
            _so = hso;
            _m = sobj.Type.GetMethod(methodName, 1);
        }
        if (selfParam == null)
        {
            selfParam = new object[1] { this };
        }
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain != null && _m != null)
        {
            appdomain.Invoke(_m, _so.scriptAttachObject, selfParam);
        }
    }
}
