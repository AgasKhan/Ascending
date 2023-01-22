using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Clase padre de los enemigos
/// </summary>
abstract public class Enemy_Character : Character, IPatrolReturn
{
    #region attributes
    [Space]
    [Header("Enemys propertys")]
    [Space]

    /// <summary>
    /// Contiene una referencia del jugador
    /// </summary>
    public Character player;

    /// <summary>
    /// almacena el proctil que se disparara
    /// </summary>
    public Proyectile proyectile;

    /// <summary>
    /// referencia de la patrulla
    /// </summary>
    public Patrol patrol;

    [Header("Detection")]

    /// <summary>
    /// Funcion que permite saber si el jugador fue detectado por el enemigo 
    /// mas eficiente, menos precisa
    /// </summary>
    public bool playerDetF;

    /// <summary>
    /// Funcion que permite saber si el jugador fue detectado por el enemigo
    /// mas precisa, menos eficiente
    /// </summary>
    public bool playerDetE
    {
        get
        {
            bool result;
            PlayerDetect(out result, out float n);
            return result;
        }
    }
    /// <summary>
    /// radio de deteccion del enemigo
    /// </summary>
    [Range(1, 50)]
    public float detectedRadio;

    /// <summary>
    /// angulo de vision a partir del forward
    /// </summary>
    [Range(0, 360)]
    public float coneOfVision;

    /// <summary>
    /// Layer de deteccion
    /// </summary>
    [SerializeField]
    protected LayerMask layerMask;

    /// <summary>
    /// tiempo de espera para volver a su patrulla
    /// </summary>
    [Range(1, 20)]
    public float waitMetalGear;
    
    /// <summary>
    /// Timer para la espera antes de volver a su ruta
    /// </summary>
    public Timer deleySearch;

    [Header("Atack")]

    /// <summary>
    /// tiempo de enfriamiento para el siguiente ataque
    /// </summary>
    public float attackCooldown;

    /// <summary>
    /// timer por defecto para el deley del ataque
    /// </summary>
    public Timer attackDelay;


    [Header("Movimiento")] 

    /// <summary>
    /// distancia minima a la cual se va a acercar al objetivo
    /// </summary>
    [Range(0, 50)]
    public float minimalDistancePlayer;

    float _minimalDistancePatrol = 4;

    [SerializeField]
    float _minimalDistance = 2;

    Vector3 _direction;

    float _rotation;

    float _angle;

    Vector3 _lastPosition;

    NavMeshAgent nav;

    #endregion

    #region functions
    public Patrol PatrolReturn()
    {
        return patrol;
    }

