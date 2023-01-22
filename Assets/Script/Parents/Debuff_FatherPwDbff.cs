using System.Collections.Generic;
using UnityEngine;

abstract public class Debuff_FatherPwDbff : FatherPwDbff
{
    /// <summary>
    /// Referencia para del indice del objeto que contenga las particulas (prefab)
    /// </summary>
    public GameObject particles;

    /// <summary>
    /// Referencia para del indice del objeto que contenga las particulas (prefab)
    /// </summary>
    public string particlesString;

    /// <summary>
    /// referencia del timer interno del debuff
    /// </summary>
    public Timer timer;

    //public Debuff_FatherPwDbff(float dbffTimer, Character me, GameObject particlesName=null)

    /// <summary>
    /// debe ser llamada luego de crear la clase
    /// </summary>
    /// <param name="me"></param>
    public virtual void Instance(Character me)
    {
        timer = TimersManager.Create(6);

        Create(me);

        if(particlesString!=null && particlesString!="")
        {
            particles = (GameObject)SpawnPowerObject(particlesString, Vector3.zero, Quaternion.identity, me.transform);
        }
            
    }


}

/*

    public static List<Debuff_FatherPwDbff> instances;

    [SerializeField]
    protected float dbffTimer;

    [SerializeField]
    protected string dbffTimerName;

    /// <summary>
    /// Prefab de las particulas, el cual sera buscado dentro del pool por su nombre
    /// </summary>
    public GameObject particlesName;

    /// <summary>
    /// Referencia para del indice del objeto que contenga las particulas (prefab)
    /// </summary>
    public Vector2Int particles;

    /// <summary>
    /// Funcion que es llamada cuando se aplica el debuffo
    /// </summary>
    /// <param name="a">Parametro que recibe de forma automatica que te da al afectado</param>
    abstract protected void Aplicate(Character a);

    /// <summary>
    /// Funcion que es llamada cuando se deja de aplicar el debuffo
    /// Tirara error si no se le pasa por parametro el Affected
    /// </summary>
    protected PrototypeFunction DeAplicate;


    /// <summary>
    /// Agregar un efecto de estado al personaje
    /// </summary>
    /// <param name="t">Tiempo por defecto para asignarle al temporizar asosiado</param>
    /// <param name="n">nombre del timer, para el debuffo</param>
    /// <param name="p">Personaje al que se le desea agregar el debuffo</param>
    public void Add(Character p)
    {
        bool check = false;

        int i = 0;

        foreach (Character Pjs in chrAffected)
        {
            if (Pjs == p)
            {
                check = true;
                break;
            }
            i++;
        }

        if(!check)
        {
            GameObject instanceParticle = SpawnPowerObject(particles, Vector3.zero, Quaternion.identity, p.transform);

            AddCooldown(dbffTimerName, dbffTimer ,p);

            AddObjRef(particlesName.name, instanceParticle, p);

            chrAffected.Add(p);

            Aplicate(p);
        }
        else
        {
            chrAffected[i].MyCooldowns[dbffTimerName].Reset();
        }

        var part = chrAffected[i].MyObjReferences[particlesName.name].GetComponentInChildren<ParticleSystem>();

        part.Stop();

        TimersManager.Create(1, 
        ()=> 
        {
            var partMain = part.main;
            partMain.duration = (dbffTimer - partMain.startLifetime.constant / partMain.simulationSpeed) * partMain.simulationSpeed;
            part.Play();
        });
    }


    /// <summary>
    /// Remueve un efecto de estado al personaje
    /// </summary>
    /// <param name="objective">Personaje objetivo</param>
    /// <param name="n">Nombre del timer del debuffo</param>
    public void Remove(Character objective)
    {
        if(objective!=null)
            for (int i = chrAffected.Count-1; i >=0 ; i--)
            {
                if (objective.GetInstanceID() == chrAffected[i].GetInstanceID() )
                {
                    ClearRefs(objective);

                    if (DeAplicate!=null)
                        DeAplicate(chrAffected[i]);

                    chrAffected.RemoveAt(i);           
                    return;
                }
            }
    }

    /// <summary>
    /// Busca un debuff 
    /// </summary>
    /// <typeparam name="T">El tipo de debuff</typeparam>
    /// <returns></returns>
    static public Debuff_FatherPwDbff SchDebuff<T> () where T : Debuff_FatherPwDbff
    {
        for (int i = 0; i < instances.Count; i++)
        {
            if (instances[i] is T)
                return instances[i];
        }

        Debug.LogWarning("Debuffo no encontrado");
        return null;
    }

    /// <summary>
    /// Busca un debuff 
    /// </summary>
    /// <param name="type">El tipo de debuff</param>
    /// <returns></returns>
    static public Debuff_FatherPwDbff SchDebuff (System.Type type)
    {
        for (int i = 0; i < instances.Count; i++)
        {
            if (instances[i].GetType()== type)
                return instances[i];
        }

        Debug.LogWarning("Debuffo no encontrado");
        return null;
    }

    private void Awake()
    {
        if(instances==null)
            instances = new List<Debuff_FatherPwDbff>();

        instances.Add(this);
    }

    private void OnDestroy()
    {
        instances.Clear();
    }

    private void Start()
    {
        if (particlesName != null)
            particles = SchPowerObject(particlesName.name);
        else
            particles.x = -1;
    }
*/

