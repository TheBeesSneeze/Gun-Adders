/*******************************************************************************
 * File Name :         Title.cs
 * Author(s) :         Claire
 * Creation Date :     3/29/2024
 *
 * Brief Description : 
 *****************************************************************************/
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Title : MonoBehaviour
{
    public int popupDuration = 10;
    public AudioSource ambience;

    private TextMeshProUGUI title;
    private ColorAdjustments colorAdjustments;
    private bool hasTriggered = false;

    private void Start()
    {
        Application.targetFrameRate = 144;
        PauseMenu.IsPaused = false;
        title = GameObject.Find("Title Text").GetComponent<TextMeshProUGUI>();
        title.enabled = false;
        title.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehaviour _) && !hasTriggered)
        {
            if (hasTriggered)
                return;

            if (FindObjectOfType<Volume>().profile.TryGet(out colorAdjustments))
            {
                colorAdjustments.saturation.value = 3;
            }
            title.enabled = true;
            StartCoroutine(TitlePopup());
        }
    }

    IEnumerator TitlePopup()
    {
        ambience.volume = 0.1f;
        GetComponent<AudioSource>().Play();
        hasTriggered = true;
        yield return new WaitForSeconds(popupDuration);
        StartCoroutine(FadeTitle());
    }

    IEnumerator FadeTitle()
    {
        float time = 0f;
        float currentDilate;
        float currentUnderlay;
        while (time < 2f)
        {
            time += Time.deltaTime;

            currentDilate = Mathf.Lerp(0f, -1f, time / 2f);
            currentUnderlay = Mathf.Lerp(0.638f, 0, time / 2f);
            title.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, currentDilate);
            title.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, currentUnderlay);
            FindObjectOfType<Vignette>().intensity.value = Mathf.Lerp(0.35f, 0.05f, time / 2f);
            yield return null;
        }
        title.enabled = false;
    }
}
