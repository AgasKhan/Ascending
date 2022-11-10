using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase padre de los personajes
/// </summary>
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthCh_LogicActive))]
abstract public class Character : MyScripts
{
    /// <summary>
    /// referencia a la vida del character
    /// </summary>
    public Health health;

    /// <summary>
    /// referencia al movimiento del character
    /// </summary>
    public Movement movement;

    /// <summary>
    /// referencia del audiomanager
    /// </summary>
    public AudioManager audioM;

    /// <summary>
    /// collider de mi objeto
    /// </summary>
    public CapsuleCollider coll;

    /// <summary>
    /// Objeto al que se esta apuntando
    /// </summary>
    public Collider scoped;

    /// <summary>
    /// caracteristicas del objeto al que se apunta en caso de ser interactuable
    /// </summary>
    public Interactuable_LogicActive interactuable;

    /// <summary>
    /// Coordenda a la que colisiona el raycast de la vision
    /// </summary>
    public Vector3 scopedPoint;

    /// <summary>
    /// referencia del animControler
    /// </summary>
    public AnimatorController animator;

    /// <summary>
    /// Vector que representa las teclas de direccion presionadas
    /// </summary>
    public Vector3 input;

    /// <summary>
    /// Referencia de todos los timers asociados al character
    /// </summary>
    [SerializeField]
    public Pictionarys<string, Timer> MyCooldowns = new Pictionarys<string, Timer>();

    /// <summary>
    /// Diccionario que contiene todas las referencias de los objetos spawneados del personaje
    /// </summary>
    [SerializeField]
    public Pictionarys<string, GameObject> MyObjReferences = new Pictionarys<string, GameObject>();


    /// <summary>
    /// Objeto que se casteara al golpear (con la daga o con en el ataque del enemigo)
    /// se guarda el indice del pool
    /// </summary>
    public List<Vector2Int> powerObjectSpawn = new List<Vector2Int>();

    /// <summary>
    /// Debuff que se aplicara al atacar
    /// </summary>
    [SerializeField]
    public List<System.Type> debuffToAplicate = new List<System.Type>();

    /// <summary>
    /// Lista de poderes que posee el character
    /// </summary>
    [SerializeField]
    public List<Powers_FatherPwDbff> power = new List<Powers_FatherPwDbff>();

    /// <summary>
    /// indice del poder actual
    /// </summary>
    public int actualPower;

    /// <summary>
    /// Danio que realizara el personaje
    /// </summary>
    public float damage;

    /// <summary>
    /// el impulso que gana para saltar
    /// </summary>
    public float jumpStrength;

    /// <summary>
    /// Cantidad de saltos que puede hacer en el aire
    /// </summary>
    public int extraJumps;

    /// <summary>
    /// guarda la anterior posicion en y
    /// </summary>
    protected float previousTransformY;

    /// <summary>
    /// se usa de forma auxiliar para guardar la velocidad maxima original
    /// </summary>
    protected float maxSpeed;

    /// <summary>
    /// El tiempo que espera antes de anular el salto
    /// </summary>
    [SerializeField]
    protected Timer coyoteTime = Timers.Create(0.3f);

    /// <summary>
    /// Variable que cuenta los saltos en el aire
    /// </summary>
    int _extraJumps;

    /// <summary>
    /// Apaga los graficos del character en un determinado tiempo
    /// </summary>
    virtual public void OffMesh()
    {
        CoroutineMesh();
    }
   
    /// <summary>
   /// setea todo lo necesario para la animacion
   /// </summary>
   /// <param name="v">vector de velocidad</param>
   protected void RefreshAnims(Vector3 v)
   {
       animator.input = input;
       animator.velocity = v;
       animator.relation = (new Vector3(v.x,0,v.z).sqrMagnitude) / (maxSpeed * maxSpeed);

       if (maxSpeed > movement.maxSpeed)
           animator.relation *= 2;


       animator.aceleration = movement.desAceleration;
   }

   /// <summary>
   /// setea todo lo necesario para la animacion
   /// </summary>
   protected void RefreshAnims()
   {
       RefreshAnims(movement.velocity3D);
   }
     /*

    /// <summary>
    /// setea todo lo necesario para la animacion
    /// </summary>
    protected void RefreshAnims()
    {
        animator.input = input;
        animator.velocity = movement.velocity3D;
        animator.relation = movement.velocity2D.sqrMagnitude/(maxSpeed*maxSpeed);
       
        if (maxSpeed > movement.maxSpeed)
            animator.relation *= 2;
       

        animator.aceleration = movement.desAceleration;
    }*/

    #region mobility

