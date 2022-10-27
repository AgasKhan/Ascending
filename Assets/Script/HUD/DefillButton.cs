using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefillButton : MonoBehaviour
{
    public Image img;
    public float amount = 0.01f;
    private void FixedUpdate()
    {
        if (img.fillAmount <= 0)
            this.enabled = false;

         img.fillAmount -= amount;
        
    }
}
