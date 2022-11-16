using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryLevelSelector : MonoBehaviour
{
    MenuManager _refMenuManager;
    private void Start()
    {
        _refMenuManager = MenuManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _refMenuManager.OpenCloseMenu();

        _refMenuManager.ChangeMenu(3);        
    }

}
