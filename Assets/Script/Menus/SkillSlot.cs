using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour,IDropHandler
{
    protected AbilitiesParent draggableItem;
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            draggableItem = dropped.GetComponent<AbilitiesParent>();
            
            if (transform != draggableItem.originalParent)
                DeclinedDrop();
            else
                AcceptedDrop();

        }
    }

    public virtual void AcceptedDrop()
    {
        draggableItem.parentAfterDrag = transform;
    }

    public virtual void DeclinedDrop()
    {
        draggableItem.parentAfterDrag = draggableItem.originalParent;
    }

}
