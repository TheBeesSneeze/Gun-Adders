/*******************************************************************************
* File Name :         BulletEffectPickup.cs
* Author(s) :         Alec, Toby
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

    private Vector3 initPosition;
    protected override void Start()
    {
        base.Start();
        initPosition = transform.position;
    }

    /// <summary>
    /// bruh moment
    /// </summary>
    private void LateUpdate()
    {
        var pos = transform.position;
        pos.y = initPosition.y + Mathf.Sin(Time.time * 1.5f) * .25f;
        transform.position = pos;

        var rot = transform.eulerAngles;
        rot.y += 25f * Time.deltaTime;
        transform.eulerAngles = rot;
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

        if( UnityEngine.Random.value > 0.5f)
        {
            gun.bulletEffect1 = loadedUpgrade;
        }
        else
        {
            gun.bulletEffect2 = loadedUpgrade;
        }
    }

    protected override void LoadNewUpgrade()
    {
        BulletEffect newUpgrade;

        //make sure new upgrade isnt same as last time
        do
        {
            newUpgrade = UpgradePool[UnityEngine.Random.Range(0, UpgradePool.Length)];
        }
        while (newUpgrade == loadedUpgrade); //DO WHILE DO WHILE DO WHILE

        loadedUpgrade = newUpgrade;
    }
}
