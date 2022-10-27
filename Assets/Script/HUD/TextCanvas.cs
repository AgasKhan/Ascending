using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextCanvas : MonoBehaviour
{
    [System.Serializable]
    public class Messages
    {
        int _id;

        public string name;

        public string[] itemText;

        public bool isActive = false;

        public TextMeshProUGUI textBox;

        [SerializeField]
        float endFadeOut = 1;

        [SerializeField]
        float startFadeIn = 1;

        public void SetTimes(float s, float e)
        {
            startFadeIn = s;
            endFadeOut = e;
        }

        public void StartText()
        {
            isActive = true;

            _id = 0;

            textBox.CrossFadeAlpha(0, 0, false);

            FadeIn();
            LoadText();
        }

        public void StartText(params string[] s)
        {
            if(s!=itemText)
            {
                itemText = s;
                StartText();
            }
            
        }

        public void CloseText()
        {
            if (isActive)
            {
                isActive = false;
                FadeOut();
            }
        }

        public void Next()
        {
            _id++;

            if (_id < itemText.Length)
            {
                LoadText();
            }
            else
            {
                _id = 0;
                CloseText();
            }
        }

        void LoadText()
        {
            textBox.text = itemText[_id];
        }
        void FadeIn()
        {
            if (isActive)
                textBox.CrossFadeAlpha(1.0f, startFadeIn, false);
        }
        void FadeOut()
        {
            if (!isActive)
                textBox.CrossFadeAlpha(0.0f, endFadeOut, false);
        }

        public Messages(TextMeshProUGUI textMeshProUGUI)
        {
            textBox = textMeshProUGUI;
        }

        public Messages(TextMeshProUGUI textMeshProUGUI, params string[] s)
        {
            textBox = textMeshProUGUI;
            StartText(s);
        }

        public Messages(TextMeshProUGUI textMeshProUGUI, float s, float e, params string[] palabra)
        {
            textBox = textMeshProUGUI;
            StartText(palabra);
            SetTimes(s, e);
        }

    }

    public Messages[] text;

    //public Dictionary<string,Messages> text2 = new Dictionary<string, Messages>();

    static TextCanvas instance;

    static public Messages SrchMessages(string name)
    {
        foreach (var item in instance.text)
        {
            print(item.name);
            if (item.name == name)
                return item;
        }

        return null;
    }

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            text[0].Next();
        }
    }

}