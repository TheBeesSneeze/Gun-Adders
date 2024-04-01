using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletEffectTextScript : MonoBehaviour
{
    public GunController gun;
    public TextMeshPro effect1;
    public TextMeshPro effect2;
    public void Update()
    {
        if (gun.bulletEffect1 != null) effect1.text = "1: " + gun.bulletEffect1.UpgradeName;
        if (gun.bulletEffect2 != null) effect2.text = "2: " + gun.bulletEffect2.UpgradeName;
    }
}
