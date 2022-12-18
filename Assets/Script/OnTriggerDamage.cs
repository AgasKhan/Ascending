using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDamage : MonoBehaviour
{
    Timer tim;

    public float damageAmount;
    Damage damage;

    // Start is called before the first frame update
    void Start()
    {
        tim = TimersManager.Create(0.25f);

        damage = new Damage();
        damage.amount = damageAmount;
        damage.debuffList = new List<System.Type>();
        damage.velocity = Vector3.zero;
    }


    void Damage(Collider other)
    {


        var arr = other.GetComponents<IOnProyectileEnter>();

        if(arr!=null)

        foreach (var item in arr)
        {
            item.ProyectileEnter(damage);
            print("recibio daño por estar debajo");
        }

       


    }

    private void OnTriggerEnter(Collider other)
    {
   
        Damage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if(tim.Chck)
        {
            Damage(other);
            tim.Reset();
        }
    }
}
