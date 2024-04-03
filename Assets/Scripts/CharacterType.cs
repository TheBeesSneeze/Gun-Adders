/*******************************************************************************
 * File Name :         CharacterType.cs
 * Author(s) :         Toby
 * Creation Date :     3/20/2024
 *
 * Brief Description : base code shared between enemies and player.
 * damage should be taken from external projectile scripts
 * 
 * stores:
 * health
 * 
 * public functions:
 * take damage
 *****************************************************************************/

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterType : MonoBehaviour
{

    [ReadOnly] public float CurrentHealth = 1;
    // Start is called before the first frame update

    protected virtual void Start()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    { 
        Destroy(gameObject); 
    }
}
