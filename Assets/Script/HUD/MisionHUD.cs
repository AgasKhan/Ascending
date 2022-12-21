using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionHUD : MonoBehaviour
{
    MiniMisionHUD[] mini;
    List<Quests.Mission> missions = new List<Quests.Mission>();

    static MisionHUD instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
     
        mini = GetComponentsInChildren<MiniMisionHUD>();

        missions.AddRange(Quests.SrchIncomplete(BaseData.currentLevel));

        UpdateMisions();
    }

    public static void UpdateMisions()
    {
        for (int i = 0; i < instance.mini.Length; i++)
        {
            instance.mini[i].gameObject.SetActive(false);
        }

        for (int i = instance.missions.Count -1 ; i >=0 ; i--)
        {
            instance.mini[i].title = instance.missions[i].Description.superior;
            instance.mini[i].text = instance.missions[i].Description.inferior;
            instance.mini[i].gameObject.SetActive(instance.missions[i].active);
        }
    }
}
