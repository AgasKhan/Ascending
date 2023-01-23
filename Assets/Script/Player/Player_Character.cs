using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Character : Character
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

    float timePressed;

    public int totalDaggers
    {
        get;
        private set;
    }

    int _actualDaggers;
    bool _previusOnFloor;
    bool _sprint;

    Action inter;

    public void MainHUDDaggers(int n)
    {
        _actualDaggers = n;
        MainHud.DaggerText(_actualDaggers, totalDaggers);
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

    public void Flip()
    {
        cameraScript.Flip();
        attackElements.distance = new Vector3(attackElements.distance.x * -1, attackElements.distance.y, attackElements.distance.z);
    }

    public void Take()
    {
        if (dagger == null)
            return;

        dagger.MoveRb.kinematic = false;

        /*
        if (dagger.transform.parent != null && dagger.transform.parent != floatElements.transform)
        {
            print(dagger.gameObject.transform.parent.name);
        }
        */

        dagger.transform.parent = floatElements.transform;

        if (dagger.owner == null)
            totalDaggers++;

        if(interactuable)
            interactuable.diseable = true;

        /*
        _actualDaggers++;
        if (_actualDaggers > _totalDaggers)
            _actualDaggers = _totalDaggers;
        */

        TakeSound();
        //MainHud.DaggerText(_actualDaggers, _totalDaggers);
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


    void MyStart()
    {
        animator.functions.AddRange(
           new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               { "attack",AttackDist },
               { "take",flag },
               { "power",ActivePowerDown },
               { "interact", flag },
               { "offMesh", OffMesh },
               { "land", LandSound },
           });

        maxSpeed = movement.maxSpeed;

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
    }

    
    
    void MyUpdate()
    {
        Vector2 mouse = new Vector2();
        float pressed = 0;

        input.x = Controllers.horizontal.pressed;
        input.z = Controllers.vertical.pressed;

        mouse.x = Controllers.horizontalMouse.pressed;
        mouse.y = Controllers.verticalMouse.pressed;

        if (movement.isOnFloor)
        {
            coyoteTime.Reset();
            if (!_previusOnFloor)
            {
                _previusOnFloor = true;
                animator.OnFloor();
            }
            else
            {
                animator.ResetOnFloor();
            }
        }
        
        _previusOnFloor = movement.isOnFloor;

        

        if (mouse.sqrMagnitude > 0)
        {
            cameraScript.Rotate(mouse);
        }

        if (interactuable != null && !interactuable.diseable)
        {

            timePressed = interactuable.pressedTime * 1/timeInteractMultiply;

            if ((pressed = Controllers.active.TimePressed())>0 && pressed< timePressed)
            {
                if (!animator.CheckAnimations(1,"Take_dagger", "Interact"))
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
            else if (pressed > timePressed)
            {
                inter();
            }
            
            
            interactuable.RefreshUi(cameraScript.cam.WorldToScreenPoint(interactuable.transform.position), (pressed / timePressed));


            if(pressed==0)
                animator.Cancel();
        }

        else
        {
            InteractiveObj.instance.CloseInfo();
            animator.Cancel();
        }

        if (interactuable == null || Controllers.active.TimePressed() > timePressed)
        {
            Controllers.active.TimePressed(false);
        }
        
        if(!Controllers.aim.pressed)
        {
            if (Controllers.jump.down && animator.CheckAnimations("Movement", "standing idle"))
            {

                animator.Jump();
            }
            else if (!_sprint && Controllers.dash.up && animator.CheckAnimations("Movement", "standing idle", "Jump", "Jumping Up", "Asending"))
            {

                //animator.Jump();
                animator.Dash(true);
            }
        }

        if (Controllers.dash.TimePressed(true) > 0.3)
            _sprint = true;
        else
            _sprint = false;

        if (Controllers.attack.up && Controllers.aim.pressed && _actualDaggers > 0)
            animator.Attack();


        if(Controllers.attack.pressed)
        {
            if (Controllers.aim.pressed)
                MainHud.ReticulaFill(attackElements.ChargeAttack());
                

            //poder de atraer las dagas
            else if(UnlockAtrackt && _actualDaggers < totalDaggers)
            {
                if(Controllers.attack.TimePressed() > totalDaggers)
                {
                    foreach (var item in gameObject.FindWithTags("Dagger"))
                    {

                        dagger = item.GetComponent<Dagger_Proyectile>();

                        if (dagger.owner != null)
                        {
                            Take();
                        }
                    }

                    Controllers.attack.TimePressed(false);
                }
                else
                {
                    MainHud.DaggerPower(Controllers.attack.TimePressed() / totalDaggers);
                }
                
            }
        }
        else if (UnlockAtrackt)
        {
            MainHud.DaggerPower(0);
        }

        if (Controllers.aim.up)
        {
            attackElements.CancelAttack();
            cameraScript.ZoomOut();
            animator.Aim(false);
            MainHud.ReticulaFill(0);
        }

        if (Controllers.aim.down)
        {
            attackElements.PreAttack();
            cameraScript.ZoomIn(new Vector3(-cameraScript.offSet.x/2, 0, 2));
            animator.Aim(true);
        }

        if (Controllers.power.down && power.Count>0)
        {
            animator.Power();

            MainHud.AddBuff();
        }
            

        if (Controllers.flip.down)
        {
            Flip();
        }

        //
    }

    
    void MyFixedUpdate()
    {
       
        if (input.sqrMagnitude > 0)
        {
            if (_sprint && movement.isOnFloor)
            {
                movement.maxSpeed = maxSpeed;
            }
            else if(movement.isOnFloor)
            {
                movement.maxSpeed = maxSpeed / 2;
            }

            movement.MoveLocal(input);

            movement.RotateY(cameraScript.transform.GetChild(0).rotation.eulerAngles.y);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            attackElements.CancelAttack();
            cameraScript.ZoomOut();
            animator.Aim(false);
        }
    }
    #endregion

    #region Sounds

    public override void AttackSound()
    {
        audioM.Play("Attack");
    }
    public override void AuxiliarSound()
    {
        audioM.Play("Attack");
    }
    public override void DeathSound()
    {
        audioM.Play("Defeat");
    }
    public override void PowerSound()
    {
        audioM.Play("PoweredDagger");
    }
    public override void DashSound()
    {
        audioM.Play("Dash");
        
    }
    public void TakeSound()
    {
        audioM.Play("CallDagger");
    }
    public void ShieldSound()
    {
        audioM.Play("Shield");
    }
    public void LandSound()
    {
        audioM.Play("Land");
    }
    public void ChargeSound()
    {
        audioM.Play("ChargeDagger");
    }

    
    #endregion


}