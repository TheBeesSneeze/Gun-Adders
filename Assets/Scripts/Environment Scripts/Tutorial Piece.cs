using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPiece : MonoBehaviour
{
    public string content;
    public int popupDuration = 10;
    public string sound;

    [HideInInspector] public bool hasTriggered = false;

    private void Start()
    {
        AssignEventListeners();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehaviour _) && !hasTriggered)
        {
            hasTriggered = true;
            if (sound != "" && AudioManager.instance != null)
                AudioManager.instance.Play(sound);

            Tutorial.instance.RequestDisplay(content, popupDuration);
        }
    }

    private void AssignEventListeners()
    {
        InputEvents.Instance.MoveHeld.AddListener(MovementStarted);
        InputEvents.Instance.JumpStarted.AddListener(JumpStarted);
    }

    private void JumpStarted()
    {
        if (hasTriggered && content == "Press 'Spacebar' to Jump")
            Tutorial.instance.CancelDisplayIfMatches(content);
    }

    private void MovementStarted()
    {
        if (hasTriggered && content == "Press 'WASD' to Move")
            Tutorial.instance.CancelDisplayIfMatches(content);
    }
}
