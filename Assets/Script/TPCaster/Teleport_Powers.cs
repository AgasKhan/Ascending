using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class Teleport_Powers : Powers_FatherPwDbff
{
    string layerDash;

    Collider[] colliders;

    public string LayerName;

    Timer tim;

    public override void OnExitState(Character me)
    {
        me.movement.layerDash = layerDash;
        MainHud.ReticulaPlay("Default");
    }

    public override void OnEnterState(Character me)
    {
        on_Update = MyUpdate;

        if (me.CompareTag("Player"))
        {
            on_Update += MyPlayer;
        }

        layerDash = me.movement.layerDash;

        if (LayerName == null || LayerName == "")
        {
            LayerName = "ObjectsAndCharacterNoCollision";
        }

        me.movement.layerDash = LayerName;

        tim = TimersManager.Create(1);
    }

    public override void ButtonEvent(Character me, float timePressed)
    {
        me.ActionOnDamage += TP;
    }
    void TP(Collider col)
    {
        if (!col.gameObject.TryGetComponent(out MoveRb moveRb))
        {
            return;
        }

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
            Vector3 pos = col.transform.position + Vector3.up * 0.5f;
            col.transform.position = player.transform.position;
            player.transform.position = pos;

            Utilitys.LerpInTime(lens.intensity.value, () => player.attackElements.lens.intensity.value, 0.5f, Mathf.Lerp, (saveData) => { lens.intensity.Override(saveData); });
            Utilitys.LerpInTime(lens.scale.value, 1, 0.5f, Mathf.Lerp, (saveData) => { lens.scale.Override(saveData); });

            moveRb.kinematic = true;

            TimersManager.Create(0.5f,
            () =>
            {
                RuntimeUtilities.DestroyVolume(volume, true, true);

                moveRb.kinematic = false;
            });

        });

    }

    void MyPlayer(Character me)
    {       
        if (me.scoped != null && me.scoped.gameObject.CompareTags("rb"))
        {
            MainHud.ReticulaPlay("Power");
        }
        else
        {
            MainHud.ReticulaPlay("Default");
        }
    }

    void MyUpdate(Character me)
    {
        if(me.movement.dash)
        {
            colliders = Physics.OverlapSphere(me.transform.position, 0.5f);

            foreach (Collider item in colliders)
            {
                if (me.name != item.name)
                {
                    Health health = item.GetComponent<Health>();
                    if (tim.Chck && health != null)
                    {
                        health.Substract(me.damage);
                        tim.Reset();
                    }
                }
            }
        }
    }


}