using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HealthUI_HealthCh : HealthCh_LogicActive
{

    float hpPercentage = 1;
    PostProcessVolume volume;
    Vignette vignette;
    void RefreshHealth()
    {
        Sliders.SrchSlider("Health").CurrValue(hpPercentage);
    }

   
    public override void Activate(params float[] floatParms)
    {
        if (floatParms[1] <= 0)
            MainHud.ReticulaPlay("Exit");

        base.Activate(floatParms);

        hpPercentage = floatParms[floatParms.Length - 1];

        vignette.intensity.Override(0.9f);

        RefreshHealth();
    }

    private void Start()
    {
        RefreshHealth();
        vignette = ScriptableObject.CreateInstance<Vignette>();
        vignette.enabled.Override(true);
        vignette.intensity.Override(0);
        vignette.color.Override(Color.red);
        vignette.smoothness.Override(0.3f);
        vignette.roundness.Override(1);
        vignette.rounded.Override(true);

        volume = PostProcessManager.instance.QuickVolume(12, 1, vignette);
    }


    private void Update()
    {
        vignette.intensity.Override(Mathf.Lerp(vignette.intensity.value, 0.6f-hpPercentage*0.6f, Time.deltaTime));
    }

    private void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(volume, true, true);
    }
}
