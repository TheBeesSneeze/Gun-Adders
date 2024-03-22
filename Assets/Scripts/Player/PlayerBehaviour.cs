/*******************************************************************************
 * File Name :         PlayerBehaviour.cs
 * Author(s) :         Toby
 * Creation Date :     3/18/2024
 *
 * Brief Description : The player code that does NOT have to do with input. 
 * Health / collisions / whatever.
 * 
 * projectile collisions are handles in AttackType.cs
 *****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerBehaviour : CharacterType
{
    private PlayerStats stats;
    // Start is called before the first frame update
    protected override void Start()
    {
        SetStats();
    }

    public void SetStats()
    {
        stats = GetComponent<PlayerStats>();

        CurrentHealth = stats.DefaultHealth;
    }
}
