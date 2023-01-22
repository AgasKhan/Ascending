using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCaster_Powers : Powers_FatherPwDbff
{

    public override void Activate()
    {
        me.RemovePower<FireCaster_Powers>();
    }

    public override void On()
    {

    }

    public override void Off()
    {

    }

    /*

    Vector2Int caster;

    float moreHP=-10;

    public override void Activate(Character me)
    {
        me.RemovePower<FireCaster_Powers>();

        me.AddPowerObjectSpawn(caster);


        SpawnPowerObject(caster, Vector3.zero);



    }

    public override void On(Character me)
    {
        Debuff_FatherPwDbff.SchDebuff<Fire_Debuff>().Add(me);
        
        caster = SchPowerObject("FireParticle");//obtengo el prefab

        
    }

    public override void Off(Character me)
    {
        Debuff_FatherPwDbff.SchDebuff<Fire_Debuff>().Remove(me);
        print(this.name + " power lost");
        me.health.Substract(moreHP);
        Debuff_FatherPwDbff.SchDebuff<Toxine_Debuff>().Add(me); 
    }

    */
}
