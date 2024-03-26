/*******************************************************************************
* File Name :         UniversalPrefabs.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/25/2024
*
* Brief Description : im trying something funky. dont put this guy on a gameobject please
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalPrefabs : MonoBehaviour
{
    [Tooltip("Used by explosion bullets")]
    public GameObject PlayerExplosion;

    public static UniversalPrefabs Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
