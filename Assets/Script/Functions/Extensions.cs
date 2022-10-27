using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Internal;

public static class Extensions
{
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



}
