using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CSVReader : MonoBehaviour
{
    /// <summary>
    /// El archivo CSV que se manejara
    /// </summary>
    public TextAsset textAssetData;
    
    public Pictionarys<string, string> myPictionary = new Pictionarys<string, string>();

    public static CSVReader instance;

    public int maximumGames;

    public static Pictionarys<string, string> BD
    {
        get
        {
            return instance.myPictionary;
        }
        set
        {
            instance.myPictionary = value;
        }
    }

    

    //[System.Serializable]
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

    void Awake()
    {
        instance = this;
        
        ReadCSV(textAssetData);

        SaveInPictionary("Scene_1_Dialogue_1","TestNumberOne");
        SaveInPictionary("Scene_1_Dialogue_2", "TestNumberTwo");
        SaveInPictionary("Scene_1_Dialogue_3", "TestNumberThree");
        SaveInPictionary("Scene_1_Dialogue_4", "TestNumberFour");

        WriteCSV(textAssetData, myPictionary);


        for (int i = 0; i < maximumGames; i++)
        {
            PlayerPrefs.SetString("GameSlot_" + i.ToString() , "");
        }

        SaveProgress(0, myPictionary);

        print(LoadProgress(0));

    }

    void SaveProgress(int indexSlot, Pictionarys<string, string> myChanges)
    {
        if (indexSlot >= 0 && indexSlot < maximumGames)
            PlayerPrefs.SetString("GameSlot_" + indexSlot, myChanges.ToString());
        else
            print("There is not a game slot whit that index");
    }

    string LoadProgress(int indexSlot)
    {
        if (indexSlot >= 0 && indexSlot < maximumGames)
            return PlayerPrefs.GetString("GameSlot_" + indexSlot);
        else
        {
            print("There is not a game slot whit that index");
            return null;
        }
    }


    #region TextManagment
    void ReadCSV(TextAsset myCSVFile)
    {
        string[] dataCSV;

        dataCSV = ReadString(myCSVFile.name + ".csv").Split('\n');
      
        for (int i = 0 ; i < dataCSV.Length - 1 ; i++)
        {
            string[] reglonCSV= dataCSV[i].Split(';');

            myPictionary.Add(reglonCSV[0], reglonCSV[1]);
        }

    }

    void WriteCSV(TextAsset myCSVFile, Pictionarys<string, string> myChanges)
    {
        WriteString(myCSVFile.name + ".csv", myChanges.ToString(";"));
    }

    static void WriteString(string file, string data)
    {
        string path = "Assets/Resources/" + file;

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(data);
        writer.Close();
    }

    static string ReadString(string file)
    {
        string aux;
        string path = "Assets/Resources/" + file;

        StreamReader reader = new StreamReader(path);
        aux = (reader.ReadToEnd());
        reader.Close();

        return aux;
    }
    #endregion

    #region SaveAndLoadJson

    /// <summary>
    /// Convierte un objeto en un json y lo almacena o reemplaza en el id que le pases
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public static void SaveInPictionary<T>(string id, T data) where T : MonoBehaviour
    {
        string json = JsonUtility.ToJson(data);

        if (BD.ContainsKey(id))
            BD[id] = json;
        else
            BD.Add(id, json);
    }


    public static void SaveInPictionary(string id, object data)
    {
        string json = data.ToString();

        if (BD.ContainsKey(id))
            BD[id] = json;
        else
            BD.Add(id, json);
    }

    /// <summary>
    /// Busca un json dentro del pictionary a partir de un id
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public static T LoadFromPictionary<T>(string id)
    {

        if (BD.ContainsKey(id))
        {
            var obj = JsonUtility.FromJson<T>(BD[id]);
            return obj;
        }
        else
        {
            Debug.Log("Not id found on pictionary");
            return default;
        }
    }

    #endregion

}

public interface IBDSave
{
    void JsonToObj(string Json);
}

public interface IBDLoad
{
    string ObjToJson();
}
