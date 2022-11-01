using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI_HealthCh : HealthCh_LogicActive
{

    float hpPercentage = 1;

    void RefreshHealth()
    {
        Sliders.SrchSlider("Health").CurrValue(hpPercentage);
    }

    private void Start()
    {
        RefreshHealth();
    }

    public override void Activate(params float[] floatParms)
    {
        base.Activate(floatParms);

        hpPercentage = floatParms[floatParms.Length - 1];

        RefreshHealth();
    }
}
