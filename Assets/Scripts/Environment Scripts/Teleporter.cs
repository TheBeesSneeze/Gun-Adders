/*******************************************************************************
 * File Name :         Teleporter.cs
 * Author(s) :         Claire
 * Creation Date :     3/29/2024
 *
 * Brief Description : 
 *****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehaviour _))
        {
            StartCoroutine(FindObjectOfType<PauseMenu>().Transition("Environment"));
        }
    }
}
