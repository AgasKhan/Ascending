using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour,IDropHandler
{
    AbilityButton draggableItem;
    public bool mainAbilitySlot = false;

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            draggableItem = dropped.GetComponent<AbilityButton>();
            draggableItem.parentAfterDrag = transform;

            if(mainAbilitySlot)
                draggableItem.RefreshAbilitiesList();
        }
    }
    
}
