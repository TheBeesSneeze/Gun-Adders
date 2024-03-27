/*******************************************************************************
* File Name :         Bullet.cs
* Author(s) :         Alec Pizziferro
* Creation Date :     3/22/2024
*
* Brief Description : Projectile Bullet Physics
 *****************************************************************************/
using NaughtyAttributes;
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

    [HideInInspector] public BulletEffect _bulletEffect1;
    [HideInInspector] public BulletEffect _bulletEffect2;

    private Rigidbody rb;
    private Vector3 lastPosition;
    private float lastTime;

    [HideInInspector] public float damageAmount = 10f;
    [HideInInspector] public float bulletForce = 200f;

    /// <summary>
    /// was previously start function, changed to get called in GunController
    /// </summary>
    public void Initialize(BulletEffect bulletEffect1, BulletEffect bulletEffect2, Vector3 dir)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(dir.normalized * bulletForce, ForceMode.Impulse);
        lastPosition = transform.position;

        _bulletEffect1 = bulletEffect1;
        _bulletEffect2 = bulletEffect2;


        GetComponent<TrailRenderer>().enabled = true;

        /*
        GetComponent<TrailRenderer>().startColor = GetBulletColor();
        GetComponent<TrailRenderer>().endColor = GetBulletColor();

        GetComponent<Material>().color = GetBulletColor();
        */

        SetColorGradient();
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
                    if (_bulletEffect1 != null)
                    {
                        _bulletEffect1.OnEnemyHit(enemy);
                    }

                    if (_bulletEffect2 != null)
                    {
                        _bulletEffect2.OnEnemyHit(enemy);
                    }
                }
                //if hit something that isnt enemy
                else
                {
                    Debug.Log("hit other");
                    if (_bulletEffect1 != null)
                    {
                        _bulletEffect1.OnHitOther(hit.point);
                    }

                    if (_bulletEffect2 != null)
                    {
                        _bulletEffect2.OnHitOther(hit.point);
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


    /// <summary>
    /// this code does not work
    /// </summary>
    private void SetColorGradient()
    {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.enabled = true;

        tr.startColor = Color.white;
        tr.endColor = Color.white;

        if (_bulletEffect1 != null)
        {
            tr.startColor = _bulletEffect1.TrailColor;
        }
        if (_bulletEffect2 != null)
        {
            tr.endColor = _bulletEffect2.TrailColor;
        }
    }

    /// <summary>
    /// averages the colors from both bullet effects.
    /// returns white if no upgrades are loaded
    /// </summary>
    /// <returns></returns>
    private Color GetBulletColor()
    {
        if(_bulletEffect1 == null && _bulletEffect2 == null)
        {
            return Color.white;
        }

        if(_bulletEffect1 == null)
        {
            return _bulletEffect2.TrailColor;
        }

        if (_bulletEffect2 == null)
        {
            return _bulletEffect1.TrailColor;
        }

        //weird way of averaging them but colors get weird when you add their parts to numbers above 1
        return (_bulletEffect1.TrailColor / 2) + (_bulletEffect2.TrailColor / 2);
    }
}
