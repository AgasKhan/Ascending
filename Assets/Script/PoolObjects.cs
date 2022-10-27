using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjects : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSpawn
    {
        [System.Serializable]
        public class Pool
        {
            public int index;

            [SerializeField]
            public GameObject[] arrayObjects;

            public Pool(int i)
            {
                arrayObjects = new GameObject[i];
            }
        }

        public string classType;

        public int poolNumber;

        public GameObject[] prefabReference;

        public Pool[] pool;
        
    }

    static public PoolObjects instance;

    public bool eneabled = true;

    [SerializeField]
    ObjectSpawn[] powerObjects;

    static public ObjectSpawn[] pwObj
    {
        get
        {
            return instance.powerObjects;
        }

        set
        {
            instance.powerObjects=value;
        }
    }


    #region busqueda por categoria

    /// <summary>
    /// Devuelve los indices de la categoria y el objeto del pool
    /// </summary>
    /// <param name="type">nombre de la clase/categoria del objeto</param>
    /// <param name="powerObject">nombre del prefab del objeto</param>
    /// <returns></returns>
    static public Vector2Int SrchInCategory(string type, string powerObject)
    {
        return SrchInCategory(SrchInCategory(type), powerObject);   
    }

    /// <summary>
    /// devuelve el indice de la categoria dentro del pool
    /// </summary>
    /// <param name="word">nombre de la clase/categoria del objeto</param>
    /// <returns></returns>
    static public int SrchInCategory(string word)
    {
        for (int i = 0; i < instance.powerObjects.Length; i++)
        {
            if (instance.powerObjects[i].classType == word)
            {
                return i;
            }
        }
        Debug.LogWarning("Error categoria no encontrada: " + word);
        return -1;
    }

    /// <summary>
    /// devuelve el indice de la categoria dentro del pool
    /// </summary>
    /// <param name="index">indice de la categoria</param>
    /// <param name="powerObject">nombre del prefab del objeto</param>
    /// <returns></returns>
    static public Vector2Int SrchInCategory(int index, string powerObject)
    {
        Vector2Int indexsFind = new Vector2Int(index, -1);

        for (int ii = 0; ii < instance.powerObjects[index].prefabReference.Length; ii++)
        {
            if (instance.powerObjects[index].prefabReference[ii].name == powerObject)
            {
                indexsFind.y = ii;
                return indexsFind;
            }
        }
        Debug.LogWarning("No se encontro el objeto: " + powerObject);
        return indexsFind;

    }

    #endregion

    #region "Spawn" pool objects

    static public GameObject SpawnPoolObject(int categoryIndex, string powerObject, Vector3 pos, Quaternion angles)
    {
        Vector2Int indexs = SrchInCategory(categoryIndex, powerObject);

        return SpawnPoolObject(indexs, pos, angles);
    }

    static public GameObject SpawnPoolObject(string type, string powerObject, Vector3 pos, Quaternion angles)
    {
        Vector2Int indexs = SrchInCategory(type, powerObject);

        return SpawnPoolObject(indexs, pos, angles);
    }

    static public GameObject SpawnPoolObject(Vector2Int indexs, Vector3 pos, Quaternion angles, Transform padre = null)
    {

        if (indexs.x < 0)
        {
            Debug.LogWarning("categoria no encontrada");
            return null;
        }
        else if (indexs.y < 0)
        {
            Debug.LogWarning("Objeto no encontrado");
            return null;
        }

        ObjectSpawn.Pool pool = pwObj[indexs.x].pool[indexs.y];

        pool.index++;

        if (pool.index >= pool.arrayObjects.Length)
            pool.index = 0;

        GameObject poolObject = pool.arrayObjects[pool.index];

        poolObject.transform.parent = padre;
        poolObject.transform.localPosition = pos;
        poolObject.transform.localRotation = angles;
        poolObject.SetActive(true);

        return poolObject;
    }

    #endregion

    private void Start()
    {
        instance = this;

        //recorro mi array de categorias de objetos
        for (int i = 0; i < powerObjects.Length; i++)
        {
            //en cada uno creo un array de pools igual a la cantidad de prefabs que estos contienen
            powerObjects[i].pool = new ObjectSpawn.Pool[powerObjects[i].prefabReference.Length];

            //guardo la referencia del array de pool para trabajar mas comodo
            ObjectSpawn.Pool[] pul = powerObjects[i].pool;

            //recorro pool
            for (int j = 0; j < pul.Length; j++)
            {
                //creo un object pool en cada uno, en la constructora defino cuantos elementos va a contener el pool
                pul[j] = new ObjectSpawn.Pool(powerObjects[i].poolNumber);

                //lo recorro para instanciarlos y los apago inmediatamente
                for (int k = 0; k < pul[j].arrayObjects.Length; k++)
                {
                    pul[j].arrayObjects[k] = Instantiate(powerObjects[i].prefabReference[j]);

                    pul[j].arrayObjects[k].name = powerObjects[i].prefabReference[j].name;

                    pul[j].arrayObjects[k].SetActive(false);

                    GameManager.AddTimeController(pul[j].arrayObjects[k].transform);
                }
            }
        }
    
    }
}

/*

static public GameObject SrchPowerObject(string type, string powerObject)
    {
        for (int i = 0; i < instance.powerObjects.Length; i++)
        {
            if(instance.powerObjects[i].classType==type)
            {
                for (int ii = 0; ii < instance.powerObjects[i].GO.Length; ii++)
                {
                    if (instance.powerObjects[i].GO[ii].name == powerObject)
                    {
                        return instance.powerObjects[i].GO[ii];
                    }
                }
            }
        }
        return null;
    }
    

    static public GameObject SpawnPowerObject(string type, string powerObject, Vector3 pos, Quaternion angles)
    {
        GameObject aux = SrchPowerObject(type, powerObject);

        if(aux==null)
        {
            Debug.LogWarning("Objeto no encontrado");
            return null;
        }

        return Instantiate(aux, pos, angles);
    } 

 */