    /// <summary>
    /// Funcion que ejecuta un salto
    /// </summary>
    public void Jump()
    {
        if (!coyoteTime.Chck())
        {
            _extraJumps = extraJumps;
            coyoteTime.Substract(10);

        }
        if (_extraJumps >= 0)
        {
            movement.CancelGravity();
            movement.MoveLocal(transform.up, jumpStrength + Physics.gravity.y * -1, false);
            _extraJumps--;
        }
        JumpEnd();
    }

    /// <summary>
    /// Funcion que ejecuta un dash
    /// </summary>
    public void Roll()
    {
        movement.Move(animator.transform.forward,100);
    }

    /// <summary>
    /// Funcion que ejecuta un dash
    /// </summary>
    public void Dash()
    {
        if(animator.Dash())
        movement.Dash(animator.transform.forward);
        //coll.height = 1;
    }

    /// <summary>
    /// Funcion que se ejecuta al finalizar un dash
    /// </summary>
    public void DashEnd()
    {
        //coll.height = 2;
        animator.Dash(false);
        JumpEnd();
    }


    void JumpEnd()
    {
        animator.ResetJump();
    }

    #endregion

    #region attack functions

    /// <summary>
    /// aniade una referencia de un objeto para spawnearlo cuando colisione el proyectil
    /// </summary>
    /// <param name="go">referencia del objeto a spawnear</param>
    public void AddPowerObjectSpawn(Vector2Int go)
    {
        if (!powerObjectSpawn.Contains(go))
            powerObjectSpawn.Add(go);

    }

    /// <summary>
    /// aniade un debuffo que se ejecutara en el siguiente ataque
    /// </summary>
    /// <param name="ttd">tipo del debufo que se agregara</param>
    public void AddDebuffToAplicate(System.Type ttd)
    {
        if (debuffToAplicate.Contains(ttd))
        {
            return;
        }

        debuffToAplicate.Add(ttd);
    }

    /// <summary>
    /// aniade un debuffo que se ejecutara en el siguiente ataque
    /// </summary>
    /// <typeparam name="T">tipo del debufo que se agregara</typeparam>
    public void AddDebuffToAplicate<T>() where T : Debuff_FatherPwDbff
    {
        AddDebuffToAplicate(Debuff_FatherPwDbff.SchDebuff<T>().GetType());
    }

    /// <summary>
    /// remueve un debuffo que se ejecutaria en el siguiente ataque
    /// </summary>
    /// <param name="ttd">tipo del debufo que se removera</param>
    public void RemoveDebuffToAplicate(System.Type ttd)
    {
        if (debuffToAplicate.Contains(ttd))
        {
            debuffToAplicate.Remove(ttd);
        }
    }

    /// <summary>
    /// remueve un debuffo que se ejecutaria en el siguiente ataque
    /// </summary>
    /// <typeparam name="T">tipo del debufo que se removera</typeparam>
    public void RemoveDebuffToAplicate<T>() where T : Debuff_FatherPwDbff
    {
        RemoveDebuffToAplicate(Debuff_FatherPwDbff.SchDebuff<T>().GetType());
    }



    #endregion

    #region objects references

    bool ChckObjRef(string n)
    {
        if (MyObjReferences.count == 0 || !MyObjReferences.ContainsKey(n))
        {
            return false;
        }

        return true;
    }

    public void AddObjRef(string n, GameObject g)
    {
        if (MyObjReferences.ContainsKey(n))
            return;

        MyObjReferences.Add(n, g);
    }

    public void OnObjRef(string n)
    {
        if (!ChckObjRef(n))
        {
            Debug.LogWarning("Se intento prender el gameObject " + n + " en el character " + name + " y no existe");
            return;
        }
            

        if (MyObjReferences[n] != null)
            MyObjReferences[n].SetActive(true);
    }

    public void OffObjRef(string n)
    {
        if (!ChckObjRef(n))
        {
            Debug.LogWarning("Se intento apagar el gameObject " + n + " en el character " + name + " y no existe");
            return;
        }

        if (MyObjReferences[n] != null)
            MyObjReferences[n].SetActive(false);
    }

    public void RemoveObjRef(string n)
    {
        OffObjRef(n);

        MyObjReferences.Remove(n);
    }

    #endregion

    #region timers references

    public void AddCooldown(string n, float t)
    {
        if (MyCooldowns.ContainsKey(n))
            return;

        MyCooldowns.Add(n, Timers.Create(t));
    }

    public void RemoveCooldown(string n)
    {
        if (!MyCooldowns.ContainsKey(n))
        {
            Debug.LogError("Se intento destruir el timer " + n + " en el character " + name + " y no existe");

            return;
        }

        Timers.Destroy(MyCooldowns[n]);

        MyCooldowns.Remove(n);

    }

    #endregion

    #region Powers

