using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Internal
{
    public class Tags : MonoBehaviour
    {
        static public Tags inst;

        [SerializeField]
        Pictionarys<string, List<GameObject>> librearyTag = new Pictionarys<string, List<GameObject>>();

        #region tags funcitons
        static string[] ToStrings(Tag[] t)
        {
            string[] aux = new string[t.Length];

            for (int i = 0; i < t.Length; i++)
            {
                aux[i] = t[i].ToString();
            }

            return aux;
        }

        static public void AddTags(GameObject g, params Tag[] t)
        {
           
            AddTags(g, ToStrings(t));
                      
        }

        static public void RemoveTags(GameObject g, params Tag[] t)
        {
           
            RemoveTags(g, ToStrings(t));
            
        }

        static public bool ChckOne(GameObject g, params Tag[] t)
        {
            return ChckOne(g, ToStrings(t));
        }

        static public bool ChckAll(GameObject g, params Tag[] t)
        {
            return ChckAll(g, ToStrings(t));
        }

        static public void AddTags(GameObject g, params string[] t)
        {
            foreach (var item in t)
            {
                if (!inst.librearyTag[item].Contains(g))
                    inst.librearyTag[item].Add(g);
            }
        }

        static public void RemoveTags(GameObject g, params string[] t)
        {
            foreach (var item in t)
            {
                if (inst.librearyTag[item].Contains(g))
                    inst.librearyTag[item].Remove(g);
            }
        }

        static public bool ChckOne(GameObject g, params string[] t)
        {
            foreach (var item in t)
            {
                if (inst.librearyTag[item].Contains(g))
                    return true;
            }

            return false;
        }

        static public bool ChckAll(GameObject g, params string[] t)
        {

            foreach (var item in t)
            {
                if (!inst.librearyTag[item].Contains(g))
                    return false;
            }

            return true;
        }

        #endregion

        // Start is called before the first frame update
        void Awake()
        {
            inst = this;

            foreach (var item in Enum.GetValues(typeof(Tag)))
            {
                librearyTag.Add(((Tag)item).ToString(), new List<GameObject>());
            }

            foreach (var item in UnityEditorInternal.InternalEditorUtility.tags)
            {
                if (item != "Untagged")
                    librearyTag.Add(item, new List<GameObject>());
            }

            foreach (var item in FindObjectsOfType(typeof(GameObject)))
            {
                GameObject aux = ((GameObject)item);
                if(!aux.CompareTag("Untagged"))
                    librearyTag[aux.tag].Add(aux);
            }
            
        }

        private void OnDestroy()
        {
            inst = null;
        }
    }

}

