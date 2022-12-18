using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeValues : LogicActive
{
    public bool invert;
    public SetInts[] setInts;

    bool _invert;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < setInts.Length; i++)
        {
            setInts[i].inter = setInts[i].mono.GetComponent<ISetInt>();
            setInts[i].inter.SetInt(setInts[i].oldValue);
        }
    }

    public override void Activate()
    {
        foreach (var item in setInts)
        {
            item.inter.SetInt(_invert && invert ? item.oldValue : item.newValue);
        }

        _invert = !_invert;
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


