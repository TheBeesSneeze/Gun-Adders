/*******************************************************************************
* File Name :         UpgradePickupType.cs
* Author(s) :         Toby
* Creation Date :     3/25/2024
*
* Brief Description : base class for those little upgrade pickup guys
 *****************************************************************************/
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradePickupType : MonoBehaviour
{
    public float DisabledSeconds;
    public float DisabledOpacity;
    public TMP_Text UpgradeText;
    public Transform BackgroundImage;
    public GameObject upgradeEffect;

    private Color defaultColor;
    private Transform playerPoint;
    private Vector3 initPosition;

    //debug only
    [ReadOnly] public bool Disabled;

    protected virtual void PickUp(GunController gun)
    {
        AudioManager.instance.Play("Pickup");

        if (upgradeEffect)
            Instantiate(upgradeEffect, gun.GetComponent<PlayerBehaviour>().transform);

        //Destroy(gameObject);
        StartCoroutine(DisablePickup());
    }

    private void OnTriggerEnter(Collider other)
    {
        GunController gun = other.GetComponent<GunController>();

        if (gun == null) return;

        PickUp(gun);
    }

    protected virtual void LoadNewUpgrade()
    {
        Debug.LogWarning("override this function");
    }

    protected IEnumerator DisablePickup()
    {
        Color c = defaultColor;
        c = c / 2;
        c.a = DisabledOpacity;
        GetComponent<Renderer>().material.color = c;
        GetComponent<Collider>().enabled = false;
        BackgroundImage.gameObject.SetActive(false);
        Disabled = true;

        yield return new WaitForSeconds(DisabledSeconds);

        GetComponent<Renderer>().material.color = defaultColor;
        GetComponent<Collider>().enabled = true;
        BackgroundImage.gameObject.SetActive(true);
        LoadNewUpgrade();

        Disabled = false;
    }

    protected virtual void Start()
    {
        defaultColor = GetComponent<Renderer>().material.color;
        LoadNewUpgrade();
        playerPoint = Camera.main.transform;
        initPosition = transform.localPosition;
    }

    private void Update()
    {
        if (BackgroundImage == null) return;

        BackgroundImage.transform.LookAt(playerPoint.position);
        BackgroundImage.transform.Rotate(0, 180, 0);
    }

    /// <summary>
    /// bruh moment
    /// </summary>
    private void LateUpdate()
    {

        var pos = transform.localPosition;
        pos.y = initPosition.y + Mathf.Sin(Time.time * 1.5f) * .25f;
        transform.localPosition = pos;

        var rot = transform.eulerAngles;
        rot.y += 25f * Time.deltaTime;
        transform.eulerAngles = rot;

    }
}
