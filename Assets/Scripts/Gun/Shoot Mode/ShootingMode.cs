/*******************************************************************************
* File Name :         ShootingMode.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/21/2024
*
* Brief Description : scriptable object that determines how the gun works
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShootingMode", menuName = "ShootingMode")]
public class ShootingMode : ScriptableObject
{
    [Header("Display")]
    public string GunName;
    public Image Crosshair;
    public Color GunColor = Color.white;
    public Sprite gunIcon;
    [Tooltip("RPM of bullets shot")] public float RPM = 120f;
    [Tooltip("# of bullets shot at one time (imagine a shotgun)")]
    public int BulletsPerShot=1; 
    [Tooltip("(angle) How much to randomize angle (0 is perfect precision)")]
    public float BulletAccuracyOffset=0;
    [Tooltip("The player can hold click to repeat fire")]
    public bool HoldFire=true; //@TODO
    [Tooltip("Speed of the projectile itself (units/second)")]
    public float BulletSpeed=10; //@TODO
    public float BulletDamage=1; //@TODO
    public float RecoilForce;

}
