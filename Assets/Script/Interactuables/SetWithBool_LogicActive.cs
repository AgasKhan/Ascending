using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWithBool_LogicActive : LogicActive
{
    [SerializeField]
    Behaviour[] components;

    [SerializeField]
    GameObject[] gameObjects;

    public override void Activate(params bool[] bul)
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(bul[0]);
        }

        foreach (var item in components)
        {
            item.enabled = bul[0];
        }
    }
}
