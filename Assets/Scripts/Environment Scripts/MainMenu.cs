using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject black;
    public AudioMixer mixer;
    public TextMeshProUGUI title;

    private AudioSource[] sources;
    private List<float> volumes;
    private Material material;
    private ColorAdjustments colorAdjustments;
    private Transform subTitle;
    private Vector3 maxScale, minScale;
    private bool scalingUp = true;
    private float currentLerpTime;

    private void Start()
    {    
        Application.targetFrameRate = 144;
        sources = FindObjectsOfType<AudioSource>();
        volumes = new List<float>();
        subTitle = title.transform.GetChild(0);
        minScale = new(0.9f, 0.9f, 0.9f);
        maxScale = new(1.1f, 1.1f, 1.1f);
        StartCoroutine(ChangeColor());
    }

    private void FixedUpdate()
    {
        Camera.main.transform.Rotate(Vector3.up, 7f * Time.fixedDeltaTime, Space.World);

        if (scalingUp)
        {
            currentLerpTime += Time.fixedDeltaTime * 3f;
            if (currentLerpTime > 1.0f)
            {
                currentLerpTime = 1.0f;
                scalingUp = false;
            }
        }
        else
        {
            currentLerpTime -= Time.fixedDeltaTime * 3f;
            if (currentLerpTime < 0.0f)
            {
                currentLerpTime = 0.0f;
                scalingUp = true;
            }
        }

        subTitle.localScale = Vector3.Lerp(minScale, maxScale, currentLerpTime);
    }

    public void ButtonHover()
    {
        AudioManager.instance.Play("Hover");
    }

    public void StartButton()
    {
        AudioManager.instance.Play("Click");
        AudioManager.instance.Play("Start");
        StartCoroutine(Transition("Environment"));
    }

    public void TutorialButton()
    {
        AudioManager.instance.Play("Click");
        AudioManager.instance.Play("Start");
        StartCoroutine(Transition("Tutorial"));
    }

    public void QuitButton()
    {
        AudioManager.instance.Play("Click");
        Application.Quit();
    }

    IEnumerator Transition(string scene)
    {
        float currentTime = 0f;
        black.SetActive(true);
        material = black.GetComponent<Image>().material;
        material.shader = Shader.Find("Custom/Dissolve");
        foreach (AudioSource source in sources)
            volumes.Add(source.volume);
        while (currentTime < 2f)
        {
            currentTime += Time.deltaTime;
            material.SetFloat("_DissolveThreshold", Mathf.Lerp(1, 0, currentTime / 2f));
            for (int i = 0; i < sources.Length; i++)
                sources[i].volume = Mathf.Lerp(volumes[i], 0, currentTime / 2f);
            yield return null;
        }
        SceneManager.LoadSceneAsync(scene);
    }

    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(39.45f);
        if (FindObjectOfType<Volume>().profile.TryGet(out colorAdjustments))
        {
        }

        float currentSoft;
        float time = 0;
        while (time < 2f)
        {
            time += Time.deltaTime;
            currentSoft = Mathf.Lerp(0.5f, 0f, time / 2f);
            colorAdjustments.saturation.value = Mathf.Lerp(70f, 3f, time / 2f);
            title.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, currentSoft);
        }
    }
}
