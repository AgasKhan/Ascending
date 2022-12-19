
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Quests
{

    static public List<Mission> incomplete = new List<Mission>();
    static public List<Mission> complete = new List<Mission>();

    static public void Update()
    {
        foreach (var item in incomplete)
        {
            item.Chck();
        }
    }

    static public Mission[] SrchComplete(int level)
    {
        return Srch(level, complete);
    }

    static public Mission[] SrchIncomplete(int level)
    {
        return Srch(level, incomplete);
    }

    static Mission[] Srch(int level, List<Mission> list)
    {
        List<Mission> missions = new List<Mission>();

        foreach (var item in list)
        {
            if (item.level == level)
                missions.Add(item);
        }

        return missions.ToArray();
    }

    // Desarrollar misiones ejecuten un update y hagan check al final de los niveles

    [System.Serializable]
    public class Mission
    {
        public int level;
        public bool active;
        public DoubleString Description;
        public System.Func<bool> chck;
        public System.Action reward;
        public bool update;

        public void Reward()
        {
            if (active)
            {
                reward();

                complete.Add(this);

                incomplete.Remove(this);
            }
        }

        public void Chck()
        {
            if(active && update &&chck())
            {
                active = false;

                Debug.Log("Perdiste");
            }
        }

        public Mission(int level, string sup, string inf, Func<bool> chck, Action reward, bool update = true)
        {
            this.level = level;
            Description = new DoubleString(sup, inf);
            this.chck = chck;
            this.reward = reward;
            this.update = update;

            incomplete.Add(this);
        }
    }

    
}


