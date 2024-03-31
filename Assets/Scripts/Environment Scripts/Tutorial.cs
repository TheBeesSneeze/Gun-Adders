/*******************************************************************************
 * File Name :         Tutorial.cs
 * Author(s) :         Claire
 * Creation Date :     3/29/2024
 *
 * Brief Description : 
 *****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    private GameObject infoBox;
    private TextMeshProUGUI text;
    private Queue<(string message, int duration)> messageQueue = new Queue<(string, int)>();
    private bool isDisplayingMessage = false;
    private string currentMessage = "";

    public static Tutorial instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        infoBox = GameObject.Find("InfoBox");
        text = infoBox.GetComponentInChildren<TextMeshProUGUI>();
        text.enabled = false;
        infoBox.SetActive(false);
    }

    public void RequestDisplay(string message, int duration)
    {
        Debug.Log($"Requesting display: {message}");
        messageQueue.Enqueue((message, duration));
        if (!isDisplayingMessage)
        {
            StartCoroutine(DisplayMessageCoroutine());
        }
    }

    IEnumerator DisplayMessageCoroutine()
    {
        while (messageQueue.Count > 0)
        {
            isDisplayingMessage = true;
            (string message, int duration) = messageQueue.Dequeue();
            currentMessage = message;

            text.text = message;
            text.enabled = true;
            infoBox.SetActive(true);

            Debug.Log($"Displaying message: {message} for {duration} seconds");
            yield return new WaitForSeconds(duration);

            text.enabled = false;
            infoBox.SetActive(false);
            Debug.Log("Message display ended");

            isDisplayingMessage = false;
            currentMessage = "";
            if (messageQueue.Count > 0)
            {
                // There's another message waiting, continue without pausing.
                continue;
            }
            yield return null; // Ensure the loop can restart cleanly.
        }
    }

    public void CancelDisplayIfMatches(string condition)
    {
        if (currentMessage.Equals(condition))
        {
            Debug.Log($"Cancelling message: {condition}");
            currentMessage = "";
            StopAllCoroutines(); // Be cautious with this, as it stops all coroutines
            StartCoroutine(DelayedHideMessage());
        }
    }

    private IEnumerator DelayedHideMessage()
    {
        yield return new WaitForSeconds(1);
        text.enabled = false;
        infoBox.SetActive(false);
        isDisplayingMessage = false;
        Debug.Log("Message hidden after delay");
    }
}
