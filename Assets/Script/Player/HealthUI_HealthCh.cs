using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HealthUI_HealthCh : HealthCh_LogicActive
{
    float hpPercentage;
    PostProcessVolume volume;
    Vignette vignette;

    static public HealthUI_HealthCh instance;

    public void RefreshHealth(float percentage)
    {
        hpPercentage = percentage;
        Sliders.SrchSlider("Health").CurrValue(hpPercentage);
    }

    public override void Activate(params float[] floatParms)
    {
        if (floatParms[1] <= 0)
            MainHud.ReticulaPlay("Exit");

        base.Activate(floatParms);

        vignette.intensity.Override(0.9f);

        RefreshHealth(floatParms[floatParms.Length - 1]);
    }

    private void Start()
    {
        RefreshHealth(1);
        vignette = ScriptableObject.CreateInstance<Vignette>();
        vignette.enabled.Override(true);
        vignette.intensity.Override(0);
        vignette.color.Override(Color.red);
        vignette.smoothness.Override(0.3f);
        vignette.roundness.Override(1);
        vignette.rounded.Override(true);

        volume = PostProcessManager.instance.QuickVolume(12, 1, vignette);

        instance = this;
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
