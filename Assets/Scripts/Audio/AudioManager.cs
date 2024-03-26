/*******************************************************************************
 * File Name :         Sound.cs
 * Author(s) :         Clare
 * Creation Date :     3/24/2024
 *
 * Brief Description : 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static UnityEngine.Random;

using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup soundMixer;

    [SerializeField] private Sound[] sounds;
    [SerializeField] private Group[] groups;
    
    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            if (s.parent == null)
                s.source = gameObject.AddComponent<AudioSource>();
            else
                s.source = s.parent.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.pitch = 1f;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;

            s.source.outputAudioMixerGroup = soundMixer;
        }

        foreach (Group g in groups)
        {
            foreach(GroupSound s in g.sounds)
            {
                if (g.parent == null)
                    s.source = gameObject.AddComponent<AudioSource>();
                else
                    s.source = g.parent.AddComponent<AudioSource>();

                s.source.clip = s.clip;
                s.source.volume = g.groupVolume;
                s.source.spatialBlend = g.groupSpatialBlend;
                s.source.outputAudioMixerGroup = soundMixer;
            }
        }
    }


    public AudioClip LoadFromGroup(string name)
    {
        Group g = FindGroup(name);
        return g.sounds[Range(0, g.sounds.Length - 1)].clip;
    }

    public AudioClip LoadClip(string name)
    {
        Sound s = FindSound(name);
        return s.clip;
    }

    public void PlayFromGroup(string name)
    {
        Group g = FindGroup(name);
        g.sounds[Range(0, g.sounds.Length - 1)].source.Play();
    }

    public void PlayAllInGroup(string name)
    {
        Group g = FindGroup(name);
        foreach (GroupSound s in g.sounds)
        {
            s.source.Play();
        }
    }


    public void Play (string name)
    {
        Sound s = FindSound(name);
        s.source.Play();
    }

    public void StopPlaying(string sound)
    {
        Sound s = FindSound(sound);
        s.source.Stop();
    }

    public void Pause (string sound)
    {
        Sound s = FindSound(sound);
        s.source.Pause();
    }

    public void UnPause (string sound)
    {
        Sound s = FindSound(sound);
        s.source.UnPause();
    }

    public void FadeOut(string sound, float FadeTime)
    {
        Sound s = FindSound(sound);
        float startVolume = s.source.volume;

        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / FadeTime;
        }

        if (s.source.volume <= 0)
        {
            s.source.Stop();
            s.source.volume = startVolume;
        }
    }

    private Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }

    private Group FindGroup(string name)
    {
        Group g = Array.Find(groups, group => group.name == name);
        if (g == null)
        {
            Debug.LogWarning("Group: " + name + " not found!");
            return null;
        }
        return g;
    }
}
