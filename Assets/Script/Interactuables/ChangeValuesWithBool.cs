using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeValuesWithBool : LogicActive
{
    public bool invert;
    public SetInts[] setInts;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < setInts.Length; i++)
        {
            setInts[i].inter = setInts[i].mono.GetComponent<ISetInt>();
            setInts[i].inter.SetInt(setInts[i].oldValue);
        }
    }

    public override void Activate(params bool[] bul)
    {
        foreach (var item in setInts)
        {
            item.inter.SetInt(bul[0] ? item.newValue : item.oldValue);
        }
    }

    [System.Serializable]
    public struct SetInts
    {
        public MonoBehaviour mono;
        public ISetInt inter;
        public int newValue;
        public int oldValue;
    }

}


