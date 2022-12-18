using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAbility_SkillSlot : SkillSlot
{

    private void Awake()
    {
        deactive = false;
    }

    public override void DeclinedDrop()
    {
        base.AcceptedDrop();
        draggableItem.ActiveAbility();

        if(draggableItem.originalParent.parent.name == "PowersBranch")
            SkillTreeManager.SwitchPowers(false);
    }
}
