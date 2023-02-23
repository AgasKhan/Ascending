using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase padre de los personajes
/// </summary>
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthCh_LogicActive))]
abstract public class Character : MyScripts, IOnProyectileEnter
{
    [Header("Common propertys")]
    [Space]

    /// <summary>
    /// referencia a la vida del character
    /// </summary>
    public Health health;

    /// <summary>
    /// referencia al movimiento del character
    /// </summary>
    public Movement movement;

    [Header("aaarte")]

    /// <summary>
    /// referencia del audiomanager
    /// </summary>
    public AudioManager audioM;

    /// <summary>
    /// referencia del animControler
    /// </summary>
    public AnimatorController animator;

    [Header("apuntado")]

    /// <summary>
    /// Objeto al que se esta apuntando
    /// </summary>
    public Collider scoped;

    /// <summary>
    /// Coordenda a la que colisiona el raycast de la vision
    /// </summary>
    public Vector3 scopedPoint;

    /// <summary>
    /// caracteristicas del objeto al que se apunta en caso de ser interactuable
    /// </summary>
    public Interactuable_LogicActive interactuable;

    [Header("Referencias internas")]

    /// <summary>
    /// Objeto que se casteara al golpear (con la daga o con en el ataque del enemigo)
    /// se guarda el indice del pool
    /// </summary>
    public List<Vector2Int> ObjectSpawnOnDamage = new List<Vector2Int>();

    /// <summary>
    /// Accion que se realizara al realizar danio
    /// </summary>
    public System.Action<Collider> ActionOnDamage;


    [Header("Poderes")]

    /// <summary>
    /// Danio que realizara el personaje
    /// </summary>
    public float damage;

    /// <summary>
    /// Lista de poderes que posee el character
    /// </summary>
    [SerializeReference]
    public List<Powers_FatherPwDbff> power = new List<Powers_FatherPwDbff>();

    /// <summary>
    /// ultimo poder
    /// </summary>
    public Powers_FatherPwDbff lastPower;

    /// <summary>
    /// diccionario de updates
    /// </summary>
    public Pictionarys<System.Type, System.Action<Character>> handlerUpdates = new Pictionarys<System.Type, System.Action<Character>>();

    /// <summary>
    /// indice del poder actual
    /// </summary>
    public int actualPower;

    /// <summary>
    /// Debuff que se aplicara al atacar
    /// </summary>
    [SerializeReference]
    public List<System.Type> debuffToAplicate = new List<System.Type>();

    /// <summary>
    /// debufs que posee el character
    /// </summary>
    [SerializeReference]
    public List<Debuff_FatherPwDbff> debuffList = new List<Debuff_FatherPwDbff>();

    [Header("Movimiento")]

    /// <summary>
    /// Vector que representa las teclas de direccion presionadas
    /// </summary>
    public Vector3 input;

    /// <summary>
    /// el impulso que gana para saltar
    /// </summary>
    public float jumpStrength;

    /// <summary>
    /// Cantidad de saltos que puede hacer en el aire
    /// </summary>
    public int extraJumps;

    /// <summary>
    /// se usa de forma auxiliar para guardar la velocidad maxima original
    /// </summary>
    public float maxSpeed;

    /// <summary>
    /// guarda la anterior posicion en y
    /// </summary>
    protected float previousTransformY;

    /// <summary>
    /// El tiempo que espera antes de anular el salto
    /// </summary>
    [SerializeField]
    public Timer coyoteTime;

    /// <summary>
    /// Variable que cuenta los saltos en el aire
    /// </summary>
    protected int _extraJumps;

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


    #region mobility

    

    /// <summary>
    /// Funcion que ejecuta un salto
    /// </summary>
    public virtual void Jump()
    {
        movement.CancelGravity();
        movement.MoveLocal(transform.up, jumpStrength + Physics.gravity.y * -1, false);
    }

    public void JumpEnd()
    {
        animator.ResetJump();
    }

    /// <summary>
    /// Funcion que ejecuta un dash
    /// </summary>
    public virtual void Roll()
    {
        movement.Move(animator.transform.forward,100);
    }

    /// <summary>
    /// Funcion que ejecuta un dash
    /// </summary>
    public virtual void Dash()
    {
        if(animator.Dash())
        {
            movement.Dash(animator.transform.forward);
        }
           
        //movement.eneableDetectFloor = false;
        //coll.height = 1;
    }

