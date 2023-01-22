using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjects : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPools
    {
        [System.Serializable]
        public class Pool
        {
            [Header("Configuracion")]

            public GameObject prefabReference;

            public MonoBehaviour script;

            public int poolNumber;

            [Header("Parte interna")]

            public int index;

            [SerializeField]
            public Object[] arrayObjects;

            public Pool(int i)
            {
                arrayObjects = new Object[i];
            }
        }

        public string classType;

        public Pool[] pool;
        
    }

    static public PoolObjects instance;

    public bool eneabled = true;

    [SerializeField]
    ObjectPools[] powerObjects;

    static public ObjectPools[] pwObj
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

        for (int ii = 0; ii < instance.powerObjects[index].pool.Length; ii++)
        {
            if (instance.powerObjects[index].pool[ii].prefabReference.name == powerObject)
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

    static public Object SpawnPoolObject(int categoryIndex, string powerObject, Vector3 pos, Quaternion angles)
    {
        Vector2Int indexs = SrchInCategory(categoryIndex, powerObject);

        return SpawnPoolObject(indexs, pos, angles);
    }

    static public Object SpawnPoolObject(string type, string powerObject, Vector3 pos, Quaternion angles)
    {
        Vector2Int indexs = SrchInCategory(type, powerObject);

        return SpawnPoolObject(indexs, pos, angles);
    }

    static public Object SpawnPoolObject(Vector2Int indexs, Vector3 pos, Quaternion angles, Transform padre = null)
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

        ObjectPools.Pool pool = pwObj[indexs.x].pool[indexs.y];

        pool.index++;

        if (pool.index >= pool.arrayObjects.Length)
            pool.index = 0;

        Transform transformObject;

        if (pool.script != null)
        {
            MonoBehaviour poolObject = (MonoBehaviour)pool.arrayObjects[pool.index];
            transformObject = poolObject.transform;
        }
        else
        {
            GameObject poolObject = (GameObject)pool.arrayObjects[pool.index];
            transformObject = poolObject.transform;
        }

        transformObject.parent = padre;
        transformObject.localPosition = pos;
        transformObject.localRotation = angles;
        transformObject.gameObject.SetActive(true);

        return pool.arrayObjects[pool.index];
    }

    #endregion

    private void Start()
    {
        instance = this;

        //recorro mi array de categorias de objetos
        for (int i = 0; i < powerObjects.Length; i++)
        {
            //guardo la referencia del array de pool para trabajar mas comodo
            ObjectPools.Pool[] pul = powerObjects[i].pool;

            //recorro pool
            for (int j = 0; j < pul.Length; j++)
            {
                //creo un object pool en cada uno, en la constructora defino cuantos elementos va a contener el pool
                pul[j].arrayObjects = new Object[pul[j].poolNumber];

                //lo recorro para instanciarlos y los apago inmediatamente
                for (int k = 0; k < pul[j].arrayObjects.Length; k++)
                {
                    if(pul[j].script!=null)
                    {
                        pul[j].arrayObjects[k] = (MonoBehaviour)Instantiate(pul[j].prefabReference).GetComponent(pul[j].script.GetType());

                        pul[j].arrayObjects[k].name = pul[j].prefabReference.name;

                        ((MonoBehaviour)pul[j].arrayObjects[k]).gameObject.SetActive(false);

                        GameManager.AddTimeController(((MonoBehaviour)pul[j].arrayObjects[k]).transform);
                    }
                    else
                    {
                        pul[j].arrayObjects[k] = Instantiate(pul[j].prefabReference);

                        pul[j].arrayObjects[k].name = pul[j].prefabReference.name;

                        ((GameObject)pul[j].arrayObjects[k]).SetActive(false);

                        GameManager.AddTimeController(((GameObject)pul[j].arrayObjects[k]).transform);
                    }
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