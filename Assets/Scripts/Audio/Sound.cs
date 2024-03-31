/*******************************************************************************
 * File Name :         Sound.cs
 * Author(s) :         Clare
 * Creation Date :     3/24/2024
 *
 * Brief Description : 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 0.5f;
    [Range(0f, 1f)] public float spatialBlend = 0;
    public bool loop;
    public GameObject parent;
    [HideInInspector]public AudioSource source;
}

[System.Serializable]
public class Group
{
    public string name;
    [Range(0f, 1f)] public float groupVolume = 0.5f;
    [Range(0f, 1f)] public float groupSpatialBlend = 0;
    public GameObject parent;

    public GroupSound[] sounds;
}

[System.Serializable]
public class GroupSound
{
    public AudioClip clip;
    [HideInInspector] public AudioSource source;
}
