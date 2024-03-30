/*******************************************************************************
* File Name :         EnemyType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/20/2024
*
* Brief Description : 
 *****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyType : CharacterType
{
    public float DefaultHealth = 1;
    private Slider slider;
    internal bool slowed { get; private set; }
    private float slowTimer = 0f;
    private float slowTimeRef = 0f;
    private bool canDamage = false;
    [SerializeField] private float iFrameSeconds = 1;
    [SerializeField] private int enemyDamage = 1;
    private PlayerBehaviour player;
    private Coroutine playerIFrames;
    [Tooltip ("What color the enemy is when it takes damage")]
    [SerializeField] private Color enemyDamageColor;
    private MeshRenderer mR;
    [Tooltip("Enemy damage flash time")]
    [SerializeField] private float enemyDamageFlashSeconds = 0.5f;
    private Color enemyOriginalColor;

    protected override void Start()
    {
        base.Start();
        CurrentHealth = DefaultHealth;
        slider = GetComponentInChildren<Slider>();
        mR = GetComponent<MeshRenderer>();
        enemyOriginalColor = mR.material.color;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (slider != null)
        {
            float t = CurrentHealth / DefaultHealth;
            slider.value = t;
        }

        StartCoroutine(EnemyDamageFlash());

    }

    public IEnumerator EnemyDamageFlash()
    {
        mR.material.color = enemyDamageColor;
        Debug.Log("setting color");
        yield return new WaitForSeconds(enemyDamageFlashSeconds);
        mR.material.color = enemyOriginalColor;
        Debug.Log("normal color");
    }

    public void ApplySlow(float slowTime)
    {
        slowed = true;
        slowTimer = 0f;
        slowTimeRef = slowTime;
    }
    
    private void Update()
    {
        if (slowed)
        {
            if (slowTimer < slowTimeRef)
            {
                slowTimer += Time.deltaTime;
            }
            else
            {
                slowTimer = 0f;
                slowed = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            player = collision.gameObject.GetComponent<PlayerBehaviour>();
            playerIFrames = StartCoroutine(PlayerIFrames());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        canDamage = false;
        if (playerIFrames != null)
        {
            StopCoroutine(playerIFrames); 
        }
        
    }

    public IEnumerator PlayerIFrames()
    {
        canDamage = true;
        do
        {
            player.TakeDamage(enemyDamage);
            Debug.Log("doin damage!");
            yield return new WaitForSeconds(iFrameSeconds);
        }
        while (canDamage);
        
    }
}
