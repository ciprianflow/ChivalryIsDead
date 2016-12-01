using UnityEngine;
using UnityEngine.EventSystems;

public class InvisibleBGCtrl : MonoBehaviour, IPointerClickHandler
{
    MenuVisibilityCtrl _parentCtrl;
    GameObject otherObject;

    public void setParentCtrl(MenuVisibilityCtrl ctrl)
    {
        _parentCtrl = ctrl;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _parentCtrl.hide();
    }
}