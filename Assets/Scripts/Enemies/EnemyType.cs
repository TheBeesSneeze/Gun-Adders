/*******************************************************************************
* File Name :         EnemyType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/20/2024
*
* Brief Description : 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : CharacterType
{
    public float DefaultHealth=1;

    protected override void Start()
    {
        base.Start();
        CurrentHealth = DefaultHealth;
    }
}
