using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    static public AudioManager instance;
    
    public Pictionarys<string, AudioLink> audios = new Pictionarys<string, AudioLink>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        foreach (var item in collection)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



[System.Serializable]
public class AudioLink
{
    public AudioClip clip;

    public AudioSource source;
}
