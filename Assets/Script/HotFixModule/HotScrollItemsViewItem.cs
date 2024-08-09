using UIKit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HotScrollItemsViewItem : ScrollItemsViewItem, IPointerClickHandler
{
    [SerializeField]
    private UnityEvent clickHandler = new UnityEvent();

    [SerializeField]
    private SelectableShow mSelectShow = null;

    private ScrollItemsView mView = null;

    protected override void Awake()
    {
        base.Awake();
        if (mSelectShow == null)
        {
            mSelectShow = this.GetComponent<SelectableShow>();
        }
        if (mView == null)
        {
            mView = this.transform.parent.GetComponent<ScrollItemsView>();
        }
    }

    protected override void ShowSelect(bool isSelected)
    {
        if (mSelectShow == null)
        {
            return;
        }
        mSelectShow.SetSelected(isSelected);
    }

    public void RefreshSelectShow()
    {
        //if (mView != null)
        //{
        //    ShowSelect(mView.SelectedIndex == ItemIndex);
        //}
    }

    public void TryInvokeViewMethod(string methodName)
    {
        var hfv = GetComponentInParent<HotFixView>();
        if (hfv != null)
        {
            hfv.InvokeHotMethod(methodName, gameObject);
        }
    }


    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (mView != null)
        {
            mView.SelectedIndex = this.ItemIndex;
        }
        clickHandler.Invoke();
    }
}