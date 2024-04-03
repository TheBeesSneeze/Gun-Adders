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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStats))]
public class PlayerBehaviour : CharacterType
{
    private PlayerStats stats;
    [SerializeField] private Image redVignette;
    private float secondsSinceLastTookDamage;
    // Start is called before the first frame update
    protected override void Start()
    {
        SetStats();
    }

    public void Update()
    {
        if (secondsSinceLastTookDamage >= stats.SecondsUntilHealing)
        {
            CurrentHealth += stats.HealthRegen * Time.deltaTime;
            CurrentHealth = Mathf.Min(CurrentHealth, stats.DefaultHealth);
        }
        secondsSinceLastTookDamage += Time.deltaTime;
        
        float t = CurrentHealth / stats.DefaultHealth;
        t = 1 - t;
        redVignette.color = new Color(redVignette.color.r, redVignette.color.g, redVignette.color.b, t * t);
    }

    public void SetStats()
    {
        stats = GetComponent<PlayerStats>();

        CurrentHealth = stats.DefaultHealth;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        secondsSinceLastTookDamage = 0;
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    }

