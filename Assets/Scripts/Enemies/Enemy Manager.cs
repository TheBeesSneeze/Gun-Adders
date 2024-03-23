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