using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    AudioLink[] audios;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



[System.Serializable]
class AudioLink
{
    AudioClip clip;
    AudioSource source;

}
