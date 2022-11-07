using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{


    Vector3 previusHealth;

    Player_Character player;

    void Start()
    {
        player = GameManager.player;
    }

    void InfiniteHelth()
    {
        var aux = player.health.GetAll();

        if (aux.AproxMagnitude() >= 1000)
        {
            previusHealth = player.health.GetAll();

            player.health.SetAll(Vector3.one * 1000);
        }
        else
        {
            player.health.SetAll(previusHealth);
        }
    }

    void Interact(float f)
    {
        player.timeInteractMultiply = f;
    }
    void Toxine()
    {
        player.ReplaceFirstPower<Toxine_Powers>();
    }

    void Stun()
    {
        player.ReplaceFirstPower<Stun_Powers>();
    }

    void TP()
    {
        player.ReplaceFirstPower<Teleport_Powers>();
    }

    void Vortex()
    {
        player.ReplaceFirstPower<Vortex_Powers>();
    }

}
