using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Clase abastracta padre de los poderes y debuffos
/// </summary>
[System.Serializable]
abstract public class FatherPwDbff : IState<Character>
{
    /// <summary>
    /// Funcion que es llamada cada frame del poder/buffo
    /// </summary>
    /// <param name="a">Parametro que recibe de forma automatica que te da al afectado</param>
    public Action<Character> on_Update;

    int _indexSpawnPool = -1;

    /// <summary>
    /// Funcion que es llamada cuando se gana el poder/buffo
    /// </summary>
    /// <param name="a">Parametro que recibe de forma automatica que te da al afectado</param>
    public abstract void On(Character me);

    /// <summary>
    /// Funcion que es llamada cuando se pierde el poder/buffo
    /// </summary>
    /// <param name="a">Parametro que recibe de forma automatica que te da al afectado</param>
    public abstract void Off(Character me);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    public void Update(Character me)
    {
        on_Update(me);
    }

    public FatherPwDbff()
    {
        NameClassToIndexCategory();
    }

    int NameClassToIndexCategory()
    {
        if (_indexSpawnPool > 0)
        {
            return _indexSpawnPool;
        }

        string nameClass = GetType().FullName;

        nameClass = nameClass.Substring(0, nameClass.IndexOf('_'));

        _indexSpawnPool = PoolObjects.SrchInCategory(nameClass);

        return _indexSpawnPool;
    }

    protected Vector2Int SchPowerObject(string name)
    {
        return PoolObjects.SrchInCategory(NameClassToIndexCategory(), name);
    }

    protected UnityEngine.Object SpawnPowerObject(string name, Vector3 pos, Quaternion angle, Transform parent = null)
    {
        return PoolObjects.SpawnPoolObject(PoolObjects.SrchInCategory(NameClassToIndexCategory(), name), pos, angle, parent);
    }

    protected UnityEngine.Object SpawnPowerObject(Vector2Int axis, Vector3 pos, Vector3 angle)
    {
        return PoolObjects.SpawnPoolObject(axis, pos, Quaternion.Euler(angle));
    }

    protected UnityEngine.Object SpawnPowerObject(Vector2Int axis, Vector3 pos)
    {
        return PoolObjects.SpawnPoolObject(axis, pos, Quaternion.identity);
    }

    protected UnityEngine.Object SpawnPowerObject(Vector2Int axis, Vector3 pos, Quaternion angle, Transform parent=null)
    {
        return PoolObjects.SpawnPoolObject(axis, pos, angle, parent);
    }    
}




/*
     /// <summary>
    /// Funcion prototipo para crear otras funciones
    /// </summary>
    /// <param name="a">Parametro que recibe de forma automatica que te da al afectado</param>
    protected delegate void PrototypeFunction(Character me);

    /// <summary>
    /// funcion que se ejecutara todos los frames
    /// Tirara error si no se le pasa por parametro el Affected
    /// </summary>
    protected PrototypeFunction on_Update;

    /// <summary>
    /// Lista de referencias de characters afectados
    /// </summary>
    public List<Character> chrAffected;

    /// <summary>
    /// diccionario que guarda todos los nombres de los objetos creados para desp eliminarlos de forma automatica
    /// </summary>
    public static Dictionary<System.Type, List<string>> keys = new Dictionary<System.Type, List<string>>();

    [SerializeField]
    int _indexSpawnPool=-1;

    int NameClassToIndexCategory()
    {
        if(_indexSpawnPool<0)
        {
            string nameClass = GetType().FullName;

            nameClass = nameClass.Substring(0, nameClass.IndexOf('_'));

            _indexSpawnPool = PoolObjects.SrchInCategory(nameClass);
        }

        return _indexSpawnPool;
    }

    void AddKey(string key)
    {
        if (keys.ContainsKey(this.GetType()))
        {
            if (!keys[GetType()].Contains(key))
                keys[GetType()].Add(key);
        }
        else
            keys.Add(GetType(), new List<string>() { key });

    }

    protected void ClearRefs(Character c)
    {
        if (!keys.ContainsKey(GetType()))
            return;

        foreach (var key in keys[GetType()])
        {
            if (c.MyCooldowns.ContainsKey(key))
                c.RemoveCooldown(key);

            else if (c.MyObjReferences.ContainsKey(key))
                c.RemoveObjRef(key);
        }
    }

    protected void AddCooldown(string id, float Timer, Character c)
    {
        c.AddCooldown(id, Timer);
        AddKey(id);
    }

    protected void AddObjRef(string id, GameObject g, Character c)
    {
        c.AddObjRef(id, g);
        AddKey(id);
    }




    private void Update()
    {
        if (on_Update != null)
            for (int i = 0; i < chrAffected.Count; i++)
            {
                on_Update(chrAffected[i]);
            }
    }

    private void OnDestroy()
    {
        keys.Clear();
    }
}

 */