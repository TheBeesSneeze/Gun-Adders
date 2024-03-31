/*******************************************************************************
 * File Name :         Credits.cs
 * Author(s) :         Claire
 * Creation Date :     3/29/2024
 *
 * Brief Description : 
 *****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public GameObject title;
    public GameObject buttons;

    public void StartCredits()
    {
        AudioManager.instance.Play("Click");
        title.SetActive(false);
        buttons.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(MoveCredits());
    }

    public void ExitCredits()
    {
        AudioManager.instance.Play("Click");
        title.SetActive(true);
        buttons.SetActive(true);
        gameObject.SetActive(false);
        gameObject.GetComponent<GridLayoutGroup>().padding.top = 0;
    }

    IEnumerator MoveCredits()
    {
        float time = 0;
        GridLayoutGroup group = gameObject.GetComponent<GridLayoutGroup>();
        while (time < 30f)
        {
            time += Time.deltaTime;
            group.padding.top = Mathf.RoundToInt(Mathf.Lerp(0f, -2400f, time / 30f));
            LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
            yield return null;
        }
        time = 0;
        TextMeshProUGUI finalText = gameObject.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        float a = 1f;
        while (time < 5f)
        {
            time += Time.deltaTime;
            finalText.color = new(finalText.color.r, finalText.color.g, finalText.color.b, Mathf.Lerp(a, 0, time / 5f));
            yield return null;
        }

        ExitCredits();
    }
}
