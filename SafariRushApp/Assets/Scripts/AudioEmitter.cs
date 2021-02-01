using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioEmitter : MonoBehaviour
{
    AudioSource source;
    public List<MyAudio> audios;
    public Dictionary<string, AudioClip> audioDictionary;
    [Range(0,100)]
    public int volume;
    public int priority;
    string lastPlayed;

    // Start is called before the first frame update
    void Awake()
    {
        audioDictionary = new Dictionary<string, AudioClip>();
        source = GetComponent<AudioSource>();
        AdjustVolume((int)GameManager.Volume*100);
        GameManager.OnVolumeChange.AddListener(() => AdjustVolume((int)(GameManager.Volume * 100)));
        LoadAudios(audios);
    }

    public void LoadAudios(List<MyAudio> audioList)
    {
        foreach(MyAudio a in audioList)
        {
            LoadAudio(a);
        }
    }

    public void LoadAudio(MyAudio audio)
    {
        LoadAudio(audio.name, audio.audio);
    }

    public void LoadAudio(string name, AudioClip audio)
    {
        if(!audioDictionary.ContainsKey(name))
        {
            audioDictionary.Add(name, audio);
        }
        else
        {
            Debug.Log("Audio Already Exist");
        }
    }

    public void MakeSound(string sound)
    {
        source.PlayOneShot(audioDictionary[sound]);
    }

    public void MakeSound(string sound, int volume)
    {
        source.PlayOneShot(audioDictionary[sound], (float)volume/100);
    }

    public void PlayBackground()
    {
        if(source.clip != null)
        {
            source.loop = true;
            source.Play();
        }
    }

    public void PlayNewBackground(string sound)
    {
        if(audioDictionary.ContainsKey(sound))
        {
            source.clip = audioDictionary[sound];
            source.loop = true;
            PlayBackground();
        }
    }

    public void StopBackground()
    {
        source.Stop();
        source.loop = false;
    }

    public void ChangeBackgroud(string sound)
    {
        if (audioDictionary.ContainsKey(sound))
        {
            source.clip = audioDictionary[sound];
        }
    }

    public void AdjustVolume(int vol)
    {
        volume = vol;
        source.volume = (float)volume / 100;
    }

    public void Shout(string name)
    {
        source.volume = 1;
        MakeSound(name);
        source.volume = volume / 100;
    }
}

[System.Serializable]
public class MyAudio
{
    public AudioClip audio;
    public string name;
}
