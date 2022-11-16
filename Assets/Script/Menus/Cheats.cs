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

        menu.eventListVoid.AddRange( new Pictionarys<string, System.Action<GameObject>>()
        {

            {"infinite", InfiniteHelth},
            {"toxine", Toxine},
            {"stun", Stun},
            {"vortex", Vortex},
            {"atracction", UnlockAtracction},
            {"teleport", Teleport}

        });

        menu.eventListFloat.Add("interact", Interact);
    }

    void InfiniteHelth(GameObject g)
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

        ChangeText(g);
    }

    void Interact(GameObject g, float f)
    {
        player.timeInteractMultiply = f;
    }

    void UnlockAtracction(GameObject g)
    {
        player.UnlockAtrackt = !player.UnlockAtrackt;
        ChangeText(g, player.UnlockAtrackt);
    }

    void Toxine(GameObject g)
    {
 
        player.ReplaceFirstPower<Toxine_Powers>();
    }

    void Stun(GameObject g)
    {

        player.ReplaceFirstPower<Stun_Powers>();
    }

    void Vortex(GameObject g)
    {

        player.ReplaceFirstPower<Vortex_Powers>();
    }

    void Teleport(GameObject g)
    {

        player.ReplaceFirstPower<Teleport_Powers>();
    }

    void ChangeText(GameObject g)
    {
        TMPro.TextMeshProUGUI text = g.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        text.text = (text.text == "Activate") ? "Deactivate" : "Activate";

    }

    void ChangeText(GameObject g, bool active)
    {
        TMPro.TextMeshProUGUI text = g.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        text.text = active ? "Deactivate" : "Activate";

    }


}
