/*******************************************************************************
 * File Name :         PauseMenu.cs
 * Author(s) :         Claire
 * Creation Date :     3/31/2024
 *
 * Brief Description : 
 *****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Image black;
    [SerializeField] private GameObject fadeBlack;

    public static bool IsPaused = false;

    private void Start()
    {
        IsPaused = false;
        StartCoroutine(FadeSceneIn());
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        if (IsPaused)
        {
            Debug.Log("Pausing game...");
            FadePause(true);
            AudioManager.instance.Play("Pause");
        }
        else
        {
            ToggleOptions(false);
            Debug.Log("Unpausing game...");
            FadePause(false);
            AudioManager.instance.Play("Click");
            AudioManager.instance.Play("Unpause");
        }
    }

    public void ToggleOptions(bool on)
    {
        float defaultVolume = PlayerPrefs.GetFloat("Master") / 100f;

        AudioManager.instance.FadeMixerVolume(on ? defaultVolume : defaultVolume / 2f, 0);
        OptionInstance.Instance.transform.GetChild(0).gameObject.SetActive(on);
    }

    private void FadePause(bool fadeIn)
    {
        // Activate the UI element for pause menu
        transform.GetChild(0).gameObject.SetActive(fadeIn);
        float defaultVolume = PlayerPrefs.GetFloat("Master") / 100f;
        float targetVolume = fadeIn ? defaultVolume / 2f : defaultVolume;

        black.color = new Color(black.color.r, black.color.g, black.color.b, fadeIn ? 0.5f : 0);
        AudioManager.instance.FadeMixerVolume(fadeIn ? targetVolume : defaultVolume, 0);
        Cursor.visible = fadeIn;
        Cursor.lockState = fadeIn ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = fadeIn ? 0 : 1;
    }

    public void ButtonHover()
    {
        // Play hover sound effect
        AudioManager.instance.Play("Hover");
    }

    public void BackToMain()
    {
        // Play click and start sound effects
        AudioManager.instance.Play("Click");
        AudioManager.instance.Play("Start");

        Time.timeScale = 1;

        // Start the scene transition coroutine to the main menu
        StartCoroutine(Transition("Main Menu"));
    }

    public void QuitButton()
    {
        // Play click sound effect and quit the application
        AudioManager.instance.Play("Click");
        Application.Quit();
    }

    public IEnumerator Transition(string scene)
    {
        Material material;
        float currentTime = 0f;

        // Enable the fadeBlack GameObject and get its material
        fadeBlack.SetActive(true);
        material = fadeBlack.GetComponent<Image>().material;
        material.shader = Shader.Find("Custom/Dissolve");

        AudioManager.instance.FadeMixerVolume(0);
        // Dissolve effect over 2 seconds
        while (currentTime < 2f)
        {
            currentTime += Time.deltaTime;
            material.SetFloat("_DissolveThreshold", Mathf.Lerp(1, 0, currentTime / 2f));
            yield return null;
        }

        // Load the specified scene asynchronously
        SceneManager.LoadSceneAsync(scene);
    }

    IEnumerator FadeSceneIn()
    {
        Material material;
        float currentTime = 0f;

        // Enable the fadeBlack GameObject and get its material
        fadeBlack.SetActive(true);
        material = fadeBlack.GetComponent<Image>().material;
        material.shader = Shader.Find("Custom/Dissolve");

        AudioManager.mainAudioMixer.SetFloat("Master", -80);
        AudioManager.instance.FadeMixerVolume(-1, 1f);
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            material.SetFloat("_DissolveThreshold", Mathf.Lerp(0, 1, currentTime / 1f));

            yield return null;
        }
    }
}