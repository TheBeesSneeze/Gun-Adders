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
    public AudioClip PickUpSound;
    public TMP_Text UpgradeText;
    public RectTransform BackgroundImage;

    private Color defaultColor;
    private PlayerBehaviour player;

    //debug only
    [ReadOnly] public bool Disabled;

    protected virtual void PickUp(GunController gun)
    {
        if (PickUpSound != null)
            AudioSource.PlayClipAtPoint(PickUpSound, gun.transform.position);

        //Destroy(gameObject);
        UpgradeText.enabled = false;
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
        Disabled = true;

        yield return new WaitForSeconds(DisabledSeconds);

        GetComponent<Renderer>().material.color = defaultColor;
        GetComponent<Collider>().enabled = true;
        LoadNewUpgrade();

        Disabled = false;
    }

    protected virtual void Start()
    {
        defaultColor = GetComponent<Renderer>().material.color;
        LoadNewUpgrade();
        player = GameObject.FindObjectOfType<PlayerBehaviour>();
    }

    private void Update()
    {
        BackgroundImage.transform.LookAt(player.transform.position);
    }
}
