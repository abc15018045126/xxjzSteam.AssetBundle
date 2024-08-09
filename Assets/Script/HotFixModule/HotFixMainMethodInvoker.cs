using ILRuntime.CLR.Method;
using UnityEngine;

public class HotFixMainMethodInvoker : MonoBehaviour
{
    [SerializeField]
    private string methodName = null;

    private IMethod hotFixMethod;
    private bool hotFixMethodGot;
    private object[] paramsCache;

    public void Invoke()
    {
        var appdomain = HotFixImpl.Instance.appdomain;
        if (appdomain == null)
        {
            return;
        }
        if (!hotFixMethodGot)
        {
            hotFixMethodGot = true;
            hotFixMethod = HotFixImpl.Instance.HotfixMainEnter.GetMethod(methodName, 1);
        }
        if (hotFixMethod == null)
        {
            return;
        }
        if (paramsCache == null)
        {
            paramsCache = new object[1] { gameObject };
        }
        appdomain.Invoke(hotFixMethod, null, paramsCache);
    }

    private void OnDestroy()
    {
        hotFixMethod = null;
    }
}
