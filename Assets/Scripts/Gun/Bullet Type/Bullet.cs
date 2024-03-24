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
    [SerializeField] private float bulletForce = 200f;
    [SerializeField] private float despawnTime = 5f;
    [SerializeField] private float hitRigidbodyImpactForce = 10f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private GameObject hitImpactEffectPrefab;
    [SerializeField] private float impactEffectPrefabDespawnTime = 0.2f;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private BulletEffect bulletEffect;
    private Rigidbody rb;
    private Vector3 lastPosition;
    private float lastTime;
    void Start()
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
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(hit.normal * hitRigidbodyImpactForce, hit.point, ForceMode.Impulse);
                }
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