    /// <summary>
    /// Funcion que se ejecuta al finalizar un dash
    /// </summary>
    public virtual void DashEnd()
    {
        //movement.eneableDetectFloor = true;
        animator.Dash(false);

        JumpEnd();
    }


    #endregion

    #region attack functions

    /// <summary>
    /// aniade una referencia de un objeto para spawnearlo cuando colisione el proyectil
    /// </summary>
    /// <param name="go">referencia del objeto a spawnear</param>
    public void AddPowerObjectSpawn(Vector2Int go)
    {
        if (!ObjectSpawnOnDamage.Contains(go))
            ObjectSpawnOnDamage.Add(go);

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
        AddDebuffToAplicate(typeof(T));
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
        RemoveDebuffToAplicate(typeof(T));
    }



    #endregion

    #region Powers

    void ActivePower(System.Action<Character, float> action, float f)
    {
        if (power.Count > 0)
        {
            PowerSound();
            action?.Invoke(this,f);
        }
    }

    /// <summary>
    /// Activa el poder con el indice actual
    /// </summary>
    public void ActivePowerDown()
    {
        ActivePower(power[actualPower].ButtonEvent,0);
    }

    /*
    public void ActivePowerPress(float f)
    {
        ActivePower(power[actualPower].ButtonEvent, f);
    }

    public void ActivePowerUp(float f)
    {
        ActivePower(power[actualPower].ButtonEvent, f);
    }
    */
    public void SwitchPower(int i = 0)
    {
        if (power.Count == 0)
            return;

        if(lastPower!=null)
        {
            lastPower.OnExitState(this);
            if (lastPower.on_Update!=null)
            {
                handlerUpdates.Remove(lastPower.GetType());
                if (handlerUpdates.count <= 0)
                    MyUpdates -= HandlerUpdates;
            }
                
        }

        actualPower = i;

        if (actualPower >= power.Count)
        {
            actualPower = power.Count - 1;
        }

        if (actualPower < 0)
        {
            actualPower=0;
        }

        power[actualPower].OnEnterState(this);
        if(power[actualPower].on_Update!=null)
        { 
            handlerUpdates.Add(power[actualPower].GetType(), power[actualPower].on_Update);
            Utilitys.AddSingleAction(ref MyUpdates, HandlerUpdates);
        }
            


        lastPower = power[actualPower];
    }

    /// <summary>
    /// Suma uno al indice del poder de ser posible
    /// </summary>
    public void NextPower()
    {
        if (power.Count - 1 > actualPower)
            SwitchPower(actualPower + 1);
    }

    /// <summary>
    /// resta uno al indice del poder de ser posible
    /// </summary>
    public void PreviosPower()
    {
        if (actualPower > 0 && power.Count > 0)
            SwitchPower(actualPower-1);
    }

    /// <summary>
    /// version privada, contiene todo el algoritmo que se ejecuta cuando se agrega un poder
    /// </summary>
    /// <param name="powerStatic"></param>
    void AddPower(Powers_FatherPwDbff powerStatic, int i =0)
    {
        power.Insert(i, powerStatic);

        if(CompareTag("Player"))
            MainHud.RefreshPowersUI();

        SwitchPower();
    }

    /// <summary>
    /// Agrega un poder al character
    /// </summary>
    /// <param name="type">Tipo del poder a agregar</param>
    public void AddPower(System.Type type, int i = 0)
    {
        AddPower((Powers_FatherPwDbff)System.Activator.CreateInstance(type), i);
    }

    /// <summary>
    /// Agregua un poder al character
    /// </summary>
    /// <typeparam name="T">Nombre del sript que es un poder</typeparam>
    public void AddPower<T>(int i = 0) where T : Powers_FatherPwDbff, new()
    {
        AddPower(new T(), i);
    }

    /// <summary>
    /// Remplaza el primer poder
    /// </summary>
    /// <typeparam name="T">El poder que se va a agregar</typeparam>
    public void ReplaceFirstPower(System.Type type)
    {
        if(power.Count>0)
            lastPower=power[0];
        RemovePower(0);
        AddPower(type,0);
    }

