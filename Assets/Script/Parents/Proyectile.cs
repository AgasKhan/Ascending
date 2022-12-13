using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveRb))]
abstract public class Proyectile : MonoBehaviour
{
    public Character owner;

    public MoveRb MoveRb;

    public Damage damage;

    protected void AplicateDebuff(Character ch)
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
        var aux = other.GetComponents<IOnProyectileEnter>();

        if (aux!=null && aux.Length>0)
        {
            foreach (var item in aux)
            {
                OnDamage(item);
            }
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

    /// <summary>
    /// Funcion por defecto de danio
    /// </summary>
    /// <param name="damaged"></param>
    protected virtual void OnDamage(IOnProyectileEnter damaged)
    {
        damaged.ProyectileEnter(damage);
    }


    /// <summary>
    /// Funcion complementaria de danio que no sera llamada por defecto
    /// </summary>
    /// <typeparam name="T">Tipo de conversion que se buscara realizar</typeparam>
    /// <param name="damaged">Interfaz que representa la funcion que se desea ejecutar cuando colisiona con un proyectil</param>
    /// <returns>devolvera verdadero en caso de que se realice con exito la conversion</returns>
    protected virtual bool OnDamaged<T>(IOnProyectileEnter damaged, out T OUT)
    {
        if (damaged is T)
        {
            T aux = ((T)damaged);

            OUT = aux;

            return true;
        }

        OUT = default;

        return false;
    }

    protected virtual void FailDamage()
    {

    }

    public void Throw(Damage dmg, Vector3 dir, float multiply)
    {
        damage = dmg;
        MoveRb.Dash(dir, multiply);
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
    public Vector3 velocity;
}