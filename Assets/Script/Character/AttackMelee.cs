using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
    Enemy_Character _enemyRef;
    float _meleeDamage;

    private void Start()
    {
        _enemyRef = GetComponentInParent<Enemy_Character>();
        _meleeDamage = _enemyRef.damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        var myHealth = other.GetComponent<Health>();

        if(myHealth!=null)
        {
            myHealth.Substract(_meleeDamage);
        }
    }
}
