using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCh_LogicActive : LogicActive
{
    [SerializeReference]
    MonoBehaviour[] _controlers;

    AnimatorController _animController;

    Animator _anim;

    Collider[] _coll;

    Rigidbody _rb;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();

        _animController = GetComponentInChildren<AnimatorController>();

        _coll = GetComponentsInChildren<Collider>();

        _rb = GetComponentInChildren<Rigidbody>();

        _controlers = GetComponentsInChildren<Character>();
    }

    //1ero damage, 2do life, 3ero percentage life
    public override void Activate(params float[] floatParms)
    {
        if (floatParms[0] > 0 && _anim!=null)
            _anim.SetTrigger("Damage");
        else if(floatParms[0] < 0 && _anim != null)
            _anim.SetTrigger("Restore");


        if (floatParms[1] <= 0 && !_animController.CheckAnimations("Charcter_Death"))
        {
            print(this.name + " ha muerto");

            gameObject.AddTags("Death");

            if (_anim != null)
                _anim.SetTrigger("Death");

            if(_rb!=null)
                _rb.isKinematic = true;

            for (int i = 0; i < _controlers.Length; i++)
                _controlers[i].enabled = false;

            for (int i = 0; i < _coll.Length; i++)
                _coll[i].enabled = false;

        }     
        else
        {
            gameObject.RemoveTags("Death");
        }
    }
}
