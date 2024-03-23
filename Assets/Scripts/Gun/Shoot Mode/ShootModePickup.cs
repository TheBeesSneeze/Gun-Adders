/*******************************************************************************
 * File Name :         ShootModePickup.cs
 * Author(s) :         Toby
 * Creation Date :     3/23/2024
 *
 * Brief Description : 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootModePickup : MonoBehaviour
{
    public ShootingMode ShootMode;
    public AudioClip PickUpSound;

    private void OnTriggerEnter(Collider other)
    {
        GunController gun = other.GetComponent<GunController>();

        if (gun == null) return;

        if(PickUpSound != null)
            AudioSource.PlayClipAtPoint(PickUpSound, gun.transform.position);

        gun.LoadShootingMode(ShootMode);
        Destroy(gameObject);
    }

    void Start()
    {
        if(ShootMode == null)
        {
            Debug.LogWarning(gameObject.name + " doesnt have a shoot mode");
            Destroy (gameObject);
        }
    }
}
