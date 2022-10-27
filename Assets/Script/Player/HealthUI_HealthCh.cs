using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI_HealthCh : HealthCh_LogicActive
{
    void RefreshHealth()
    {
        Sliders.SrchSlider("Health").CurrValue(GameManager.player.health.HpPercentage());
    }

    private void Start()
    {
        RefreshHealth();
    }

    public override void Activate(params float[] floatParms)
    {
        base.Activate(floatParms);
        RefreshHealth();
    }
}
