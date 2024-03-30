/*******************************************************************************
* File Name :         BulletEffectPickup.cs
* Author(s) :         Alec, Toby, SKy
* Creation Date :     
*
* Brief Description : 
 *****************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectPickup : UpgradePickupType
{
    //public GameObject BulletPrefab;
    public BulletEffect[] UpgradePool;
    private BulletEffect loadedUpgrade;


    protected override void Start()
    {
        base.Start();
    }

    protected override void PickUp(GunController gun)
    {
        base.PickUp(gun);

        Debug.Log("loaded " + loadedUpgrade.UpgradeName);

        if (gun.bulletEffect1 == null)
        {
            gun.bulletEffect1 = loadedUpgrade;
            return;
        }

        if (gun.bulletEffect2 == null)
        {
            gun.bulletEffect2 = loadedUpgrade;
            return;
        }

        if (UnityEngine.Random.value > 0.5f)
        {
            gun.bulletEffect1 = loadedUpgrade;
        }
        else
        {
            gun.bulletEffect2 = loadedUpgrade;
        }

        UpgradeText.text = loadedUpgrade.UpgradeName;
    }

    protected override void LoadNewUpgrade()
    {
        BulletEffect newUpgrade;

        //make sure new upgrade isnt same as last time
        if (UpgradePool.Length > 1)
        {
            do
            {
                newUpgrade = UpgradePool[UnityEngine.Random.Range(0, UpgradePool.Length)];
            } while (newUpgrade == loadedUpgrade); //DO WHILE DO WHILE DO WHILE
        }
        else
        {
            newUpgrade = UpgradePool[0];
        }

        loadedUpgrade = newUpgrade;

        UpgradeText.enabled = true;
        UpgradeText.text = loadedUpgrade.UpgradeName;
    }
}