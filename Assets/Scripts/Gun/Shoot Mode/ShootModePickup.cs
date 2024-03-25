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

public class ShootModePickup : UpgradePickupType
{
    public ShootingMode[] ShootModes;
    
    protected override void PickUp(GunController gun)
    {
        int randomIndex = Random.Range(0, ShootModes.Length);
        gun.LoadShootingMode(ShootModes[randomIndex]);

        base.PickUp(gun);
    }

    protected override void Start()
    {
        base.Start(); 
        if(ShootModes.Length < 1)
        {
            Debug.LogWarning(gameObject.name + " doesnt have shoot modes");
            Destroy (gameObject);
        }
    }
}
