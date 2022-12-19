using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniMisionHUD : MonoBehaviour
{
    public string title
    {
        set
        {
            _title.text = value;
        }
    }

    public string text
    {
        set
        {
            _text.text = value;
        }
    }

    [SerializeField]
    TextMeshProUGUI _title;
    [SerializeField]
    TextMeshProUGUI _text;
}
