using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Internal;

public static class Extensions
{

    #region Tags

    static public void AddTags(this GameObject g, params Tag[] t)
    {
        Tags.AddTags(g, t);
    }

    static public void RemoveTags(this GameObject g, params Tag[] t)
    {
        Tags.RemoveTags(g, t);
    }

    static public bool CompareTags(this GameObject g, params Tag[] t)
    {
        return Tags.ChckAll(g, t);
    }

    static public bool CompareOneTags(this GameObject g, params Tag[] t)
    {
        return Tags.ChckOne(g, t);
    }

    static public GameObject[] FindWithTags(this GameObject g, params Tag[] t)
    {
        return Tags.Find(t);
    }


    static public void AddTags(this GameObject g, params string[] t)
    {
        Tags.AddTags(g, t);
    }

    static public void RemoveTags(this GameObject g, params string[] t)
    {
        Tags.RemoveTags(g, t);
    }

    static public bool CompareTags(this GameObject g, params string[] t)
    {
        return Tags.ChckAll(g, t);
    }

    static public bool CompareOneTags(this GameObject g, params string[] t)
    {
        return Tags.ChckOne(g, t);
    }

    static public GameObject[] FindWithTags(this GameObject g, params string[] t)
    {
        return Tags.Find(t);
    }

    #endregion

    #region Vectors

    /// <summary>
    /// devuelve una magnitud aproximada del vector donde el valor de este es determinado por la mayor de todas sus proyecciones
    /// muy util para el input del movimiento
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    static public float AproxMagnitude(this Vector2 v)
    {
        return (v.x > v.y ? v.x : v.y);
    }

    /// <summary>
    /// devuelve una magnitud aproximada del vector donde el valor de este es determinado por la mayor de todas sus proyecciones
    /// muy util para el input del movimiento
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    static public float AproxMagnitude(this Vector3 v)
    {
        return (AproxMagnitude(v.Vect3To2()) > v.z ? AproxMagnitude(v.Vect3To2()) : v.z);
    }

    /// <summary>
    /// Quita el parametro z del vector 3 y devuelve un vector 2
    /// </summary>
    /// <param name="v">Vector que modifica</param>
    /// <returns></returns>
    static public Vector2 Vect3To2(this Vector3 v)
    {
        return new Vector2(v.x,v.y);
    }
    #endregion

    #region string

    static public string RichText(this string s, string tag, string valor)
    {
        return "<"+tag+"="+valor+">"+s+"</"+tag+">" ;
    }

    #endregion
}
