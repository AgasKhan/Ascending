using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Character : Character
{
    public Float_KnifeElements floatElements;
    public Attack_KnifeElements atackElements;
    public CameraParent cameraScript;

    public Dagger_Proyectile dagger;

    public float timeInteractMultiply=1;

    public bool UnlockAtrackt;

    float timePressed;

    int _totalDaggers;
    bool _previusOnFloor;
    bool _sprint;

    Action inter;


    public void AttackDist()
    {
        Debug.Log("EL JUGADOR HA ATACADO");
        atackElements.Attack();
        MainHud.RemoveAllBuffs();
    }

    public void Flip()
    {
        cameraScript.Flip();
        atackElements.distance = new Vector3(atackElements.distance.x * -1, atackElements.distance.y, atackElements.distance.z);
    }

    public void Take()
    {
        
        interactuable?.Activate();

        if (dagger == null)
            return;

        dagger.MoveRb.kinematic = false;

        if (dagger.gameObject.transform.parent != null && dagger.gameObject.transform.parent != floatElements.transform)
        {
            print(dagger.gameObject.transform.parent.name);
        }

        dagger.gameObject.transform.parent = floatElements.transform;

        if (dagger.owner == null)
            _totalDaggers++;

        interactuable.diseable = true;
    }

    public void Interact()
    {
        interactuable.Activate();
    }

    void flag() { }

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

    
    void MyStart()
    {
        coll = GetComponent<CapsuleCollider>();

        animator.functions.AddRange(
           new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               { "attack",AttackDist},
               { "take",flag },
               {"power",ActivePower},
               {"interact", flag },
               { "jump", Jump},
               { "dash", Dash },
               { "roll", Roll },
               { "offMesh", OffMesh }
           });

        maxSpeed = movement.maxSpeed;
 
    }
    void MyUpdate()
    {
        float pressed = 0;

        input = new Vector3(Controllers.dir.x, 0, Controllers.dir.y);

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

        if (Controllers.cameraInput.sqrMagnitude > 0)
        {
            cameraScript.Rotate(Controllers.cameraInput);
        }

        if (interactuable != null && !interactuable.diseable)
        {

            timePressed = interactuable.pressedTime * 1/timeInteractMultiply;

            if ((pressed = Controllers.active.TimePressed())>0 && pressed< timePressed)
            {
                if (!animator.CheckAnimations(1,"Take_dagger", "Interact"))
                {
                    if (interactuable.transform.parent != null && interactuable.transform.parent.CompareTag("Dagger"))
                    {
                        animator.Take(timePressed);
                        inter = Take;
                    }
                    else
                    {
                        animator.Interact(timePressed);
                        inter = Interact;
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
            else if (!_sprint && Controllers.dash.up && animator.CheckAnimations("Movement"))
            {

                animator.Jump();
                animator.Dash(true);
            }
        }

        if (Controllers.dash.TimePressed(true) > 0.3)
            _sprint = true;
        else
            _sprint = false;

        if (Controllers.attack.up && Controllers.aim.pressed)
            animator.Attack();


        if(Controllers.attack.pressed)
        {
            if (Controllers.aim.pressed)
                atackElements.ChargeAttack();

            else if(UnlockAtrackt && Controllers.attack.TimePressed()> _totalDaggers)
            {
                foreach (var item in gameObject.FindWithTags("Dagger"))
                {
                    
                    dagger = item.GetComponent<Dagger_Proyectile>();

                    if(dagger.owner!=null)
                    {
                        Take();
                    }
                        
                }

                Controllers.attack.TimePressed(false);
            }
        }

        if (Controllers.aim.up)
        {
            atackElements.CancelAttack();
            cameraScript.ZoomOut();
            animator.Aim(false);
        }

        if (Controllers.aim.down)
        {
            atackElements.PreAttack();
            cameraScript.ZoomIn(new Vector3(-cameraScript.offSet.x/2, 0, 2));
            animator.Aim(true);
        }

        if (Controllers.power.down)
        {
            animator.Power();

            MainHud.AddBuff();
        }
            

        if (Controllers.flip.down)
        {
            Flip();
        }

        scoped = null;
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

        if (input.sqrMagnitude > 0)
        {
            if (_sprint)
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

        if(animator.CheckAnimations("Jump"))
            movement.Move(animator.transform.forward);

        if (animator.CheckAnimations("Falling"))
            movement.Move(animator.transform.forward, movement.maxSpeed*0.25f);


        previousTransformY = transform.position.y;
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            atackElements.CancelAttack();
            cameraScript.ZoomOut();
            animator.Aim(false);
        }
    }
    #endregion
}