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

[RequireComponent(typeof(PlayerStats))]
public class PlayerBehaviour : CharacterType
{
    private PlayerStats stats;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private GameObject damageCanvas;
    [SerializeField] private float visualDamageSeconds = 0.5f;
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

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (hurtSound != null)
        {
            AudioSource.PlayClipAtPoint(hurtSound, gameObject.transform.position);
        }

        damageCanvas.SetActive(true);
        StartCoroutine(TakeDamageVisual());
    }

    public IEnumerator TakeDamageVisual()
    {
        yield return new WaitForSeconds(visualDamageSeconds);
        damageCanvas.SetActive(false);
    }

    public override void Die()
    {
        SceneManager.LoadScene("Arena Scene");
    }
}
