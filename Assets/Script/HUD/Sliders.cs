using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{

    [System.Serializable]
    public class Slid
    {
        public string name;
        public Image slider;

        public Color color
        {
            set
            {
                slider.color = value;
            }

            get
            {
                return slider.color;
            }
        }

        [SerializeField]
        float amount;

        public void Lerp()
        {
            if(amount != slider.fillAmount)
                slider.fillAmount = Mathf.Lerp(slider.fillAmount, amount, Time.deltaTime);
        }

        public void CurrValue(float currValue)
        {
            amount = currValue;
        }



    }

    static public Sliders instance;

    public Slid[] sliders;

    static public Slid SrchSlider(string word)
    {
        foreach (var item in instance.sliders)
        {
            if (word == item.name)
                return item;
        }

        Debug.LogWarning("No se encontro el slider + " + word);

        return null;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        foreach (var item in sliders)
        {
            item.Lerp();
        }
    }


}




