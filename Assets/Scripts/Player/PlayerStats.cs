/*******************************************************************************
 * File Name :         PlayerStats.cs
 * Author(s) :         Toby
 * Creation Date :     3/18/2024
 *
 * Brief Description : its like a scriptable object. but not!
 * have fun designers!
 * might be a bit excessive, but i want small scripts. and variables take up space!
 * 
 * gun stats should be somewhere else
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float DefaultHealth; 

    public float Sensitivity = 100;

    [Tooltip ("Movement speed. legs.")]
    public float Speed;
    public float Friction = 0.175f;

    public float AirMovementMultiplier = 0.5f;
    //[Tooltip("how much velocity is carried over from one frame to another")]
    //public float PlayerSlipperyness; //@TODO
    public float JumpHeight = 2.5f;

    public float MaxSpeed = 20f;

    [Tooltip("m/s of gravity")] public float GravityBoost = 10f;
    [Tooltip("gravity when player is holding jump key")]
    public float JumpingGravitySpeed; //@TODO. might get cut also.
}
