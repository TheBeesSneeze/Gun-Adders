/*******************************************************************************
 * File Name :         InputEvents.cs
 * Author(s) :         Clare
 * Creation Date :     3/23/2024
 *
 * Brief Description : 
 * Spawns in enemies when press F key
 * Each round spawns more enemies 
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
    [Tooltip("This is multiplied by round number to calculate how many more enemies per round")]
    [SerializeField] public int roundExtraSpawn = 1;
    public GameObject enemey;
    

    private int roundNumber = 0;

    public void Update()
    {
        if (enemey == null) { return; }
        SpawnEnemies();
    }
    public void SpawnEnemies()
    {
        if(!InputEvents.RespawnPressed) { return; }

        int iterrations = spawnPoints.Length + (roundExtraSpawn * roundNumber); 
        for (int i = 0; i < iterrations; ++i)
        {
            Instantiate(enemey, spawnPoints[i % (spawnPoints.Length)].transform.position, Quaternion.identity);
        }
        ++roundNumber;
        InputEvents.RespawnPressed = false;
        
    }


}