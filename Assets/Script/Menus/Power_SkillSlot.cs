using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_SkillSlot : SkillSlot
{
    public override void AcceptedDrop()
    {
        base.AcceptedDrop();
        SkillTreeManager.SwitchPowers(true);
    }
}
