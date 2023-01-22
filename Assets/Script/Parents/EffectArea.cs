using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectArea : MyScripts
{

    [System.Serializable]
    public class Affected
    {
        public GameObject go;
        public Character affected;
        public Rigidbody absordedRB;
        public Health health;
        Timer tim;

        public void ChckandSubsHealth(float damage)
        {
            if (tim.Chck && !go.CompareTags("Death"))
            {
                health.Substract(damage);
                tim.Reset();
            }
        }

        ~Affected()
        {
            TimersManager.Destroy(tim);
        }

        public Affected(GameObject affected, float time=0)
        {
            go = affected;
            absordedRB = affected.GetComponent<Rigidbody>();

            tim = TimersManager.Create(time);

            if(affected.TryGetComponent(out this.affected))
            {
                health = this.affected.health;
            }
        }
    }

    public float coolDown;
    public float toDestroy;
    public float dmg;


    [SerializeField]
    protected List<Affected> affected = new List<Affected>();
    protected Timer toDeactivate;

    protected ParticleSystem particle;

    string strTimer = "areaEffect-";

    protected void ChckAddAffected(GameObject g)
    {

        if(g.CompareTags("Death"))
        {
            foreach (var item in affected)
            {
                if (item.go == g)
                {
                    affected.Remove(item);
                    return;
                }
            }
        }

        Debug.Log(g.name + " ha sido detectado");

        for (int i = 0; i < affected.Count; i++)
        {
            if (affected[i].go == g)
            {
                return;
            }
                
        }

        if (g.CompareTags(Tag.rb))
        {
            AddAffected(g);
            Debug.Log(g.name + " ha sido agregado en " + name);
        }
    }

    protected void AddAffected(GameObject g, float n=0)
    {
        affected.Add(new Affected(g, n));
    }


    private void OnEnable()
    {
        toDeactivate.Reset();
        particle.gameObject.SetActive(true);
        particle.Play();
    }

    private void OnDisable()
    {
        /*
        for (int i = 0; i < affected.Count; i++)
        {
            affected[i].affected.RemoveCooldown(strTimer);
        }*/

        affected.Clear();
        particle.Stop();

    }


    protected override void Config()
    {

        MyAwakes += MyAwake;
       
        MyUpdates += MyUpdate;

    }

     void MyAwake()
    {
        toDeactivate = TimersManager.Create(toDestroy);

        strTimer += GetType().FullName+"-"+GetInstanceID();

        particle = GetComponentInChildren<ParticleSystem>();

        particle.Stop();

        var part = particle.main;

        part.duration = (toDestroy- part.startLifetime.constant/ part.simulationSpeed) * part.simulationSpeed;

    }


    void MyUpdate()
    {
        if (toDeactivate.Chck)
        {
            gameObject.SetActive(false);
        }
    }
}
