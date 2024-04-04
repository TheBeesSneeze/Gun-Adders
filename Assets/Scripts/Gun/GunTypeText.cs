using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunTypeText : MonoBehaviour
{
    public GunController gun;
    public TextMeshPro tb;
    public void Update()
    {
        tb.text = gun.shootingMode.GunName;
    }
}
