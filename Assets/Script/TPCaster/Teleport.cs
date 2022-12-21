using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Teleport : PowerSpawner
{
    protected override void Detect(MoveRb item)
    {
        Player_Character player = GameManager.player;

        LensDistortion lens = ScriptableObject.CreateInstance<LensDistortion>();
        lens.enabled.Override(true);
        lens.intensity.Override(0);
        lens.centerX.Override(0);
        lens.centerY.Override(0);

        PostProcessVolume volume = PostProcessManager.instance.QuickVolume(12, 100f, lens);

        Utilitys.LerpInTime(lens.intensity.value, 75, 0.5f, Mathf.Lerp, (saveData) => { lens.intensity.Override(saveData); });
        Utilitys.LerpInTime(lens.scale.value, 0.01f, 0.5f, Mathf.Lerp, (saveData) => { lens.scale.Override(saveData); });

        TimersManager.Create(0.5f,
        () =>
        {
            item.transform.position = player.transform.position;
            player.transform.position = transform.position;

            Utilitys.LerpInTime(lens.intensity.value, () => player.attackElements.lens.intensity.value, 0.5f, Mathf.Lerp, (saveData) => { lens.intensity.Override(saveData); });
            Utilitys.LerpInTime(lens.scale.value, 1, 0.5f, Mathf.Lerp, (saveData) => { lens.scale.Override(saveData); });

            TimersManager.Create(0.5f,
            () =>
            {
                RuntimeUtilities.DestroyVolume(volume, true, true);
            });

        });

    }


    


    
}
