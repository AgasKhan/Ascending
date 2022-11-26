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

    public void SetLine(Vector3 p1, Vector3 p2)
    {
        daggerEffect.SetLine(p1, p2);
    }

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        interact = GetComponentInChildren<Interactuable_LogicActive>();
        powerSteal = new List<System.Type>();
        daggerEffect = GetComponentInChildren<DaggerEffect>();
        audioM = GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(active && !other.CompareTag("Player") && !other.CompareTag("Shield"))
        {
            Character aux;
            gameObject.transform.parent = other.transform;
            MoveRb.kinematic = true;
            ImpactWallSound();

            if ((aux = other.gameObject.GetComponent<Character>()) != null)
            {
                if (aux.power.Count > 0)
                    for (int i = 0; i < aux.power.Count; i++)
                    {
                        powerSteal.Add(aux.power[i].GetType());
                    }

                AplicateDebuff(aux);
            }

            ((MoveRotAndGlueRb)MoveRb).AddGlue(other.transform);

            active = false;

            

            Damage(other);

            CasterObject();
        }
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
}