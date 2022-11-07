using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractiveObj : MonoBehaviour
{

    public GameObject obj;

    public TextMeshProUGUI text;

    public TextMeshProUGUI subTittle;

    public Image clock;

    Vector3 _position;

    public static InteractiveObj instance;

    public MonoBehaviour buttonScript;

    Graphic[] fades;

    [SerializeField]
    float timeFadeIn;

    [SerializeField]
    float timeFadeOut;

    private void Start()
    {
        _position = obj.transform.position;

        fades=buttonScript.GetComponentsInChildren<Graphic>();

    }


    public void LoadInfo(string key, float value)
    {
        LoadInfo(key, Vector3.zero, value);
    }

    public void LoadInfo(string key, Vector3 position)
    {
        LoadInfo(key, position, 0);
    }    

   public void LoadInfo(float value)
    {
        
        if (value == 0 && clock.fillAmount != 0)
        {
            buttonScript.enabled = true;
            return;
        }

        buttonScript.enabled = false;
        clock.fillAmount = value;
    }

   public void LoadInfo(string key, Vector3 position, float value)
    {
        LoadInfo(value);
        
        text.text = key;
        _position = position;
        obj.transform.position = _position;
        FadeIn();
    }


    public void CloseInfo()
    {
        FadeOut();
    }


    void FadeIn()
    {

        //obj.SetActive(true);
        foreach (var item in fades)
        {
            item.CrossFadeAlpha(1, timeFadeIn, false);
        }
    }

    void FadeOut()
    {

        //obj.SetActive(false);

        foreach (var item in fades)
        {
            item.CrossFadeAlpha(0, timeFadeOut, false);
        }
    }




    private void Awake()
    {
        instance = this;

    }

}
