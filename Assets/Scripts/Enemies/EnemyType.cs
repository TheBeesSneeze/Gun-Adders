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
using UnityEngine.UI;

public class EnemyType : CharacterType
{
    public float DefaultHealth=1;
    private Slider slider;

    protected override void Start()
    {
        base.Start();
        CurrentHealth = DefaultHealth;
        slider = GetComponent<Slider>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (slider != null)
        {
            float t = CurrentHealth / DefaultHealth;
            slider.value = t;
        }

    }
}
