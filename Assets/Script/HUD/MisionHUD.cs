using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionHUD : MonoBehaviour
{

    MiniMisionHUD[] mini;

    // Start is called before the first frame update
    void Start()
    {
        mini = GetComponentsInChildren<MiniMisionHUD>();

        Quests.Mission[] missions = Quests.SrchIncomplete(1);

        for (int i = 0; i < mini.Length; i++)
        {
            mini[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < missions.Length; i++)
        {
            mini[i].title = missions[i].Description.superior;
            mini[i].text = missions[i].Description.inferior;
            mini[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