    virtual protected void Shoot()
    {
        //Proyectile proyScript = Instantiate(proyectile, transform.position + transform.forward, transform.rotation);

        Proyectile proyScript = (Proyectile)PoolObjects.SpawnPoolObject(0, proyectile.name, transform.position + transform.forward, Quaternion.identity);

        proyScript.damage.SetWithCharacter(this);

        proyScript.owner = this;

        proyScript.MoveRb.Move((player.transform.position - transform.position));

        GameManager.DeActivateRetarded(proyScript.gameObject, 3);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.forward, 0.5f);
    }

    protected void AttackMelee()
    {
        Debug.Log("El enemigo atacó");

        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward, 0.5f);

        foreach (Collider item in colliders)
        {
            if (name != item.name)
            {
                Character chr = item.GetComponent<Character>();
                if (chr != null)
                {
                    chr.health.Substract(damage);

                    print(debuffToAplicate[0].FullName);

                    chr.AddDebuff(debuffToAplicate[0]);
                }
            }
        }
        //attackCollider.SetActive(true);
    }

    protected void EndAttackMelee()
    {
        //attackCollider.SetActive(false);
    }

    /// <summary>
    /// version modificada del raycast donde ya configura el scope y el scopepoint con los valores del raycasthit
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    protected bool Raycast(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit rayCastHit, detectedRadio, layerMask))
        {
            scoped = rayCastHit.collider;

            scopedPoint = rayCastHit.point;

            return true;
        }

        return false;
    }

    /// <summary>
    /// version simplificada del raycast, donde ya esta configurado el radio de deteccion y el layer mask
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="raycastHit"></param>
    /// <returns></returns>
    protected bool Raycast(Ray ray, out RaycastHit rayCastHit)
    {
        return Physics.Raycast(ray, out rayCastHit, detectedRadio, layerMask);
    }


    /// <summary>
    /// Funcion que devolvera el vector direccion con respecto al jugador en caso de detectarlo, o la direccion puesta como parametro en caso que no
    /// Guardara el paramtro booleano un true en caso de detectarlo
    /// </summary>
    /// <param name="playerDetect">Si detecto o no al jugador</param>
    /// <param name="direction">El vector direccion que tomara el enemigo, ya sea su patrulla o el jugador</param>
    /// <returns></returns>
    protected Vector3 PlayerDetect(out bool playerDetect, out float distance)
    {

        Vector3 playerDirection = player.transform.position - transform.position;

        if (Utilitys.DeltaAngleY(playerDirection, out float angle, transform.rotation) < coneOfVision/2)
        {
            if (Raycast(new Ray(transform.position, playerDirection)))
            {
                if (scoped.CompareTag("Player"))
                {
                    distance = minimalDistancePlayer;
                    playerDetect = true;
                    _lastPosition = player.transform.position;
                    deleySearch.Set(waitMetalGear);

                    return playerDirection;
                }
            }
        }
        distance = _minimalDistancePatrol;
        playerDetect = false;
        return _direction;
    }

    /// <summary>
    /// Si el player ha sido detectado, devuelve si llego a su distancia minima
    /// si no, devolvera si llego a la distancia minima con respecto al punto de guardia
    /// </summary>
    /// <returns>Booleano</returns>
    protected bool MinimalChck()
    {
        if (!playerDetF)
            return patrol.MinimalChck(_minimalDistance);

        if (_direction.sqrMagnitude < _minimalDistance * _minimalDistance)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region unity functions

    protected override void Config()
    {
        base.Config();

        MyStarts += MyStart;
        MyUpdates += MyUpdate;
        MyFixedUpdates += MyFixedUpdate;
    }

    void MyStart()
    {
        StartCoroutine(PostAwake());

        patrol.Start(this);

        /*
        nav = GetComponent<NavMeshAgent>();

        nav.speed = movement.maxSpeed;
        nav.angularSpeed = movement.desAcelerationAxis;
        nav.acceleration = movement.desAceleration;
        */

        animator.AddFunction("offMesh", OffMesh);

        GameManager.AddEnemy(this);
    }

    void MyUpdate()
    {
        if (player == null)
            return;

        scoped = null;
        scopedPoint = Vector3.zero;

        if (deleySearch.Chck)
            _direction = patrol.Distance();
        else
            _direction = _lastPosition - transform.position;

        _direction = PlayerDetect(out playerDetF, out _minimalDistance);

        _angle = movement.RotateToDirY(_direction);

        if (_angle < coneOfVision/2 && !MinimalChck())
            input = (Vector3.forward);

        else if (MinimalChck() && !deleySearch.Chck)
            input = (Vector3.right * -1);

        else if (MinimalChck())
        {
            input = Vector3.zero;
            movement.RotateY(_rotation);
        }

        /*
        if (scoped != null)
            print("Enemy see: " + scoped.name);
        */

    }

    void MyFixedUpdate()
    {
        if (player == null)
            return;

        movement.MoveLocal(input);
    }

    IEnumerator PostAwake()
    {
        yield return null;

        player = GameManager.player;

        attackDelay = TimersManager.Create(attackCooldown);
        deleySearch = TimersManager.Create(0);

        _rotation = transform.rotation.eulerAngles.y;
    }

    #endregion
}