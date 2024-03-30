/*******************************************************************************
* File Name :         ElectricityBullet.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/30/2024
*
* Brief Description : zaps nearby enemies
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityBullet : BulletEffect
{
    public float ElectrocuteDistance;
    public int MaxEnemiesToZap;

    public LayerMask OnlyEnemiesMask;

    public override void OnEnemyHit(EnemyType type)
    {
        throw new System.NotImplementedException();
    }

    public override void OnHitOther(Vector3 point)
    {
        
    }
    
    /// <summary>
    /// some values in list will/may be null
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private EnemyType[] GetEnemiesInRange(Vector3 point)
    {
        RaycastHit[] hit;
        hit = Physics.SphereCastAll(point, MaxEnemiesToZap, Vector3.zero, OnlyEnemiesMask);

        EnemyType[] enemies = new EnemyType[hit.Length];

        for(int i =0; i< hit.Length; i++)
        {
            enemies[i] = hit[i].transform.GetComponent<EnemyType>();
        }

        return enemies;

    }
}
