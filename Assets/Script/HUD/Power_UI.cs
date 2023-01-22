using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_UI : MonoBehaviour
{
    [System.Serializable]
    public struct UI
    {
        public Sprite GeneralIcon;
        public Sprite ActiveIcon;

        [TextArea(3, 6)]
        public string generalText;

        [TextArea(3, 6)]
        public string activeText;
    }

    [SerializeField]
    Pictionarys<string, UI> _uis = new Pictionarys<string, UI>();

    static Power_UI instance;

    public static Pictionarys<string, UI> uis
    {
        set
        {
            instance._uis = value;
        }

        get
        {
            return instance._uis;
        }
    }

    private void Awake()
    {
        instance = this;
    }
}
