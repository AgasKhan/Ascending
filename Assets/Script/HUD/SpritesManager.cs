using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritesManager : MonoBehaviour
{
    static public SpritesManager instance;

    public GameObject geografia;

    public Image[] siguiente = new Image[3];
    public Image[] actual = new Image[3];
    public Image[] anterior = new Image[3];

    /*
    //Para cuando tengamos mas de un poder
    static public void RefreshUI()
    {
        int countPower = GameManager.player.power.Count;

        int id = GameManager.player.actualPower;

        Load(instance.actual, GameManager.player.power[id].art);

        if (id == 0)
        {
            Load(instance.siguiente, GameManager.player.power[id + 1].art);
            Load(instance.anterior, GameManager.player.power[countPower - 1].art);
        }
        else if (id == countPower - 1)
        {
            Load(instance.siguiente, GameManager.player.power[0].art);
            Load(instance.anterior, GameManager.player.power[id - 1].art);
        }
        else
        {
            Load(instance.siguiente, GameManager.player.power[id + 1].art);
            Load(instance.anterior, GameManager.player.power[id - 1].art);
        }

    }
    */

    //para un solo poder
    static public void RefreshUI()
    {
        int countPower = GameManager.player.power.Count;

        int id = GameManager.player.actualPower;

        Load(instance.actual, GameManager.player.power[id].art);

        instance.geografia.SetActive(true);

    }

    static public void Load(Image[] arr, Powers_FatherPwDbff.Sprites sprites)
    {
        for (int i = 0; i < 3; i++)
        {
            arr[i].sprite = sprites.spriteImage[i];
        }

    }

    private void Awake()
    {
        instance = this;
    }
}