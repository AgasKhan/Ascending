using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    Vector3 previusHealth;

    Player_Character player;

    MenuManager menu;

    void Start()
    {
        player = GameManager.player;

        menu = MenuManager.instance;

        menu.eventListVoid.AddRange( new Pictionarys<string, System.Action>()
        {

            {"infinite", InfiniteHelth},
            {"toxine", player.ReplaceFirstPower<Toxine_Powers>},
            {"stun", player.ReplaceFirstPower<Stun_Powers>},
            {"vortex", player.ReplaceFirstPower<Vortex_Powers>},
            {"teleport", player.ReplaceFirstPower<Teleport_Powers>}

        });

        menu.eventListFloat.Add("interact", Interact);
    }

    void InfiniteHelth()
    {
        var aux = player.health.GetAll();

        if (aux.AproxMagnitude() < 1000)
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

   
}
