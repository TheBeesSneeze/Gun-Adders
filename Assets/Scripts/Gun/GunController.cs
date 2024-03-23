/*******************************************************************************
* File Name :         GunController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/20/2024
*
* Brief Description : 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public ShootingMode defaultShootingMode;
    public Transform bulletSpawnPoint;
    public GameObject CurrentBulletPrefab;

    private ShootingMode currentShootMode;
    private float secondsSinceLastShoot;
    private Rigidbody playerRB;
    private Transform camera;

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
        //maybe put a sound effect here or something
    }

    /// <summary>
    /// direction is (get this) the direction the bullet goes
    /// </summary>
    private void ShootBullet(Vector3 direction)
    {
        //alec put ur code here
        var bullet = Instantiate(CurrentBulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction.normalized));
        Debug.Log("pew");
        Debug.DrawLine(bulletSpawnPoint.position, bulletSpawnPoint.position + (direction * 10), Color.white);
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
        secondsSinceLastShoot += Time.deltaTime;

        if (!InputEvents.ShootPressed) return;

        if (secondsSinceLastShoot < currentShootMode.SecondsBetweenShots) return;

        //shootin time
        secondsSinceLastShoot = 0;
        for(int i = 0; i< currentShootMode.BulletsPerShot; i++)
        {
            Vector3 angle = GetRandomizedAngle();
            ShootBullet(angle);
        }

       playerRB.AddForce(camera.forward * -1 * currentShootMode.RecoilForce, ForceMode.Impulse);
    }

    private void Start()
    {
        //InputEvents.Instance.ShootHeld.AddListener(ShootHeld);
        //InputEvents.Instance.ShootCanceled.AddListener(ShootReleased);
        playerRB = GetComponent<Rigidbody>();
        camera = Camera.main.transform;
        LoadShootingMode(defaultShootingMode);
    }

    
}
