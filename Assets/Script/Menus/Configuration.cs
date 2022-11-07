using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configuration : MonoBehaviour
{
    Player_Character player;

    MenuManager menu;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.player;
        menu = MenuManager.instance;

        menu.eventListFloat.Add("sens", CameraSpeed);

    }

    void CameraSpeed(float f)
    {
        player.cameraScript.Speed(f);
    }
}
