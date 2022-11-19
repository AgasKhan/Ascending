using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Configuration : MonoBehaviour
{
    public AudioMixerGroup group;

    Player_Character player;

    MenuManager menu;

    //
    // Start is called before the first frame update
    void Awake()
    {
        player = GameManager.player;
        menu = MenuManager.instance;

        menu.eventListFloat.AddRange(new Pictionarys<string, System.Action<GameObject, float>>()
        {
            {"sens", CameraSpeed},
            {"Master", ChangeVolumeLevel},
            {"Ambiental", ChangeVolumeLevel},
            {"Effect", ChangeVolumeLevel}
        });

        menu.eventListSliderOn.AddRange(new Pictionarys<string, System.Action<Slider>>()
        {
            {"sens", CameraSpeed},
            {"Master", LoadVolumeLevel},
            {"Ambiental", LoadVolumeLevel},
            {"Effect", LoadVolumeLevel}
        });
    }

    void CameraSpeed(GameObject g,float f)
    {
        player.cameraScript.Speed(f);
    }

    void CameraSpeed(Slider s)
    {
        s.value = player.cameraScript.Speed();
    }

    void ChangeVolumeLevel(GameObject g , float volume)
    {
        ChangeVolume(volume, g.name);
    }

    float LoadVolume(string name)
    {
        float volume;
        group.audioMixer.GetFloat(name, out volume);
        return Mathf.Pow(10, (volume / 20));
    }


    void LoadVolumeLevel(Slider s)
    {
        s.value = LoadVolume(s.name);
    }

    void ChangeVolume(float volume, string name)
    {
        var value = Mathf.Log10(volume) * 20;
        group.audioMixer.SetFloat(name, value);
    }


}
