﻿/*******************************************************************************
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
using UnityEngine;

public abstract class BulletEffect : ScriptableObject
{
    public Color TrailColor= Color.white;
    public string UpgradeName;
    public abstract void OnEnemyHit(EnemyType type);

    public abstract void OnHitOther(Vector3 point);
}