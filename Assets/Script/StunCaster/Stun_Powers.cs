using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Powers : Powers_FatherPwDbff
{
    [SerializeField]
    Vector2Int myShield;

    float originalDamage;
    public override void On(Character me)
    {
        //SchDebuff<Stun_Debuff>().Add(me, 5);
        myShield = SchPowerObject("Shield");

        me.AddDebuffToAplicate<Stun_Debuff>();

        AddCooldown("ShieldTimer", 10, me);

        AddObjRef("Shield", null, me);

        on_Update = MyUpdate;

        originalDamage = me.damage;
        me.damage *= 1.5f;
    }

    public override void Off(Character me)
    {
        base.Off(me);

        me.damage = originalDamage;

        me.RemoveDebuffToAplicate<Stun_Debuff>();
    }

    public override void Activate(Character me)
    {
        if (!me.MyObjReferences.ContainsKey("Shield") || (me.MyObjReferences["Shield"] != null && me.MyObjReferences["Shield"].activeSelf))
            return;

        me.MyObjReferences["Shield"] = PoolObjects.SpawnPoolObject(myShield, Vector3.zero, Quaternion.identity, me.transform);

        me.MyCooldowns["ShieldTimer"].Reset();

        me.animator.OnFloor();

    }

    private void MyUpdate(Character me)
    {
        if (me.MyCooldowns["ShieldTimer"].Chck || me.movement.dash)
        {
            me.OffObjRef("Shield");
        }
        else if (!me.MyCooldowns["ShieldTimer"].Chck && me.animator.CheckAnimations("Jump"))
        {
            me.animator.OnFloor();
        }
            
    }

}