    /// <summary>
    /// Remplaza el poder actual
    /// </summary>
    /// <typeparam name="T">El poder que se va a agregar</typeparam>
    public void ReplaceFirstPower<T>() where T : Powers_FatherPwDbff, new()
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
            power.RemoveOff(i, this);
        }

        SwitchPower();
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

    #region debuff

    public void AddDebuff(System.Type type)
    {
        var aux = (Debuff_FatherPwDbff)System.Activator.CreateInstance(type);

        aux.Instance(this);

        debuffList.Add(aux);

        if (aux.on_Update != null)
            handlerUpdates.Add(aux.GetType(), aux.on_Update);

        Utilitys.AddSingleAction(ref MyUpdates, HandlerUpdates);
        Utilitys.AddSingleAction(ref MyUpdates, DebuffUpdates);
        
    }

    #endregion

    #region Sonido
    public virtual void AttackSound()
    {
        audioM.Play("Attack");
    }
    public virtual void AuxiliarSound()
    {
        audioM.Play("Attack");
    }
    public virtual void DeathSound()
    {
        audioM.Play("Defeat");
    }
    public virtual void PowerSound()
    {
        audioM.Play("PoweredDagger");
    }
    public virtual void DashSound()
    {
        audioM.Play("Dash");

    }
    public virtual void TakeSound()
    {
        audioM.Play("CallDagger");
    }
    public virtual void ShieldSound()
    {
        audioM.Play("Shield");
    }
    public virtual void LandSound()
    {
        audioM.Play("Land");
    }
    public virtual void ChargeSound()
    {
        audioM.Play("ChargeDagger");
    }


    public virtual void WoRSoundLeft()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
            audioM.Play("StepLeft1");
        else
            audioM.Play("StepLeft2");
    }
    
    public virtual void WoRSoundRight()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
            audioM.Play("StepRight1");
        else
            audioM.Play("StepRight2");
    }

    #endregion

    public void ProyectileEnter(Damage damage)
    {
        health.Substract(damage.amount);
    }

    protected override void Config()
    {
        MyAwakes += MyAwake;
        MyStarts += MyStart;
        MyUpdates += MyUpdate;
        MyFixedUpdates += MyFixedUpdate;
    }

    void MyAwake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<AnimatorController>();
        audioM = GetComponent<AudioManager>();

        maxSpeed = movement.maxSpeed;
    }

    void MyStart()
    {
        gameObject.AddTags(Tag.character);

        coyoteTime = TimersManager.Create(0.1f);


        animator.functions.AddRange(new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               //{ "WoRLeft",WoRSoundLeft},
               //{ "WoRRight",WoRSoundRight},
               { "jump", Jump },
               { "dash", Dash },
               { "roll", Roll },
               {"AttackSound", AttackSound},
               {"AuxiliarSound", AuxiliarSound},
               {"DeathSound", DeathSound},
               {"dashSound", DashSound},
               { "land", LandSound }

           });
        
    }

    void HandlerUpdates()
    {
        foreach (var item in handlerUpdates)
        {
            item.value(this);
        }
    }

    void DebuffUpdates()
    {
        for (int i = debuffList.Count - 1; i >= 0; i--)
        {
            if (debuffList[i].timer.Chck)
            {
                if (debuffList[i].on_Update != null)
                    handlerUpdates.Remove(debuffList[i].GetType());

                debuffList.RemoveOff(i, this);

                if (debuffList.Count == 0)
                    MyUpdates -= DebuffUpdates;

                if (handlerUpdates.count <= 0)
                    MyUpdates -= HandlerUpdates;
            }
        }
    }

    void MyUpdate()
    {
        RefreshAnims();
    }

    void MyFixedUpdate()
    {
 
        animator.FloorDistance(movement.lastFloorDistance);

        if ((movement.dash && movement.relation < 0.7f) || (!movement.dash && animator.CheckAnimations("Dash")))
            DashEnd();

        if ((transform.position.y - previousTransformY) > 0.2f)
            animator.Ascending(true);
        else
            animator.Ascending(false);

        /*
        if (animator.CheckAnimations("Jump"))
            movement.Move(animator.transform.forward);
        

        if (animator.CheckAnimations("Falling"))
            movement.Move(animator.transform.forward, movement.maxSpeed * 0.25f);
        */

        previousTransformY = transform.position.y;
    }

    IEnumerator CoroutineMesh()
    {
        yield return new WaitForSeconds(5);

        gameObject.SetActive(false);
    }

    
}