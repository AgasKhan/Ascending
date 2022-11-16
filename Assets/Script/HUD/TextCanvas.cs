using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Internal;

public class TextCanvas : MonoBehaviour
{
    public TextManager[] text;

    //public Dictionary<string,Messages> text2 = new Dictionary<string, Messages>();

    static TextCanvas instance;

    static public TextManager SrchMessages(string name)
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

        foreach (var item in text)
        {
            item.Start();
        }
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            //ShowText(bool setLoad, params string[] s)

            SrchMessages("Lucas").ShowText(false,"MISION " + " NO ".RichText("b").RichText("color", "red") + "FRACASADA");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SrchMessages("display").ShowText(false, "holi", "chau");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SrchMessages("display").AddNextText("Agregado","agregado 2");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SrchMessages("display").AddActualText("Sumado");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SrchMessages("display").Message("Mensaje");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            text[0].Next();
        }

        foreach (var item in instance.text)
        {
            if (item.Load() && item.timeInScreen.Chck())
                item.Next();

            item.Fade();
        }
    }

}

namespace Internal
{

    [System.Serializable]
    public class TextManager
    {
        public string name;//un id

        [SerializeField]
        int _id; //para recorrer el array

        public List<string> itemText;//array de texto

        public bool isActive = false;

        public TextMeshProUGUI textBox;//referencia del objeto a manipular

        [SerializeReference]
        public Timer timeInScreen;

        [SerializeReference]
        public Timer letters;

        public Func<bool> Load;

        [SerializeField]
        float inScreen;

        [SerializeField]
        float btwLetters;


        [SerializeField]
        float endFadeOut = 1;

        [SerializeField]
        float startFadeIn = 1;

        [SerializeField]
        [TextArea(3, 6)]
        string final;

        bool setLoad;

        float alpha;
        float timeAlpha;


        public void AddNextText(params string[] s)
        {
            itemText.AddRange(s);
        }

        public void AddActualText(string s)
        {
            itemText[_id] += s;

            SetText();
        }

        public void Message(string s, bool setLoad = true)
        {
            if (isActive)
                AddActualText("\n" + s);
            else
                ShowText(true, s);
        }

        /// <summary>
        /// Muestra el ultimo texto en pantalla (de 0)
        /// </summary>
        public void ShowText()
        {
            isActive = true;

            textBox.CrossFadeAlpha(1, 0, false);

            //textBox.alpha = 0;

            if (setLoad)
                Load = Write;
            else
                Load = WriteInst;

            timeInScreen.Reset();
            letters.Reset();
            _id = 0;
            SetText();

            FadeIn();
        }

        public void ShowText(bool setLoad)
        {
            this.setLoad = setLoad;
            ShowText();
        }

        /// <summary>
        /// Carga el texto y lo muestra
        /// </summary>
        /// <param name="s"></param>
        public void ShowText(bool setLoad, params string[] s)
        {
            //if(s!=itemText.ToArray())
            {
                if (s[0] != textBox.text)
                    textBox.text = "";

                itemText.Clear();
                itemText.AddRange(s);
                ShowText(setLoad);
            }

        }

        /// <summary>
        /// Oculta el texto a traves de un fadeout
        /// </summary>
        public void HideText()
        {
            if (isActive)
            {
                isActive = false;
                FadeOut();
            }
        }

        /// <summary>
        /// va al siguiente indice del array
        /// </summary>
        public void Next()
        {
            _id++;

            if (_id >= itemText.Count)
            {
                _id = 0;
                HideText();
            }
            else
            {
                textBox.text = "";
                SetText();
            }

        }

        public void SetText()
        {
            final = itemText[_id];
        }

        bool WriteInst()
        {
            if (!isActive)
                return false;

            if (textBox.text != final)
            {
                textBox.text = final;
                timeInScreen.Reset();

            }

            return true;
        }

        bool Write()
        {
            if (!isActive)
                return false;

            if (textBox.isTextOverflowing)
            {
                final = final.Substring(final.IndexOf("\n") + 1);
                textBox.text = textBox.text.Substring(textBox.text.IndexOf("\n") + 1);
            }

            if (textBox.text == final && final != "")
            {
                final = "";
            }
            else if (final != "")
            {
                if (letters.Chck())
                {
                    textBox.text += final[textBox.text.Length];

                    //AudioManager.instance.Play("tec"+Random.Range(1,4));

                    letters.Reset();
                    timeInScreen.Reset();
                }
                return false;
            }
            return true;
        }
        void FadeIn()
        {
            if (isActive)
                //textBox.CrossFadeAlpha(1.0f, startFadeIn, false);
                Fade(startFadeIn, 1);
        }
        void FadeOut()
        {
            if (!isActive)
                //textBox.CrossFadeAlpha(0.0f, endFadeOut, false);
                Fade(endFadeOut, 0);

        }

        void Fade(float t, float a)
        {
            alpha = a;
            timeAlpha = t;
        }

        public void Fade()
        {
            var a = alpha;
            var t = Time.deltaTime / timeAlpha;

            if ((textBox.color.a - a) != 0)
            {
                if (Mathf.Abs(textBox.color.a - a) * 100 < 1)
                    textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, a);
                else if (Mathf.Abs(textBox.color.a - a) * 100 < 15)
                    t *= 4;
                else if (Mathf.Abs(textBox.color.a - a) * 100 < 30)
                    t *= 2;
            }

            if (textBox.color.a != a)
            {
                textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, Mathf.Lerp(textBox.color.a, a, t));
            }
            else if (a == 0)
            {
                textBox.text = "";
                final = "";
            }
        }

        void SetTimes(float s, float e)
        {
            startFadeIn = s;
            endFadeOut = e;
        }


        public void Start()
        {
            timeInScreen = Timers.Create(inScreen + startFadeIn);
            letters = Timers.Create(btwLetters);
            Load = Write;
            textBox.text = "";
            itemText = new List<string>();
        }
    }

}