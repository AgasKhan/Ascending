using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Powers : Powers_FatherPwDbff
{
    string layerDash;

    Collider[] colliders;

    public string LayerName;

    public override void Activate(Character me)
    {
        me.AddPowerObjectSpawn(SchPowerObject("Teleport"));
    }

    public override void Off(Character me)
    {
        base.Off(me);

        me.movement.layerDash = layerDash;
    }

    public override void On(Character me)
    {
        on_Update = MyUpdate;


        layerDash = me.movement.layerDash;
        if (LayerName == null || LayerName == "")
        {
            LayerName = "ObjectsAndEnemyNoCollision";
        }
        me.movement.layerDash = LayerName;

        me.AddCooldown("dashDamageCooldown",1);
    }

    void MyUpdate(Character me)
    {
        //se puede optimizar

        if(me.movement.dash)
        {
            colliders = Physics.OverlapSphere(me.transform.position, 0.5f);

            foreach (Collider item in colliders)
            {
                if (me.name != item.name)
                {
                    Health health = item.GetComponent<Health>();
                    if (me.MyCooldowns["dashDamageCooldown"].Chck() && health != null)
                    {
                        health.Substract(me.damage);
                        me.MyCooldowns["dashDamageCooldown"].Reset();
                    }
                }
            }

        }

       
    }
}