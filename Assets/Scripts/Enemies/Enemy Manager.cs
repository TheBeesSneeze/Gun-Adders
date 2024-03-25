/*******************************************************************************
 * File Name :         InputEvents.cs
 * Author(s) :         Clare
 * Creation Date :     3/23/2024
 *
 * Brief Description : 
 * Made up of an array of empty transforms that represent the spawn points
 * When the player hits F it will spawn in enemies at every spawn point 
 * 
 *****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public Transform[] spawnPoints;
    public GameObject enemey;
    private static int roundNumber = 0;

    public void Update()
    {
        if (enemey == null) { return; }
        SpawnEnemies();
    }
    public void SpawnEnemies()
    {
        if(!InputEvents.RespawnPressed) { return; }

        int iterrations = spawnPoints.Length * roundNumber; 
        for (int i = 0; i < iterrations; ++i)
        {
            Instantiate(enemey, spawnPoints[i % (spawnPoints.Length)].transform.position, Quaternion.identity);
        }
        ++roundNumber;
        InputEvents.RespawnPressed = false;
        
    }


}