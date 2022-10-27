using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{

    public Vector3 velocity;
    public Vector3 input;
    public float relation, aceleration;


    public delegate void PrototypeFunc();

    public Pictionarys<string, PrototypeFunc> functions = new Pictionarys<string, PrototypeFunc>();

    Animator animator;

    Vector3 _input;

    AnimatorStateInfo animatorInfo;
    int currentAnimation;
    int previousAnimation;

    public void AddFunction(string name, PrototypeFunc func)
    {
        functions.Add(name, func);
    }

    public void ANIM_EVENT(string name)
    {
        functions[name]();
    }

    #region animator functions

    public void ResetOnFloor()
    {
        animator.ResetTrigger("onFloor");
    }
    public void OnFloor()
    {
        animator.SetTrigger("onFloor");
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void Interact()
    {
        animator.SetTrigger("Interact");
    }
    public void Power()
    {
        animator.SetTrigger("Power");
    }
    public void Take()
    {
        animator.SetTrigger("Take");
    }

    public void ResetJump()
    {
        animator.ResetTrigger("Jump");
    }

    public void Jump()
    {
       animator.SetTrigger("Jump");
    }
    public void Ascending(bool b)
    {
        animator.SetBool("Ascending", b);
    }
    public void Input(bool b)
    {
        animator.SetBool("Input", b);
    }
    public bool Aim()
    {
        return animator.GetBool("Aim");
    }
    public void Aim(bool b)
    {
        animator.SetBool("Aim", b);
    }
    public bool Dash()
    {
        return animator.GetBool("Dash");
    }
    public void Dash(bool b)
    {
        /*
        if (b && !animator.GetBool("Dash"))
            Jump();
        */

        animator.SetBool("Dash", b);
    }
    public void FloorDistance(float f)
    {
        animator.SetFloat("FloorDistance", f);
    }

    public float Local()
    {
        return animator.GetFloat("localZ");
    }

    public void Local(float z)
    {
        animator.SetFloat("localZ", z);
    }
    public void Local(Vector3 v)
    {
        animator.SetFloat("localZ", v.z);
        animator.SetFloat("localX", v.x);
    }

    public void ModelDeath()
    {
        animator.SetTrigger("Death");
    }

    #endregion

    /// <summary>
    /// Chequea si se estan ejecutando algunas de las animaciones pasadas por parametro
    /// </summary>
    /// <param name="animationNames">El nombre del estado de la animacion</param>
    /// <returns>True en caso de que ejecute alguna de las animaciones</returns>
    public bool CheckAnimations(params string[] animationNames)
    {
        return CheckAnimations(0,animationNames);
    }

    /// <summary>
    /// Chequea si se estan ejecutando algunas de las animaciones pasadas por parametro
    /// </summary>
    /// <param name="layer">El numero del layer</param>
    /// <param name="animationNames">El nombre del estado de la animacion</param>
    /// <returns>True en caso de que ejecute alguna de las animaciones</returns>
    public bool CheckAnimations(int layer, params string[] animationNames)
    {
        //string nameHash = "Base Layer.";

        animatorInfo = animator.GetCurrentAnimatorStateInfo(layer);
      
        previousAnimation = currentAnimation;

        currentAnimation = animatorInfo.shortNameHash;

        foreach (var animName in animationNames)
        {
            //print("current :" + currentAnimation + "\nCompare:" + Animator.StringToHash(animName));
            if (Animator.StringToHash(animName) == currentAnimation)
                return true;
        }
        return false;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 vectorRot;

        if (Aim())
        {
            vectorRot = transform.parent.forward;
            Local(_input);
        }
        else
        {
            vectorRot = velocity;

            float _inputLocal = 0;

            if (Mathf.Abs(_input.z) > Mathf.Abs(_input.x))
                _inputLocal =(Mathf.Abs(_input.z));
            else
                _inputLocal=(Mathf.Abs(_input.x));

            if (_inputLocal > 1)
                _inputLocal = 1;
            

            Local(_inputLocal);
        }

        if (input.sqrMagnitude > 0 && velocity.sqrMagnitude > 0)
        {
            Input(true);

            animator.transform.rotation = Quaternion.Slerp
            (
                animator.transform.rotation,
                Quaternion.Euler(0, Utilitys.AngleOffAroundAxis(vectorRot, Vector3.forward, Vector3.up, false), 0),
                Time.deltaTime * aceleration
            );
        }
        else if (velocity.sqrMagnitude < 0.01f && !Dash())
        {
            Input(false);
        }

        if (_input != (input * relation))
        {
            _input = Vector3.Lerp(_input, input * relation, Time.deltaTime * aceleration);
        }
    }
}