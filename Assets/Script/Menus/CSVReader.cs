using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CSVReader : MonoBehaviour, IBDSave, IBDLoad
{
    public TextAsset textAssetData;
    
    public Pictionarys<string, string> myPictionary = new Pictionarys<string, string>();

    public static CSVReader instance;

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

    public string[] dataCSV;

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
        
        ReadCSV();

        SaveInPictionary("Scene_1_Dialogue_4","Test");


        /*
         Guardar partida

        PlayerPrefs.SetString("File", "DDDDDDD");
        print(PlayerPrefs.GetString("File"));
        
         */
    }

    void ReadCSV()
    {

        char[] delimitator = { '\n' };
        dataCSV = ReadString(textAssetData.name + ".csv").Split(delimitator); 
        int tableSize = dataCSV.Length -1 ; 
        
        for (int i = 0; i < tableSize; i++)
        {
            string[] reglonCSV= dataCSV[i].Split(';');

            myPictionary.Add(reglonCSV[0], reglonCSV[1]);
        }

        print(myPictionary);
        
    }

    void WriteCSV()
    {
        textAssetData = new TextAsset ("AAA ; BBB");

        //textAssetData
    }


    void test()
    {
        string json = JsonUtility.ToJson(new Pictionarys<string, float>() {


            {"id1", 1.5f},
            {"id2" , 20.3f}

        });

        print(json);

        Pictionarys<string, float> pic = JsonUtility.FromJson<Pictionarys<string, float>>(json);

        print(pic);
    }

    public string ObjToJson()
    {
        //CSVReader.SaveInPictionary<GameObject>()
        
        return null;
    }

    public void JsonToObj (string json)
    {
        //CSVReader.LoadFromPictionary<GameObject>(json);
    }

    #region SaveAndLoadJson

    /// <summary>
    /// Convierte un objeto en un json y lo almacena o reemplaza en el id que le pases
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public static void SaveInPictionary <T> (string id, T data) where T : MonoBehaviour
    {
        string json = JsonUtility.ToJson(data);

        if (BD.ContainsKey(id))
            BD[id] = json;
        else
            BD.Add(id, json);
    }


    public static void SaveInPictionary (string id, object data)
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


    static void WriteString(string file, string data)
    {
        string path = "Assets/Resources/" + file;
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(data);
        writer.Close();

    }

    static string ReadString(string file)
    {
        string aux;

        string path = "Assets/Resources/" + file;
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        aux = (reader.ReadToEnd());
        reader.Close();

        return aux;
    }

    private void OnDestroy()
    {
        WriteString(textAssetData.name + ".csv" , myPictionary.ToString(";"));
    }


}

public interface IBDSave
{
    void JsonToObj(string Json);
}

public interface IBDLoad
{
    string ObjToJson();
}
