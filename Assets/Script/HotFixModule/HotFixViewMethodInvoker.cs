using UnityEngine;

public class HotFixViewMethodInvoker : MonoBehaviour
{
    [SerializeField]
    private string methodName = null;

    private HotFixView mHotFixView = null;
    private bool hadInited = false;

    public void Invoke()
    {
        if (!hadInited)
        {
            mHotFixView = this.GetComponentInParent<HotFixView>();
        }
        if (mHotFixView == null)
        {
            return;
        }
        mHotFixView.InvokeHotMethod(methodName, gameObject);
    }
}