    /// <summary>
    /// Activa el poder con el indice actual
    /// </summary>
    public void ActivePower()
    {
        if (power.Count > 0)
            power[actualPower].Activate(this);
    }

    /// <summary>
    /// Suma uno al indice del poder de ser posible
    /// </summary>
    public void NextPower()
    {
        if (power.Count - 1 > actualPower)
            actualPower++;
    }

    /// <summary>
    /// resta uno al indice del poder de ser posible
    /// </summary>
    public void PreviosPower()
    {
        if (actualPower > 0 && power.Count > 0)
            actualPower--;
    }


    /// <summary>
    /// version privada, contiene todo el algoritmo que se ejecuta cuando se agrega un poder
    /// </summary>
    /// <param name="powerStatic"></param>
    void AddPower(Powers_FatherPwDbff powerStatic, int i =0)
    {
        powerStatic.chrAffected.Add(this);
        powerStatic.On(this);
        
        power.Insert(i, powerStatic);

        SpritesManager.RefreshUI();
    }

    /// <summary>
    /// Agregua un poder al character
    /// </summary>
    /// <param name="type">Tipo del poder a agregar</param>
    public void AddPower(System.Type type, int i = 0)
    {
        if (type != null)
        {
            foreach (Powers_FatherPwDbff powerStatic in Powers_FatherPwDbff.powers)
            {
                if (powerStatic.GetType() == type)
                {
                    AddPower(powerStatic, i);
                }
            }
        }
    }

    /// <summary>
    /// Agregua un poder al character
    /// </summary>
    /// <typeparam name="T">Nombre del sript que es un poder</typeparam>
    public void AddPower<T>(int i = 0) where T : Powers_FatherPwDbff
    {
        foreach (Powers_FatherPwDbff powerStatic in Powers_FatherPwDbff.powers)
        {
            if (powerStatic is T)
            {
                AddPower(powerStatic,i);
            }
        }
    }

    /// <summary>
    /// Remplaza el primer poder
    /// </summary>
    /// <typeparam name="T">El poder que se va a agregar</typeparam>
    public void ReplaceFirstPower(System.Type type)
    {
        RemovePower(0);
        AddPower(type,0);
    }

 
    /// <summary>
    /// Remplaza el poder actual
    /// </summary>
    /// <typeparam name="T">El poder que se va a agregar</typeparam>
    public void ReplaceFirstPower<T>() where T : Powers_FatherPwDbff
    {
        RemovePower(0);
        AddPower<T>(0);
    }

    /// <summary>
    /// Remueve un poder en base al indice
    /// </summary>
    /// <param name="i"></param>
    public void RemovePower(int i)
    {
        if (i >= 0 && i < power.Count && power.Count > 0)
        {
            power[i].Off(this);
            power[i].chrAffected.Remove(this);
            power.RemoveAt(i);
        }

        if (actualPower >= power.Count)
        {
            actualPower = power.Count - 1;
        }

        if (actualPower < 0)
        {
            actualPower++;
        }
    }

    /// <summary>
    /// remueve el poder si el tipo coincide con el indice
    /// </summary>
    /// <typeparam name="T">Tipo de poder</typeparam>
    /// <param name="i">indice del poder</param>
    public bool RemovePower<T>(int i) where T : Powers_FatherPwDbff
    {
        if (power[i] is T)
        {
            RemovePower(i);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Remueve un poder al character (el primero que encuentra)
    /// </summary>
    /// <param name="p">Tipo de poder</param>
    public void RemovePower<T>() where T : Powers_FatherPwDbff
    {
        for (int i = power.Count - 1; i >= 0; i--)
        {
            if (RemovePower<T>(i))
                return;
        }
    }
    #endregion
    /*
    #region Sonido
    public abstract void AttackSound();

    public abstract void AuxiliarSound();

    public abstract void DeathSound();

    public abstract void WoRSoundLeft();

    public abstract void WoRSoundRight();
    #endregion
    */

    protected override void Config()
    {
        MyAwakes += MyAwake;
        MyStarts += MyStart;
        MyUpdates += MyUpdate;
    }

    void MyAwake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<AnimatorController>();
        audioM = GetComponent<AudioManager>();   
    }

    void MyStart()
    {
        gameObject.AddTags(Tag.character);

        /*
        animator.functions.AddRange(new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               { "WoRLeft",WoRSoundLeft},
               { "WoRRight",WoRSoundRight },
               {"AttackSound",AttackSound},
               {"AuxiliarSound", AuxiliarSound },
               { "DeathSound", DeathSound}
           });
        */
    }

    void MyUpdate()
    {
        RefreshAnims();
    }

    IEnumerator CoroutineMesh()
    {
        yield return new WaitForSeconds(5);

        gameObject.SetActive(false);
    }


}