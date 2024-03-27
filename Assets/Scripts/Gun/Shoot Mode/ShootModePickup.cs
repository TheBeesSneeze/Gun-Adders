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
using NaughtyAttributes;

public class ShootModePickup : UpgradePickupType
{
    public ShootingMode[] ShootModes;
    [ReadOnly] public ShootingMode loadedShootMode;
    
    protected override void PickUp(GunController gun)
    {
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

    protected override void LoadNewUpgrade()
    {
        ShootingMode newUpgrade;

        //make sure new upgrade isnt same as last time
        do
        {
            newUpgrade = ShootModes[UnityEngine.Random.Range(0, ShootModes.Length)];
        }
        while (newUpgrade == loadedShootMode); //DO WHILE DO WHILE DO WHILE

        loadedShootMode = newUpgrade;
    }
}
