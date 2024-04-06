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

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] public Transform[] spawnPoints;
    [Tooltip("This is multiplied by round number to calculate how many more enemies per round")]
    [SerializeField] public int roundExtraSpawn = 1;
    public GameObject[] enemyPrefabs;
    private DynamicStage dynamicStage;


    public int numberOfEnemies;

    private int roundNumber;

    private bool roundJustEnd;
    [SerializeField]public float timeBetweenRounds = 5f;
    private float currentTime;
    private float goalTime; 

    /*
     * public Transform bulletUpgrade spawn
     * public Transform gunUpgrade spawn
     * public GameObject bullet Upgrade
     * public Gameobject gunupgrade
     * public int roundsBetweenUpgradeSpawn
    */
    public void Start()
    {
        numberOfEnemies = 0;
        roundNumber = 0;
        RoundStart();
    }

    public void RoundStart()
    {
        roundJustEnd = false;
        /*
         * if(round % roundsBetweenUpgradeSpawn) 
         * {
         *      instantiate(bulletUpgrade, bulletUpgradeSpawn.position, Quaaternion.identiy)
         *      instantiate(gunUpgrade, gunUpgradeSpawn.position, Quaternion.identiy)
         *  }
         *  else
         *  {
         *      somehow destroy the gunUp and bullet up in the scene currently need to ask how these work 
         *  }
         */
        if(GameObject.FindAnyObjectByType<DynamicStage>() != null)
        {
            dynamicStage = FindAnyObjectByType<DynamicStage>();
            dynamicStage.ChangeStage();
        }
        
        numberOfEnemies = 0;
        SpawnEnemies();
        ++roundNumber;
    }

    public void RoundEnd()
    {
        roundJustEnd = true; 
        if(currentTime > goalTime) { RoundStart(); }
        else { currentTime = Time.time; }

        
    }

    public void Update()
    {
        if (enemyPrefabs == null) { return; }
        if (numberOfEnemies == 0) 
        {
            if (!roundJustEnd)
            {
                currentTime = Time.time;
                goalTime = currentTime + timeBetweenRounds;
            }
            RoundEnd();
            
        }

    }

    public void SpawnEnemies()
    {
        int iterrations = spawnPoints.Length + (roundExtraSpawn * roundNumber);

        for (int i = 0; i < iterrations; ++i)
        {
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            Vector3 location = new Vector3(
                spawnPoints[i % (spawnPoints.Length)].transform.position.x + Random.Range(-1f, 1f),
                spawnPoints[i % (spawnPoints.Length)].transform.position.y,
                spawnPoints[i % (spawnPoints.Length)].transform.position.z + Random.Range(-1f, 1f));
            Quaternion q = new Quaternion(0, 0, 0, 0);
            Instantiate(enemyPrefabs[enemyIndex], location, q);
            
            ++numberOfEnemies;
        }
    }

}