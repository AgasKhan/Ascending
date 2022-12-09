using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveRb))]
abstract public class Proyectile : MonoBehaviour
{
    public Character owner;

    public MoveRb MoveRb;

    public Damage damage;
    public void AplicateDebuff(Character ch)
    {
        if (damage.debuffList != null)
        {
            if (damage.debuffList.Count != 0)
            {
                foreach (var item in damage.debuffList)
                {
                    Debuff_FatherPwDbff.SchDebuff(item).Add(ch);
                }
            }
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

    protected virtual void OnEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IOnProyectileEnter aux))
        {
            OnDamage(aux);
        }
        else
            FailDamage();
    }
    protected virtual void OnExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IOnProyectileExit aux))
        {
            aux.ProyectileExit();
        }
    }
    protected virtual void OnDamage(IOnProyectileEnter aux)
    {
        aux.ProyectileEnter(damage);
    }
    protected virtual void FailDamage()
    {

    }

    private void Awake()
    {
        damage.proyectile = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tag) && !other.CompareTag(owner.tag))
        {
            OnEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(tag) && !other.CompareTag(owner.tag))
        {
            OnExit(other);
        }
    }
}

public interface IOnProyectileEnter
{
    void ProyectileEnter(Damage damage);
}

public interface IOnProyectileExit
{
    void ProyectileExit();
}

public struct Damage
{
    public float amount;
    public List<System.Type> debuffList;
    public Proyectile proyectile;
}