using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

    public Pictionarys()
    {
        pictionaries = new List<Pictionary<K, V>>();
    }
}




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

