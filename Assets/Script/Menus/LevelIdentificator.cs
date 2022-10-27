using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIdentificator : MonoBehaviour
{ 
   
    public string level;
    public MenuManager refMenuManager;

    public void ChargeLevel()
    {
        if(refMenuManager != null)
            refMenuManager.SelectLevel(level);
    }
}
