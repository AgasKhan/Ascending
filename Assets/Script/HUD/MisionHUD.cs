using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionHUD : MonoBehaviour
{
    MiniMisionHUD[] mini;
    Quests.Mission[] missions;

    static MisionHUD instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
     
        mini = GetComponentsInChildren<MiniMisionHUD>();

        missions = Quests.SrchIncomplete(BaseData.currentLevel);

        print("nivel: " +BaseData.currentLevel);

        UpdateMisions();
    }

    public static void UpdateMisions()
    {
        for (int i = 0; i < instance.mini.Length; i++)
        {
            instance.mini[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < instance.missions.Length; i++)
        {
            instance.mini[i].title = instance.missions[i].Description.superior;
            instance.mini[i].text = instance.missions[i].Description.inferior;
            instance.mini[i].gameObject.SetActive(true);
        }
    }
}
