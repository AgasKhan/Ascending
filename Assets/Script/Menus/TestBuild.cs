using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuild : MonoBehaviour
{
    public MenuManager myMenuManager;

    private void Start()
    {
        myMenuManager.OpenCloseMenu();
    }
}
