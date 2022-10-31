using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuild : MonoBehaviour
{

    public SceneChanger sc;


    void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            sc.Load("Level_2");
        }
    }
}
