using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger_Proyectile : Proyectile
{
    public List<System.Type> powerSteal;

    public DaggerEffect daggerEffect;

    public AudioManager audioM;

    Interactuable_LogicActive interact;

    Collider[] colliders;

    /// <summary>
    /// Determina si los colliders de la deteccion como del objeto estan activos
    /// </summary>
    public bool pause
    {
        set
        {
            colliders[0].enabled = !value;
            colliders[1].enabled = !value;
            interact.diseable = value;
        }
    }

    /// <summary>
    /// setea el collider suyo y de forma negada el de deteccion
    /// obtiene el valor de su collider
    /// </summary>
    public bool active
    {
        get
        {
            return colliders[0].enabled;
        }

        set
        {
            colliders[0].enabled = value;
            colliders[1].enabled = !value;
            interact.diseable = value;
        }
    }

    protected override void OnDamage(IOnProyectileEnter aux)
    {
        base.OnDamage(aux);
        ImpactEnemySound();
    }

    protected override void FailDamage()
    {
        base.FailDamage();
        ImpactWallSound();
    }

    protected override void OnEnter(Collider other)
    {
        if (active)
        {
            gameObject.transform.parent = other.transform;
            MoveRb.kinematic = true;

            base.OnEnter(other); 

            ((MoveRotAndGlueRb)MoveRb).AddGlue(other.transform);

            active = false;

            CasterObject();
        }
    }

    public void StealPowers(Character ch)
    {
        if (ch.power.Count > 0)
            for (int i = 0; i < ch.power.Count; i++)
            {
                powerSteal.Add(ch.power[i].GetType());
            }
    }

    public void SetLine(Vector3 p1, Vector3 p2)
    {
        daggerEffect.SetLine(p1, p2);
    }

    public void ImpactEnemySound()
    {
        audioM.Play("ImpactEnemy");
    }
    public void ImpactWallSound()
    {
        audioM.Play("ImpactWall");
    }
    public void ImpactTpSound()
    {
        audioM.Play("ImpactTpObject");
    }

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        interact = GetComponentInChildren<Interactuable_LogicActive>();
        powerSteal = new List<System.Type>();
        daggerEffect = GetComponentInChildren<DaggerEffect>();
        audioM = GetComponent<AudioManager>();
    }
}


