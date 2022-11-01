using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase padre de los enemigos
/// </summary>
abstract public class Enemy_Character : Character, IPatrolReturn
{
    #region attributes

    /// <summary>
    /// Contiene una referencia del jugador
    /// </summary>
    public Character player;

    /// <summary>
    /// almacena el proctil que se disparara
    /// </summary>
    public Proyectile proyectile;

    /// <summary>
    /// timer por defecto para el deley del ataque
    /// </summary>
    public Timer attackDelay;

    /// <summary>
    /// referencia de la patrulla
    /// </summary>
    public Patrol patrol;

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
    [Range(0, 180)]
    public float coneOfVision;

    /// <summary>
    /// distancia minima a la cual se va a acercar al objetivo
    /// </summary>
    [Range(0, 50)]
    public float minimalDistancePlayer;

    /// <summary>
    /// tiempo de espera para volver a su patrulla
    /// </summary>
    [Range(1, 20)]
    public float waitMetalGear;

    /// <summary>
    /// tiempo de enfriamiento para el siguiente ataque
    /// </summary>
    public float attackCooldown;

    public GameObject attackCollider;

    public Timer deleySearch;

    [SerializeField]
    LayerMask layerMask;

    float _minimalDistancePatrol = 4;

    [SerializeField]
    float _minimalDistance = 2;

    Vector3 _direction;

    float _rotation;

    float _angle;

    Vector3 _lastPosition;

    #endregion

    #region functions
    public Patrol PatrolReturn()
    {
        return patrol;
    }

    virtual protected void Shoot()
    {
        //Proyectile proyScript = Instantiate(proyectile, transform.position + transform.forward, transform.rotation);

        GameObject aux = PoolObjects.SpawnPoolObject(0, proyectile.name, transform.position + transform.forward, transform.rotation);

        Proyectile proyScript = aux.GetComponent<Proyectile>();

        proyScript.owner = this;
        proyScript.MoveRb.MoveLocal(Vector3.forward, 10 * proyScript.MoveRb.maxSpeed);

        GameManager.DeActivateRetarded(aux, 3);
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

                    Debuff_FatherPwDbff.SchDebuff(debuffToAplicate[0]).Add(chr);
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
            Ray ray = new Ray(transform.position, playerDirection);

            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit, detectedRadio, layerMask))
            {
                scoped = raycastHit.collider;

                scopedPoint = raycastHit.point;

                if (raycastHit.collider.CompareTag("Player"))
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

    protected override void MyStart()
    {
        StartCoroutine(PostAwake());

        patrol.Start(this);

        movement = GetComponent<Movement>();
        health = GetComponent<Health>();

        animator = GetComponentInChildren<AnimatorController>();

        animator.AddFunction("offMesh", OffMesh);

        GameManager.AddEnemy(this);

        base.MyStart();
    }

    protected override void MyUpdate()
    {
        if (player == null)
            return;

        RefreshAnims();

        if (deleySearch.Chck())
            _direction = patrol.Distance();
        else
            _direction = _lastPosition - transform.position;

        _direction = PlayerDetect(out playerDetF, out _minimalDistance);

        _angle = movement.RotateToDirY(_direction);

        if (_angle < coneOfVision/2 && !MinimalChck())
            input = (Vector3.forward);

        else if (MinimalChck() && !deleySearch.Chck())
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

        scoped = null;
        scopedPoint = Vector3.zero;
    }

    protected override void MyFixedUpdate()
    {
        if (player == null)
            return;

        movement.MoveLocal(input);
    }

    protected override void MyAwake()
    {

    }

    IEnumerator PostAwake()
    {
        yield return null;

        player = GameManager.player;

        attackDelay = Timers.Create(attackCooldown);
        deleySearch = Timers.Create(0);

        _rotation = transform.rotation.eulerAngles.y;

        //inicializo los poderes del enemigo
        foreach (Powers_FatherPwDbff item in power)
        {
            if (item == null)
            {
                Debug.LogWarning("Tiene el poder vacio el enemigo: " + name);
                break;
            }

            item.chrAffected.Add(this);
            item.On(this);
        }

    }

    #endregion
}