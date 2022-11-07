using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configuration : MonoBehaviour
{
    Player_Character player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.player;

    }

    void CameraSpeed(float f)
    {
        player.cameraScript.Speed(f);
    }
}
