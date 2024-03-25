using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectPickup : MonoBehaviour
{
    public GameObject BulletPrefab;
    public AudioClip PickupSound;
    private Vector3 initPosition;
    private void Start()
    {
        initPosition = transform.position;
    }

    private void LateUpdate()
    {
        var pos = transform.position;
        pos.y = initPosition.y + Mathf.Sin(Time.time * 1.5f) * .25f;
        transform.position = pos;

        var rot = transform.eulerAngles;
        rot.y += 25f * Time.deltaTime;
        transform.eulerAngles = rot;
    }

    private void OnTriggerEnter(Collider other)
    {
        GunController gun = other.GetComponent<GunController>();
        if (gun == null) return;
        gun.CurrentBulletPrefab = BulletPrefab;

        if (PickupSound != null)
        {
            AudioSource.PlayClipAtPoint(PickupSound, transform.position);
        }
        
        Destroy(gameObject);
    }
}
