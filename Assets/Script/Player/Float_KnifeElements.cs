using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float_KnifeElements : KnifeElements
{
    void Order()
    {
        System.Type[] type=null;
        float angle = 360;
        List<Transform> aux = new List<Transform>();
        aux.AddRange(transform.GetChild(0).GetComponentsInChildren<Transform>());
        aux.RemoveAt(0);
        elements.Clear();

        for (int i = 0; i < aux.Count; i++)
        {
            angle -= 360 / aux.Count;

            if(aux[i].GetComponentInChildren<Dagger_Proyectile>()!=null)
                elements.Add(new Knifes(aux[i], Quaternion.Euler(0, angle, 0) * distance, character));  

            if(elements[elements.Count - 1].daggerScript.powerSteal.Count>0)
                type = elements[elements.Count-1].daggerScript.powerSteal.ToArray();

            if(type != null)
            {
                character.ReplaceFirstPower(type[0]);
                /*else
                {
                    for (int ii = 0; ii < type.Length; ii++)
                    {
                        character.AddPower(type[ii]);
                    }
                }*/
                elements[elements.Count - 1].daggerScript.powerSteal.Clear();
            }
        }   
    }


    private void Update()
    {
        bool checkDistance = true;

        transform.GetChild(0).RotateAround(character.transform.position, Vector3.up, journeyTime * Time.deltaTime);
            
        for (int i = 0; i < elements.Count; i++)
        {
            //antiguamente un slerp
            elements[i].reference.localPosition = Vector3.Slerp(elements[i].reference.localPosition, elements[i].position, Time.deltaTime*2);
        }

        for (int i = 1; i < transform.childCount; i++)
        {
            MoveRotAndGlueRb move = transform.GetChild(i).GetComponent<MoveRotAndGlueRb>();

            move.kinematic = false;
            move.eneableDrag = true;
            move.Move(transform.position - transform.GetChild(i).position);

            if ((transform.position - transform.GetChild(i).position).sqrMagnitude <= (distance * 2).sqrMagnitude)
            {
                move.Stop();
                move.kinematic = true;
                transform.GetChild(i).parent = transform.GetChild(0);
            }
        }

        foreach (Transform item in transform)
        {
            if ((item.position - transform.position).sqrMagnitude > (distance * 3).sqrMagnitude)
            {
                checkDistance = false;
            }
        }

        if (transform.GetChild(0).childCount != elements.Count && checkDistance)
        {
            Order();
            if (Controllers.aim.pressed && Other.transform.childCount == 0)
                ((Attack_KnifeElements)Other).PreAttack();
        }


    }
}