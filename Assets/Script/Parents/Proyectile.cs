using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveRb))]
abstract public class Proyectile : MonoBehaviour
{
    public Character owner;

    public MoveRb MoveRb;

    protected void Damage(Collider other)
    {
        Health health;

        if ((health = other.GetComponent<Health>()) != null)
        {
            health.Substract(owner.damage);
        }
    }

    protected void CasterObject()
    {
        if (owner != null)
        {
            if (owner.powerObjectSpawn.Count != 0)
            {
                foreach (var item in owner.powerObjectSpawn)
                {
                    PoolObjects.SpawnPoolObject(item, transform.position, Quaternion.identity);
                }

                owner.powerObjectSpawn.Clear();
            }
        }
    }

    protected void AplicateDebuff(Character ch)
    {
        if (owner != null)
        {
            if (owner.debuffToAplicate.Count != 0)
            {
                foreach (var item in owner.debuffToAplicate)
                {
                    Debuff_FatherPwDbff.SchDebuff(item).Add(ch);
                }

            }

        }
    }

}
