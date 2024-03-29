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
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyType : CharacterType
{
    public float DefaultHealth=1;
    private Slider slider;
    internal bool slowed { get; private set; }
    private float slowTimer = 0f;
    private float slowTimeRef = 0f;
    private bool canDamage = false;
    [SerializeField] private float iFrameSeconds = 1;
    [SerializeField] private int enemyDamage = 1;
    private PlayerBehaviour player;
    private Coroutine iFrames;

    [Header("Ranged Enemy Stuffs")]
    public Transform playerLocation;

    [SerializeField]public bool isRangedEnemy;
    [SerializeField]private GameObject BulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float rangeOfAttack = 20;
    [SerializeField] private float slowedFireRate = 1f;
    [SerializeField] private float rangedAttackDamage = 20;

    private bool isAttacking = false;
    private Vector3 Distance;
    private float distanceFrom;
    private float nextFire = 0;
    private float currentFireRate;
    

    protected override void Start()
    {
        base.Start();
        CurrentHealth = DefaultHealth;
        slider = GetComponentInChildren<Slider>();
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

    public void ApplySlow(float slowTime)
    {
        slowed = true;
        slowTimer = 0f;
        slowTimeRef = slowTime;
    }
    
    private void Update()
    {
        currentFireRate = fireRate;
        if (slowed)
        {
             
            if (slowTimer < slowTimeRef)
            {
                slowTimer += Time.deltaTime;
                currentFireRate = slowedFireRate;
            }
            else
            {
                slowTimer = 0f;
                slowed = false;
                currentFireRate = fireRate; 
            }
        }
        if(isRangedEnemy)
        {
            Attacking();
            canRangeAttack();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            player = collision.gameObject.GetComponent<PlayerBehaviour>();
            iFrames = StartCoroutine(IFrames());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        canDamage = false;
        if (iFrames != null)
        {
            StopCoroutine(iFrames); 
        }
        
    }

    public IEnumerator IFrames()
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

    private void canRangeAttack()
    {
        Distance = (transform.position - playerLocation.position).normalized;
        Distance.y = 0;
        distanceFrom = Distance.magnitude;
        Distance /= distanceFrom;

        if(distanceFrom < rangeOfAttack)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false; 
        }
    }
    private void Attacking()
    {
        if(isAttacking)
        {
            transform.LookAt(playerLocation.position);
            
            if(Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                var Shoot = Instantiate(BulletPrefab.gameObject.GetComponent<Rigidbody>(), bulletSpawnPoint.position, playerLocation.rotation);
                Shoot.AddForce(playerLocation.position * 500, ForceMode.Acceleration);
                
            }
        }
    }
    
}
