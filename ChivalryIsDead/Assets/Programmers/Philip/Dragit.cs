using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dragit : MonoBehaviour,
          IBeginDragHandler, IDragHandler, IEndDragHandler {

    public bool canDrag;
    private Vector3 iniPos;

    public void OnBeginDrag(PointerEventData eventData) {
        //iniPos{

        //}
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log ("OnDrag");
        if(canDrag)
            transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) { }
}