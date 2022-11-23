using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase abastracta padre de los poderes y debuffos
/// </summary>
abstract public class FatherPwDbff : MonoBehaviour
{
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

    protected Vector2Int SchPowerObject(string name)
    {
        return PoolObjects.SrchInCategory(NameClassToIndexCategory(), name);
    }

    protected GameObject SpawnPowerObject(Vector2Int axis, Vector3 pos)
    {
        return PoolObjects.SpawnPoolObject(axis, pos, Quaternion.identity);
    }

    protected GameObject SpawnPowerObject(Vector2Int axis, Vector3 pos, Vector3 angle)
    {
        return PoolObjects.SpawnPoolObject(axis, pos, Quaternion.Euler(angle));
    }

    protected GameObject SpawnPowerObject(Vector2Int axis, Vector3 pos, Quaternion angle)
    {
        return PoolObjects.SpawnPoolObject(axis, pos, angle);
    }

    protected GameObject SpawnPowerObject(Vector2Int axis, Vector3 pos, Quaternion angle, Transform parent)
    {
        return PoolObjects.SpawnPoolObject(axis, pos, angle, parent);
    }

    protected GameObject SpawnPowerObject(string name, Vector3 pos)
    {
        return PoolObjects.SpawnPoolObject(NameClassToIndexCategory(), name, pos, Quaternion.identity);
    }

    protected GameObject SpawnPowerObject(string name, Vector3 pos, Vector3 angles)
    {
        return PoolObjects.SpawnPoolObject(NameClassToIndexCategory(), name, pos, Quaternion.Euler(angles));
    }

    protected GameObject SpawnPowerObject(string name, Vector3 pos, Quaternion angles)
    {
        return PoolObjects.SpawnPoolObject(NameClassToIndexCategory(), name, pos, angles);
    }

    protected void SpawnObjRef(Character c ,Vector2Int indexs, Vector3 pos, Vector3 angles)
    {
        GameObject g = SpawnPowerObject(indexs, pos, angles);

        AddObjRef(g.name, g, c);
    }

    protected void SpawnObjRef(Character c,string name, Vector3 pos, Vector3 angles)
    {
        AddObjRef(name, SpawnPowerObject(name, pos, angles), c);
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
