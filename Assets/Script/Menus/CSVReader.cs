using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    public TextAsset textAssetData;

    [System.Serializable]
    public class TestPlayer
    {
        public string name;
        public int health;
        public int damage;
        public int defense;
    }

    [System.Serializable]
    public class TestPlayerList
    {
        public TestPlayer[] player;
    }

    public TestPlayerList myTestPlayerList = new TestPlayerList();

    void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / 4 - 1;

        myTestPlayerList.player = new TestPlayer[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            myTestPlayerList.player[i] = new TestPlayer();

            myTestPlayerList.player[i].name = data[4 * (i + 1)];
            myTestPlayerList.player[i].health = int.Parse(data[4 * (i + 1) + 1]);
            myTestPlayerList.player[i].damage = int.Parse(data[4 * (i + 1) + 2]);
            myTestPlayerList.player[i].defense = int.Parse(data[4 * (i + 1) + 3]);
        }


    }

}

public interface IBDComunication
{
    string ObjToJson();

    void JsonToObj(string Json);
}




