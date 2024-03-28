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

public class GunController : MonoBehaviour
{
    public ShootingMode defaultShootingMode;
    public Transform bulletSpawnPoint;
    [SerializeField] private GameObject BulletPrefab;
    public Transform Gun;

    [ReadOnly] public BulletEffect bulletEffect1;
    [ReadOnly] public BulletEffect bulletEffect2;
    public LayerMask scanMask;

    private ShootingMode currentShootMode;
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

        currentShootMode = shootMode;
        
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
            Random.Range(-currentShootMode.BulletAccuracyOffset, currentShootMode.BulletAccuracyOffset),
            Random.Range(-currentShootMode.BulletAccuracyOffset, currentShootMode.BulletAccuracyOffset),
            Random.Range(-currentShootMode.BulletAccuracyOffset, currentShootMode.BulletAccuracyOffset));
        Vector3 dir = destination - bulletSpawnPoint.position;
        var bullet = Instantiate(BulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.forward = dir.normalized;
        bullet.GetComponent<Bullet>().damageAmount = currentShootMode.BulletDamage;
        bullet.GetComponent<Bullet>().bulletForce = currentShootMode.BulletSpeed;
        bullet.GetComponent<Bullet>().Initialize(bulletEffect1, bulletEffect2, dir);
    }
    
    private Vector3 GetRandomizedAngle()
    {
        float a = currentShootMode.BulletAccuracyOffset / 90;
        float x = Random.Range(-a, a);
        float y = Random.Range(-a, a);

        Vector3 angle = bulletSpawnPoint.forward;
        angle = new Vector3 (angle.x + x, angle.y + y, angle.z);

        return angle;
    }
    /*
    private void ShootHeld()
    {

    }

    private void ShootReleased()
    {

    }
    */

    private void Update()
    {
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

        if (secondsSinceLastShoot < (60f / currentShootMode.RPM)) return;

        //shootin time
        secondsSinceLastShoot = 0;
        for(int i = 0; i< currentShootMode.BulletsPerShot; i++)
        {
            ShootBullet();
        }

       playerRB.AddForce(-camera.transform.forward * currentShootMode.RecoilForce, ForceMode.Impulse);
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
