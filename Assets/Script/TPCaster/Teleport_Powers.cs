using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Teleport_Powers : Powers_FatherPwDbff
{
    string layerDash;

    Collider[] colliders;

    public string LayerName;

    public override void Activate(Character me)
    {
        //me.AddPowerObjectSpawn(SchPowerObject("Teleport"));

        me.ActionOnDamage.Add(TP);
    }

    public override void Off(Character me)
    {
        base.Off(me);

        me.movement.layerDash = layerDash;
        MainHud.ReticulaPlay("Default");
    }

    public override void On(Character me)
    {    
        layerDash = me.movement.layerDash;
        if (LayerName == null || LayerName == "")
        {
            LayerName = "ObjectsAndEnemyNoCollision";
        }
        me.movement.layerDash = LayerName;

        me.AddCooldown("dashDamageCooldown",1);
    }

    void TP(Collider col)
    {
        if (col.gameObject.TryGetComponent(out MoveRb moveRb))
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
                Vector3 pos = col.transform.position+Vector3.up*0.5f;
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

    }

    void MyUpdatePlayer(Character me)
    {
        /*
        print(me.name);
        print(me.scoped.name);
        */
        

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
                    if (me.MyCooldowns["dashDamageCooldown"].Chck && health != null)
                    {
                        health.Substract(me.damage);
                        me.MyCooldowns["dashDamageCooldown"].Reset();
                    }
                }
            }
        }

        if (me.CompareTag("Player"))
            MyUpdatePlayer(me);
    }

    private void Start()
    {
        on_Update = MyUpdate;
    }
}