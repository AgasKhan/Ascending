using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger_Proyectile : Proyectile, ISwitchState<FSMDagger>
{
    public List<System.Type> powerSteal = new List<System.Type>();

    public DaggerEffect daggerEffect;

    public AudioManager audioM;

    [SerializeReference]
    Interactuable_LogicActive interact;

    [SerializeReference]
    Collider[] colliders;

    FSMDagger fsmDagger;

    Timer localAtrackt = new Timer(1);
    Timer angularAtrackt = new Timer(1);
    public Timer finishTimer= new Timer(1);


    public bool damageCollider
    {
        get
        {
            return colliders[0].enabled;
        }
        set
        {
            colliders[0].enabled = value;
        }
    }

    public bool interactCollider
    {
        set
        {
            colliders[1].enabled = value;
            interact.diseable = !value;
        }
    }

    public IState<FSMDagger> CurrentState 
    { 
        get => fsmDagger.CurrentState; 
        set => fsmDagger.CurrentState=value; 
    }

    public void SetLine(Vector3 p1, Vector3 p2)
    {
        daggerEffect.SetLine(p1, p2);
    }

    public void MoveLerpToParent(System.Func<Vector3> vec, float time, Transform parent)
    {
        var aux = ((MoveRotAndGlueRb)MoveRb);

        if (aux.glue.transform.parent != null)
            OnExit(aux.glue.transform.parent.gameObject);

        fsmDagger.CurrentState=fsmDagger.travel;
        
        localAtrackt = Utilitys.LerpInTime(transform.position, vec, time, Vector3.Lerp, (saveData) => {transform.position = saveData; });

        angularAtrackt = Utilitys.LerpInTime(transform.rotation, Quaternion.identity, time, Quaternion.Slerp, (saveData) => {transform.rotation = saveData; });

        finishTimer = TimersManager.Create(time,
            () =>
            {
                transform.parent = parent;
                fsmDagger.CurrentState=fsmDagger.orbit;
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

    public override void Throw(Damage dmg, Vector3 dir, float multiply)
    {
        fsmDagger.CurrentState=fsmDagger.shoot;
        base.Throw(dmg, dir, multiply);
    }

    protected override void OnEnter(Collider other)
    {
        fsmDagger.CurrentState=fsmDagger.ground;

        base.OnEnter(other);
        ((MoveRotAndGlueRb)MoveRb).AddGlue(other.transform);
        CasterObject();
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

    public void StealPowers(Character ch)
    {
        if (ch.power.Count > 0)
            for (int i = 0; i < ch.power.Count; i++)
            {
                powerSteal.Add(ch.power[i].GetType());
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

    private void Awake()
    {
        StartCoroutine(PosAwake());
    }

    public void Init()
    {
        if(fsmDagger==null)
            fsmDagger = new FSMDagger(this);
    }

    private void FixedUpdate()
    {
        fsmDagger?.UpdateState();
    }

    IEnumerator PosAwake()
    {
        yield return null;
        Init();
    }
}


public class FSMDagger : FSM<FSMDagger, Dagger_Proyectile>
{
    public IState<FSMDagger> ground = new Ground();
    public IState<FSMDagger> travel = new Travel();
    public IState<FSMDagger> orbit = new Orbit();
    public IState<FSMDagger> shoot = new Shoot();

    public FSMDagger(Dagger_Proyectile reference) : base(reference)
    {
        Init(ground);
    }
}

public class Ground : IState<FSMDagger>
{
    public void OnEnterState(FSMDagger param)
    {
        param.context.MoveRb.kinematic = true;

        param.context.interactCollider = true;
    }

    public void OnExitState(FSMDagger param)
    {
        param.context.interactCollider = false;

        param.context.MoveRb.kinematic = false;
    }

    public void OnStayState(FSMDagger param)
    {
    }
}

public class Shoot : IState<FSMDagger>
{
    public void OnEnterState(FSMDagger param)
    {
        param.context.damageCollider = true;
    }

    public void OnExitState(FSMDagger param)
    {
        param.context.damageCollider = false;
        param.context.MoveRb.useGravity = false;
    }

    public void OnStayState(FSMDagger param)
    {
    }
}

public class Travel : IState<FSMDagger>
{
    public void OnEnterState(FSMDagger param)
    {
        param.context.MoveRb.kinematic = true;
        param.context.MoveRb.eneableDrag = true;
        param.context.transform.parent = null;


        param.context.MoveRb.Stop();
    }

    public void OnExitState(FSMDagger param)
    { 
    }

    public void OnStayState(FSMDagger param)
    {
        if ((param.context.transform.parent == null || ((MoveRotAndGlueRb)param.context.MoveRb).glue.transform.parent==null) && param.context.finishTimer.Chck)
        {
            param.context.MoveRb.useGravity = true;
            param.CurrentState=param.orbit;
            param.CurrentState=param.shoot;
        }
    }
}

public class Orbit : IState<FSMDagger>
{
    public void OnEnterState(FSMDagger param)
    {
    }

    public void OnExitState(FSMDagger param)
    {
        param.context.MoveRb.kinematic = false;
        param.context.MoveRb.eneableDrag = false;
        param.context.transform.parent = null;
    }

    public void OnStayState(FSMDagger param)
    {
    }
}
