using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Internal;

[System.Serializable]
public class Pictionarys<K, V> : IEnumerable<Pictionary<K, V>>
{
    [SerializeField]
    List<Pictionary<K, V>> pictionaries;

    public float count
    {
        get;
        private set;
    }

    /// <summary>
    /// busca por el orden de la lista
    /// </summary>
    /// <param name="k"></param>
    /// <returns></returns>
    public V this[int k]
    {
        get
        {
            return pictionaries[k].value;
        }

        set
        {
            pictionaries[k].value = value;
        }
    }

    /// <summary>
    /// busca por el nombre del enum, si coincide con el nombre del key
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public V this[Enum e]
    {
        get
        {
            return pictionaries[StringIndex(e)].value;
        }

        set
        {
            pictionaries[StringIndex(e)].value = value;
        }
    }

    /// <summary>
    /// Busca por el key ingresado
    /// </summary>
    /// <param name="k"></param>
    /// <returns></returns>
    public V this[K k]
    {
        get
        {
            return pictionaries[SearchIndex(k)].value;
        }

        set
        {
            pictionaries[SearchIndex(k)].value = value;
        }
    }

    public IEnumerator<Pictionary<K, V>> GetEnumerator()
    {
        return pictionaries.GetEnumerator();
    }


    /// <summary>
    /// devuelve el index en caso de encontrar similitud con el nombre del key
    /// </summary>
    /// <param name="s"></param>
    /// <returns>devuelve la poscion en un entero</returns>
    public int StringIndex(Enum s)
    {
        return StringIndex(s.ToString());
    }

    /// <summary>
    /// devuelve el index en caso de encontrar similitud con el nombre del key
    /// </summary>
    /// <param name="s"></param>
    /// <returns>devuelve la poscion en un entero</returns>
    public int StringIndex(string s)
    {
        for (int i = 0; i < pictionaries.Count; i++)
        {
            if (pictionaries[i].key.ToString() == s)
            {
                return i;
            }
        }

        return -1;
    }

    public bool ContainsKey(K key)
    {
        for (int i = 0; i < pictionaries.Count; i++)
        {
            if (pictionaries[i].key.Equals(key))
            {
                return true;
            }
        }

        return false;
    }

    public void AddRange(Pictionary<K, V>[] pic)
    {
        pictionaries.AddRange(pic);
        count += pic.Length;
    }

    public void AddRange(List<Pictionary<K, V>> pic)
    {
        pictionaries.AddRange(pic);
        count += pic.Count;
    }

    public void AddRange(Pictionarys<K, V> pic)
    {
        pictionaries.AddRange(pic);
        count += pic.count;
    }

    public void Add(K key, V value)
    {
        if (ContainsKey(key))
            return;

        pictionaries.Add(new Pictionary<K, V>(key, value));
        count++;
    }

    public void Remove(K key)
    {
        for (int i = 0; i < pictionaries.Count; i++)
        {
            if (pictionaries[i].key.Equals(key))
            {
                pictionaries.RemoveAt(i);
                count--;
                return;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    int SearchIndex(K key)
    {
        for (int i = 0; i < pictionaries.Count; i++)
        {
            if (pictionaries[i].key.Equals(key))
            {
                return i;
            }
        }

        return -1;
    }

    public Pictionarys()
    {
        pictionaries = new List<Pictionary<K, V>>();
    }
}



namespace Internal
{ 

    [System.Serializable]
    public class Pictionary<K, V>
    {
        public K key;
        public V value;

        public Pictionary() { }
        public Pictionary(K k, V v)
        {
            key = k;
            value = v;
        }

    }

}