using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Character : Character, ISwitchState<FSMAimingPlayer>
{
    [Space]
    [Header("Player propertys")]
    [Space]
    public Float_KnifeElements floatElements;
    public Attack_KnifeElements attackElements;
    public CameraParent cameraScript;
    public Dagger_Proyectile dagger;

    public float timeInteractMultiply=1;

    public bool UnlockAtrackt;

    public int actualDaggers;
    public int totalDaggers
    {
        get;
        private set;
    }
    public IState<FSMAimingPlayer> CurrentState 
    { 
        get => fsmAiming.CurrentState; 
        set => fsmAiming.CurrentState=value; 
    }

    public FSMAimingPlayer fsmAiming;

    Action inter;

    public void MainHUDDaggers(int n)
    {
        actualDaggers = n;
        MainHud.DaggerText(actualDaggers, totalDaggers);
    }

    public void AttackDist()
    { 
        if(attackElements.Attack())
        {
            //_actualDaggers--;
            MainHud.RemoveAllBuffs();
            AttackSound();
        }
        //MainHud.DaggerText(_actualDaggers, _totalDaggers);
        MainHud.ReticulaFill(0);
    }

    public void Flip(float f)
    {
        cameraScript.Flip();
        attackElements.distance = new Vector3(attackElements.distance.x * -1, attackElements.distance.y, attackElements.distance.z);
    }

    public void Take()
    {
        if (dagger == null)
            return;

        dagger.transform.parent = floatElements.transform;

        if (dagger.owner == null)
            totalDaggers++;

        TakeSound();
    }
    public void Sprint(float n)
    {
        movement.maxSpeed = maxSpeed;
    }

    public void Walk(float n)
    {
        movement.maxSpeed = maxSpeed / 2;
    }

    public void Interact()
    {
        interactuable.Activate();
    }

    public override void OffMesh()
    {
        //print("Moriste");
        //MenuManager.instance.OpenCloseMenu();

        DeathSound();

        GameManager.ReverseAllCoroutine();
    }

    void flag() { }

   
    public void Jump(float number)
    {
        if (fsmAiming.CurrentState != fsmAiming.noAiming)
            return;

        if (!coyoteTime.Chck || _extraJumps >= 0)
        { 
            _extraJumps--;
            animator.Jump();
        }
    }
    
    public void Dash(float number)
    {
        if (number < 0.3f)
            animator.Dash(true);
    }

    private void Active_eventDown(float obj)
    {
        Controllers.active.OnExitState();
    }

    private void Active_eventUp(float obj)
    {
        animator.Cancel();
    }

    void Action(float pressed)
    {
        if (interactuable == null)
            return;

        float timePressed = interactuable.pressedTime * 1 / timeInteractMultiply;

        interactuable.RefreshUi(cameraScript.cam.WorldToScreenPoint(interactuable.transform.position), (pressed / timePressed));

        if (pressed > 0 && pressed < timePressed)
        {
            if (!animator.CheckAnimations(1, "Take_dagger", "Interact"))
            {
                inter = Interact;

                if (interactuable.transform.parent != null && interactuable.transform.parent.CompareTag("Dagger"))
                {
                    animator.Take(timePressed);
                    inter += Take;
                }
                else
                {
                    animator.Interact(timePressed);
                }

            }
        }
        else if (pressed >= timePressed)
        {
            inter();
            Controllers.active.timePressed = 0;
        }
        else
        {
            animator.Cancel();
            Controllers.active.timePressed = 0;
        }
    }

    private void Ground_onExit(FSMEntorno obj)
    {
        Controllers.dash.eventDown -= Sprint;
        Controllers.dash.eventUp -= Walk;

        movement.maxSpeed = maxSpeed / 10;

        animator.ResetOnFloor();
    }

    private void Ground_onEnter(FSMEntorno obj)
    {
        Controllers.dash.eventDown += Sprint;
        Controllers.dash.eventUp += Walk;

        _extraJumps = extraJumps;

        animator.OnFloor();
        Walk(0);
    }

    private void Ground_onStay(FSMEntorno obj)
    {
        coyoteTime.Reset();
    }

    #region unity Functions

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
        MyStarts += MyStart;
        MyUpdates += MyUpdate;
        MyFixedUpdates += MyFixedUpdate;
    }

    void MyAwake()
    {
        GameManager.player = this;

        Controllers.active.eventDown += Active_eventDown;
        Controllers.active.eventPress += Action;
        Controllers.active.eventUp += Active_eventUp;

        Controllers.flip.eventDown += Flip;

        Controllers.jump.eventDown += Jump;

        Walk(0);
    }

    void MyStart()
    {
        animator.functions.AddRange(
           new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               { "attack",AttackDist },
               { "take",flag },
               { "power",ActivePowerDown },
               { "interact", flag },
               { "offMesh", OffMesh }
           });

        fsmAiming = new FSMAimingPlayer(this);

        movement.entorno.ground.onEnter += Ground_onEnter;
        movement.entorno.ground.onExit += Ground_onExit;
        movement.entorno.ground.onStay += Ground_onStay;
    }

    void MyUpdate()
    {
        Vector2 mouse = new Vector2();

        input.x = Controllers.horizontal.pressed;
        input.z = Controllers.vertical.pressed;

        mouse.x = Controllers.horizontalMouse.pressed;
        mouse.y = Controllers.verticalMouse.pressed;       

        if (mouse.sqrMagnitude > 0)
        {
            cameraScript.Rotate(mouse);
        }

        if (interactuable != null && !interactuable.diseable)
        {
            interactuable.RefreshUi(cameraScript.cam.WorldToScreenPoint(interactuable.transform.position), 0);
        }
        else
        {
            InteractiveObj.instance.CloseInfo();
            Controllers.active.OnExitState();
        }

        if (Controllers.power.down && power.Count > 0)
        {
            animator.Power();

            MainHud.AddBuff();
        }

        fsmAiming.UpdateState();
    }
    void MyFixedUpdate()
    {
        if (input.sqrMagnitude > 0)
        { 
            movement.MoveLocal(input);
            movement.RotateY(cameraScript.transform.GetChild(0).rotation.eulerAngles.y);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            fsmAiming.CurrentState=fsmAiming.noAiming;
        }
    }
    #endregion
}

