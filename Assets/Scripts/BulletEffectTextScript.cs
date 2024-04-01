using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletEffectTextScript : MonoBehaviour
{
    public TextMeshPro effect1;
    public TextMeshPro effect2;

    private GunController gun;

    private void Start()
    {
        gun = GetComponent<GunController>();
    }
    public void Update()
    {
        if (gun.bulletEffect1 != null) effect1.text = "1: " + gun.bulletEffect1.UpgradeName;
        if (gun.bulletEffect2 != null) effect2.text = "2: " + gun.bulletEffect2.UpgradeName;
    }
}
