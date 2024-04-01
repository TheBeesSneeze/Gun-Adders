/*******************************************************************************
 * File Name :         AudioManager.cs
 * Author(s) :         Claire
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
    public static AudioMixer mainAudioMixer; // Change to AudioMixer

    [SerializeField] private Sound[] sounds;
    [SerializeField] private Group[] groups;

    public static AudioManager instance;
    [HideInInspector] public float defaultMixerVolume;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        mainAudioMixer = Resources.Load<AudioMixer>("Audio/Master"); // Load the main AudioMixer
        if (mainAudioMixer == null)
        {
            Debug.LogError("Main AudioMixer not found. Please ensure it's placed in Resources/Audio.");
            return;
        }

        foreach (Sound s in sounds)
        {
            SetupSound(s, gameObject);
        }

        foreach (Group g in groups)
        {
            foreach (GroupSound s in g.sounds)
            {
                SetupGroupSound(s, g, g.parent == null ? gameObject : g.parent);
            }
        }

        mainAudioMixer.GetFloat("Master", out defaultMixerVolume);
    }

    // This setups individual sounds
    private void SetupSound(Sound s, GameObject targetGameObject)
    {
        s.source = targetGameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        // Set other AudioSource properties based on Sound properties
        s.source.volume = s.volume;
        s.source.loop = s.loop;
        s.source.spatialBlend = s.spatialBlend;
        // Assign to a default mixer group, if necessary
        AssignGroupToAudioSource(s.source, "SFX");
    }

    // This setups sounds in groups
    private void SetupGroupSound(GroupSound s, Group group, GameObject targetGameObject)
    {
        s.source = targetGameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        // Set other AudioSource properties based on GroupSound properties
        s.source.volume = group.groupVolume;
        s.source.spatialBlend = group.groupSpatialBlend;
        AssignGroupToAudioSource(s.source, "SFX");
    }

    // Assign an AudioSource to a specific mixer group
    public static void AssignGroupToAudioSource(AudioSource audioSource, string groupName)
    {
        if (mainAudioMixer == null)
        {
            Debug.LogError("Main AudioMixer is not loaded.");
            return;
        }

        AudioMixerGroup[] mixerGroups = mainAudioMixer.FindMatchingGroups(groupName);
        if (mixerGroups.Length > 0)
        {
            audioSource.outputAudioMixerGroup = mixerGroups[0];
        }
        else
        {
            Debug.LogWarning($"AudioMixerGroup '{groupName}' not found.");
        }
    }

    public void FadeMixerVolume(float linearVolume, float fadeDuration = 2f)
    {
        float targetVolumeInDb;

        if (linearVolume < 0)
            targetVolumeInDb = LinearToDecibel(PlayerPrefs.GetFloat("Master")/100f);
        else if (linearVolume == 0)
        {
            targetVolumeInDb = -80f;
        }
        else
        {
            // Clamp to avoid negative infinity in decibel conversion
            linearVolume = Mathf.Clamp(linearVolume, 0.0001f, 1f);
            // Convert the clamped linear volume to decibels
            targetVolumeInDb = LinearToDecibel(linearVolume);
        }

        StartCoroutine(FadeMixer(targetVolumeInDb, fadeDuration));
    }

    public static float LinearToDecibel(float linear)
    {
        if (linear == 0) return -80f; // Return -80 dB for silence
        return Mathf.Log10(linear) * 20f;
    }

    public static float DecibelToLinear(float dB)
    {
        return Mathf.Pow(10.0f, dB / 20.0f);
    }


    IEnumerator FadeMixer(float v, float d)
    {
        float currentTime = 0;
        mainAudioMixer.GetFloat("Master", out float currentVolume); // Get the current volume

        while (currentTime < d)
        {
            currentTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(currentVolume, v, currentTime / d);
            mainAudioMixer.SetFloat("Master", newVolume);
            yield return null; // Wait a frame
        }

        mainAudioMixer.SetFloat("Master", v);
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


    public void Play(string name)
    {
        Sound s = FindSound(name);

        if (s != null)
            s.source.Play();
    }

    public void StopPlaying(string sound)
    {
        Sound s = FindSound(sound);

        if (s != null)
            s.source.Stop();
    }

    public void Pause(string sound)
    {
        Sound s = FindSound(sound);

        if (s != null)
            s.source.Pause();
    }

    public void UnPause(string sound)
    {
        Sound s = FindSound(sound);

        if (s != null)
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
