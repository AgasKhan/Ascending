using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSelecctor : MonoBehaviour
{
    Vector3 powerSlctrPos;
    [SerializeField]
    float velocity=1;

    Animator animator;

    public void Animation(string anim)
    {
        animator.Play(anim);
    }

    public void PowerSlctrPos(Vector3 v)
    {
        powerSlctrPos = v;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        powerSlctrPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).shortNameHash != Animator.StringToHash("New"))
        {
            transform.position = Vector3.Lerp(transform.position, powerSlctrPos, velocity * Time.deltaTime);


            if ((transform.position - powerSlctrPos).sqrMagnitude < 0.1f)
                enabled = false;
        }
            
        
    }
}
