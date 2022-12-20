using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWithToggleObject : SetWithBool_LogicActive
{
    public override void Activate(params bool[] bul)
    {
        Activate();
    }

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
