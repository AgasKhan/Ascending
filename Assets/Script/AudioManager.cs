using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{    
    public Pictionarys<string, AudioLink> audios = new Pictionarys<string, AudioLink>();

    public void Play(string name)
    {
        audios[name].source.Play();
    }

    public void Stop(string name)
    {
        audios[name].source.Stop();
    }

    public void Pause(string name)
    {
        audios[name].source.Pause();
    }

    // Start is called before the first frame update
    void Awake()
    { 
        foreach (var item in audios)
        {
            item.value.source = gameObject.AddComponent<AudioSource>();
            item.value.source.outputAudioMixerGroup = item.value.mixer;
            item.value.source.clip = item.value.clip;
            item.value.source.volume = item.value.volume;
            item.value.source.pitch = item.value.pitch;
            item.value.source.loop = item.value.loop;
            item.value.source.playOnAwake = item.value.onAwake;
        }
    }
}


[System.Serializable]
public struct AudioLink
{
    public AudioMixerGroup mixer;
    public AudioClip clip;

    [Range(0,1)]
    public float volume;
    [Range(-1, 3)]
    public float pitch;
    public bool loop;
    public bool onAwake;

    [HideInInspector]
    public AudioSource source;
}
