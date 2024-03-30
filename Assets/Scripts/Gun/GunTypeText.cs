using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunTypeText : MonoBehaviour
{
    public GunController gun;
    public TextMeshPro tb;
    /*public GameObject poisonImage;
    public GameObject freezeImage;
    public GameObject explotionImage;
    public GameObject windImage;
    public GameObject lightningImage;
    public GameObject pierceImage;*/
    public void Start()
    {
        
    }
    public void Update()
    {
        tb.text = gun.defaultShootingMode.GunName;
    }
}