public class FSMAimingPlayer : FSM<FSMAimingPlayer, Player_Character>
{
    public IState<FSMAimingPlayer> aiming = new Aiming();
    public IState<FSMAimingPlayer> noAiming = new NoAiming();
    public FSMAimingPlayer(Player_Character player) : base(player)
    {
        Init(noAiming);
    }

    #region public event functions

    #region Attack button
    public void Attack_eventPress(float obj)
    {
        if (context.actualDaggers > 0)
        {
            MainHud.ReticulaFill(context.attackElements.ChargeAttack());
        }
    }
    public void Attack_eventUp(float number)
    {
        if (context.actualDaggers > 0)
        {
            context.animator.Attack();
        }
    }


    public void Attack_eventPressNO(float number)
    {
        if (context.UnlockAtrackt && context.actualDaggers < context.totalDaggers)
        {
            if (number > context.totalDaggers)
            {
                foreach (var item in context.gameObject.FindWithTags("Dagger"))
                {

                    context.dagger = item.GetComponent<Dagger_Proyectile>();

                    if (context.dagger.owner != null)
                    {
                        context.Take();
                    }
                }
            }
            else
            {
                MainHud.DaggerPower(number / context.totalDaggers);
            }
        }
    }

    public void Attack_eventUpNO(float obj)
    {
        MainHud.DaggerPower(0);
    }
    #endregion


    public void Aim_eventUp(float obj)
    {
        CurrentState=noAiming;
    }  
    public void Aim_eventDownNO(float obj)
    {
        CurrentState = aiming;
    }

    #endregion
}

public class Aiming : IState<FSMAimingPlayer>
{
    public void OnEnterState(FSMAimingPlayer param)
    {
        param.context.attackElements.PreAttack();
        param.context.cameraScript.ZoomIn(new Vector3(-GameManager.player.cameraScript.offSet.x / 2, 0, 2));
        param.context.animator.Aim(true);

        Controllers.attack.eventUp += param.Attack_eventUp;
        Controllers.attack.eventPress += param.Attack_eventPress;

        Controllers.aim.eventUp += param.Aim_eventUp;
    }

    public void OnExitState(FSMAimingPlayer param)
    {
        param.context.attackElements.CancelAttack();
        param.context.cameraScript.ZoomOut();
        param.context.animator.Aim(false);

        Controllers.attack.eventUp -= param.Attack_eventUp;
        Controllers.attack.eventPress -= param.Attack_eventPress;

        Controllers.aim.eventUp -= param.Aim_eventUp;
    }

    public void OnStayState(FSMAimingPlayer param)
    {
    }
}

public class NoAiming : IState<FSMAimingPlayer>
{
    public void OnEnterState(FSMAimingPlayer param)
    {
        Controllers.aim.eventDown += param.Aim_eventDownNO;

        //Controllers.jump.eventDown += Jump;
        Controllers.dash.eventUp += param.context.Dash;
        Controllers.attack.eventPress += param.Attack_eventPressNO;
        Controllers.attack.eventUp += param.Attack_eventUpNO;
    }

    public void OnExitState(FSMAimingPlayer param)
    {
        Controllers.aim.eventDown -= param.Aim_eventDownNO;

        //Controllers.jump.eventDown -= Jump;
        Controllers.dash.eventUp -= param.context.Dash;
        Controllers.attack.eventPress -= param.Attack_eventPressNO;
        Controllers.attack.eventUp -= param.Attack_eventUpNO;

        param.Attack_eventUpNO(0);
    }

    public void OnStayState(FSMAimingPlayer param)
    {
    }
}

