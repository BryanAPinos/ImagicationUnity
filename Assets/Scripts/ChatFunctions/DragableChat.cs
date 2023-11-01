using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableChat : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    // offset: distance from mouse position to center of object
    private Vector3 offset;

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        // calculate offset from mouse position to center of object when dragging starts
        offset = transform.position - (Vector3)eventData.position;
    }
    #endregion

    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        // move object to mouse position + offset when dragging
        transform.position = (Vector3)eventData.position + offset;
    }
    #endregion
}
