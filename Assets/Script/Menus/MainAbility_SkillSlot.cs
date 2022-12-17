using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAbility_SkillSlot : SkillSlot
{
    public override void DeclinedDrop()
    {
        base.AcceptedDrop();
        draggableItem.ActiveAbility();
        SkillTreeManager.SwitchPowers(false);
    }
}
