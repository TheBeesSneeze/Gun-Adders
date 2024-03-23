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


    private void Update()
    {
        SpawnEnemies();
    }
    void SpawnEnemies()
    {
        if(!InputEvents.RespawnPressed) { return; }
        for(int i = 0; i < spawnPoints.Length; ++i)
        {
            Instantiate(enemey, spawnPoints[i].transform.position, Quaternion.identity);
        }
        InputEvents.RespawnPressed = false;
        
    }


}