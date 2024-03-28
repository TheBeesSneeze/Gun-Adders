using Beautify.Universal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Teleporter : MonoBehaviour
{
    public GameObject black, white;
    public AudioMixer mixer;

    private AudioSource[] sources;
    private List<float> volumes;
    private Material material;

    private void Start()
    {
        sources = FindObjectsOfType<AudioSource>();
        volumes = new List<float>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehaviour _))
        {
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        float currentTime = 0f;
        black.SetActive(true);
        white.SetActive(true);
        material = white.GetComponent<Image>().material;
        material.shader = Shader.Find("Custom/Dissolve");
        foreach (AudioSource source in sources)
            volumes.Add(source.volume);
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            material.SetFloat("_DissolveThreshold", Mathf.Lerp(0, 1, currentTime / 1f));
            for (int i = 0; i < sources.Length - 1; i++)
                sources[i].volume = Mathf.Lerp(volumes[i], 0, currentTime / 1f);
            yield return null;
        }
        SceneManager.LoadScene("Environment");
    }
}
