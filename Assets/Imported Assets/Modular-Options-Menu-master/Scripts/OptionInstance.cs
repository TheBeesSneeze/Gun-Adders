using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class OptionInstance : MonoBehaviour
{
    public static OptionInstance Instance;
    public static float sensitivity;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    public void ClickSound()
    {
        instance.Play("Click");
    }

    public void HoverSound()
    {
        instance.Play("Hover");
    }

    public void SliderSound()
    {
        instance.Play("Slider");
    }

    public void BackSound()
    {
        instance.Play("Unpause");
    }

    public void ConfirmMenuSound()
    {
        instance.Play("Warning");
    }
}
