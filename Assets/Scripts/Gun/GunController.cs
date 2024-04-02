/*******************************************************************************
* File Name :         GunController.cs
* Author(s) :         Toby, Alec
* Creation Date :     3/20/2024
*
* Brief Description : 
 *****************************************************************************/

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using static AudioManager;
using static Unity.VisualScripting.Member;

public class GunController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public ShootingMode defaultShootingMode;
    [SerializeField] private GameObject BulletPrefab;
    [Header("Unity Stuff")]
    public Transform Gun;
    public Transform bulletSpawnPoint;

    [ReadOnly] public BulletEffect bulletEffect1;
    [ReadOnly] public BulletEffect bulletEffect2;
    public LayerMask scanMask;

    private float secondsSinceLastShoot;
    private Rigidbody playerRB;
    private Camera playerCamera;
    private Animator animator;

    private bool canShoot;

    /// <summary>
    /// call this in from those little pickup guys
    /// </summary>
    public void LoadShootingMode(ShootingMode shootMode)
    {
        //fuck you im going violent
        if(shootMode == null)
        {
            Debug.LogError("empty shooting mode");
            Application.Quit();
        }

        defaultShootingMode = shootMode;
        
        //change color

        Gun.GetChild(0).GetComponent<Renderer>().material.color = shootMode.GunColor;

        //maybe put a sound effect here or something

        canShoot = true;

        if(defaultShootingMode.Crosshair != null)
        {
            CrosshairScript cross = GetComponent<CrosshairScript>();
            cross.ChangeCrosshairSprite(defaultShootingMode.Crosshair);
        }
        
    }

    /// <summary>
    /// shoots all the bullets. calls the ShootBullet function
    /// </summary>
    private void Shoot()
    {
        secondsSinceLastShoot = 0;
        animator.SetTrigger("Shoot");

        Gun.TryGetComponent(out AudioSource source);
        if (source != null)
            source.Play();

        for (int i = 0; i < defaultShootingMode.BulletsPerShot; i++)
        {
            ShootBullet();
        }

        float multiplier = 1f;
        if (BulletPrefab != null)
        {
            var bullet = BulletPrefab.GetComponent<Bullet>();
            if (bullet._bulletEffect1 && bullet._bulletEffect1.PlayerRecoilMultiplier != 0)
            {
                multiplier *= bulletEffect1.PlayerRecoilMultiplier;
            }

            if (bullet._bulletEffect2 && bullet._bulletEffect2.PlayerRecoilMultiplier != 0)
            {
                multiplier *= bulletEffect2.PlayerRecoilMultiplier;
            }
        }
        playerRB.AddForce(-playerCamera.transform.forward * (defaultShootingMode.RecoilForce * multiplier), ForceMode.Impulse);
    }

    /// <summary>
    /// direction is (get this) the direction the bullet goes
    /// </summary>
    private void ShootBullet()
    {
        //alec put code here
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(.5f, 0.5f, 0f));
        Vector3 destination;
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, scanMask))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(100f);
        }

        destination += new Vector3(
            Random.Range(-defaultShootingMode.BulletAccuracyOffset, defaultShootingMode.BulletAccuracyOffset),
            Random.Range(-defaultShootingMode.BulletAccuracyOffset, defaultShootingMode.BulletAccuracyOffset),
            Random.Range(-defaultShootingMode.BulletAccuracyOffset, defaultShootingMode.BulletAccuracyOffset));
        Vector3 dir = destination - bulletSpawnPoint.position;
        var bullet = Instantiate(BulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.forward = dir.normalized;
        var bulletObj = bullet.GetComponent<Bullet>();
        bulletObj.damageAmount = defaultShootingMode.BulletDamage;
        bulletObj.bulletForce = defaultShootingMode.BulletSpeed;
        bulletObj.GetComponent<Rigidbody>().velocity = playerRB.GetPointVelocity(bulletSpawnPoint.position);
        bulletObj.Initialize(bulletEffect1, bulletEffect2, dir);
        
        
        
    }
    
    private void Update()
    {
        if (PauseMenu.IsPaused)
            return;

        DebugTarget();
        secondsSinceLastShoot += Time.deltaTime;

        if (!canShoot) return;
        if (!InputEvents.Instance.ShootPressed) return;
        if (secondsSinceLastShoot < (60f / defaultShootingMode.RPM)) return;

        if (!defaultShootingMode.HoldFire)
            canShoot = false;

        //shootin time
        Shoot();
    }

    private void OnShootStart()
    {
        canShoot = true;
    }

    private void DebugTarget()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(.5f, 0.5f, 0f));
        Vector3 destination;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, LayerMask.GetMask("Default")))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000f);
        }
        Debug.DrawLine(ray.origin, destination, Color.red);
        Debug.DrawLine(bulletSpawnPoint.position, destination, Color.red);
    }

    private void Start()
    {
        //InputEvents.Instance.ShootHeld.AddListener(ShootHeld);
        //InputEvents.Instance.ShootCanceled.AddListener(ShootReleased);
        playerRB = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        animator = GetComponent<Animator>();
        LoadShootingMode(defaultShootingMode);
        InputEvents.Instance.ShootStarted.AddListener(OnShootStart);
    }

    
}
