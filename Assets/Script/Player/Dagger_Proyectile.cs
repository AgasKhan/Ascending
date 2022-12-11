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

    
    Timer localAtrackt = new Timer(1);
    Timer angularAtrackt = new Timer(1);
    Timer finishTimer= new Timer(1);

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

    public void MoveLerpToParent(System.Func<Vector3> vec, float time, Transform parent)
    {
        //enabled = true;
        MoveRb.kinematic = true;
        MoveRb.eneableDrag = true;
        MoveRb.Stop();
        transform.parent = null;

        localAtrackt=Utilitys.LerpInTime(transform.position, vec, time, Vector3.Lerp, (saveData) => {transform.position = saveData; });

        angularAtrackt=Utilitys.LerpInTime(transform.rotation, Quaternion.identity, time, Quaternion.Slerp, (saveData) => {transform.rotation = saveData; });


        finishTimer=TimersManager.Create(time,
            () =>
            {
                transform.parent = parent;
                //enabled = false;
            });
    }

    public void CancelLerps()
    {
        TimersManager.Destroy(localAtrackt);
        TimersManager.Destroy(angularAtrackt);
        TimersManager.Destroy(finishTimer);

        localAtrackt.Set(0);
        angularAtrackt.Set(0);
        finishTimer.Set(0);

    }

    protected override void OnDamage(IOnProyectileEnter damaged)
    {
        base.OnDamage(damaged);

        if(OnDamaged(damaged, out Character ch))
        {
            AplicateDebuff(ch);

            StealPowers(ch);
        }

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
            //gameObject.transform.parent = other.transform;
            MoveRb.kinematic = true;
            MoveRb.useGravity = false;

            base.OnEnter(other); 

            ((MoveRotAndGlueRb)MoveRb).AddGlue(other.transform);

            active = false;

            CasterObject();

            //enabled = false;
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

    private void FixedUpdate()
    {
        if (finishTimer.Chck && transform.parent == null && MoveRb.kinematic)
        {
            active = true;
            MoveRb.kinematic = false;
            MoveRb.useGravity = true;
        }
            
    }
}


