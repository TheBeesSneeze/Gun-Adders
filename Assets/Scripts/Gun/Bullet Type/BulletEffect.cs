/*******************************************************************************
 * File Name :         BulletEffect.cs
 * Author(s) :        Alec, Toby, Sky
 * Creation Date :     3/22/2024
 *
 * Brief Description : Scriptable Object base for handling Enemy Logic
 *****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletEffect : ScriptableObject
{
    public Color TrailColor= Color.white;
    public string UpgradeName;
    //this is the longest tooltip in the game
    [Tooltip("*IF* the bullet effect has a component that does damage, that damage is determined by multiplying the current shootmodes damage (per bullet) by this number")]
    public float DamageMultiplier; //you might have to implement this manually on a effect by effect basis. sorry guys
    public abstract void OnEnemyHit(EnemyType type, float damage);

    public abstract void OnHitOther(Vector3 point, float damage);
}