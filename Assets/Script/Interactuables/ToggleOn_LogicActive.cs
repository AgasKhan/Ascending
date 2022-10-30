using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOn_LogicActive : LogicActive
{
    [SerializeField]
    Behaviour[] components;

    [SerializeField]
    GameObject[] gameObjects;

    public override void Activate()
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(!item.activeSelf);
        }

        foreach (var item in components)
        {
            item.enabled = !item.enabled;
        }
    }
}
