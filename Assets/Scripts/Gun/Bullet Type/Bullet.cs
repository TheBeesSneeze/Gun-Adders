/*******************************************************************************
* File Name :         Bullet.cs
* Author(s) :        Alec Pizziferro
* Creation Date :     3/22/2024
*
* Brief Description : Projectile Bullet Physics
 *****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
   
    [SerializeField] private float despawnTime = 5f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private GameObject hitImpactEffectPrefab;
    [SerializeField] private float impactEffectPrefabDespawnTime = 0.2f;
    [SerializeField] private BulletEffect bulletEffect;
    [SerializeField] private BulletEffect bulletEffect2;
    private Rigidbody rb;
    private Vector3 lastPosition;
    private float lastTime;

    [HideInInspector] public float damageAmount = 10f;
    [HideInInspector] public float bulletForce = 200f;

    /// <summary>
    /// was previously start function, changed to get called in GunController
    /// </summary>
    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (lastTime < despawnTime)
        {
            lastTime += Time.fixedDeltaTime;
            Debug.DrawRay(lastPosition, transform.forward);
            if (Physics.Raycast(lastPosition, transform.forward, out RaycastHit hit, Vector3.Distance(transform.position, lastPosition), hitLayers,
                    QueryTriggerInteraction.Ignore))
            {
                print(hit.collider);
                if (hitImpactEffectPrefab != null)
                {
                    var obj = Instantiate(hitImpactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(obj, impactEffectPrefabDespawnTime);
                }

                if (hit.collider.TryGetComponent(out EnemyType enemy))
                {
                    enemy.TakeDamage(damageAmount);
                    if (bulletEffect != null)
                    {
                        bulletEffect.OnEnemyHit(enemy);
                    }

                    if (bulletEffect2 != null)
                    {
                        bulletEffect2.OnEnemyHit(enemy);
                    }
                }
                Destroy(gameObject);
            }

            lastPosition = transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
