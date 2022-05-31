using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    public string title;
    public AudioClip audioClip;
    public float volume;
    public bool loop;
}

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public List<AudioData> audioDatas = new List<AudioData>();

    private List<AudioSource> audioSources = new List<AudioSource>();

    public void Awake()
    {
        instance = this;
    }

    public static void PlayAudio (string title)
    {
        instance.InitAudio(title);
    }
    public static void StopAudio (string title)
    {
        AudioData audioData = instance.GetAudioData(title);


        if (audioData != null)
        {
            AudioSource audioSource = instance.audioSources.Find(nextAudioSource => nextAudioSource.clip == audioData.audioClip);
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
    public static void StopAllAudio()
    {
        for (int i = 0; i < instance.audioSources.Count; i++)
        {
            instance.audioSources[i].Stop();
        }
    }
    void InitAudio(string title)
    {
        AudioData audioData = GetAudioData(title);
        
        if(audioData != null)
        {
            AudioSource audioSource = GetAudioSource();

            audioSource.clip = audioData.audioClip;
            audioSource.volume = audioData.volume;
            audioSource.loop = audioData.loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("lmao error, no audio found");
        }
    }

    AudioData GetAudioData (string title)
    {
        AudioData audioData = audioDatas.Find(nextAudioData => nextAudioData.title == title);

        return audioData;
    }
    AudioSource GetAudioSource ()
    {
        AudioSource audioSource = audioSources.Find(nextAudioSource => !nextAudioSource.isPlaying);
        if(audioSource != null)
        {
            return audioSource;
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioSource);
            return audioSource;
        }
    }
}
