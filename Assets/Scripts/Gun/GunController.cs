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
    private Camera camera;

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

        Gun.GetComponent<Renderer>().material.color = shootMode.GunColor;

        //maybe put a sound effect here or something
    }

    /// <summary>
    /// direction is (get this) the direction the bullet goes
    /// </summary>
    private void ShootBullet()
    {
        //alec put code here
        Ray ray = camera.ViewportPointToRay(new Vector3(.5f, 0.5f, 0f));
        Vector3 destination;
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, scanMask))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000f);
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
        bulletObj.Initialize(bulletEffect1, bulletEffect2, dir);
        if(instance != null)
            instance.Play("Shoot Default");
    }
    
    private void Update()
    {
        if (PauseMenu.IsPaused)
            return;

        Ray ray = camera.ViewportPointToRay(new Vector3(.5f, 0.5f, 0f));
        Vector3 destination;
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, LayerMask.GetMask("Default")))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000f);
        }
        Debug.DrawLine(ray.origin, destination, Color.red);
        Debug.DrawLine(bulletSpawnPoint.position, destination, Color.red);
        secondsSinceLastShoot += Time.deltaTime;

        if (!InputEvents.Instance.ShootPressed) return;

        if (secondsSinceLastShoot < (60f / defaultShootingMode.RPM)) return;

        //shootin time
        secondsSinceLastShoot = 0;
        for(int i = 0; i< defaultShootingMode.BulletsPerShot; i++)
        {
            ShootBullet();
        }

       playerRB.AddForce(-camera.transform.forward * defaultShootingMode.RecoilForce, ForceMode.Impulse);
    }

    private void Start()
    {
        //InputEvents.Instance.ShootHeld.AddListener(ShootHeld);
        //InputEvents.Instance.ShootCanceled.AddListener(ShootReleased);
        playerRB = GetComponent<Rigidbody>();
        camera = Camera.main;
        LoadShootingMode(defaultShootingMode);
    }

    
}
