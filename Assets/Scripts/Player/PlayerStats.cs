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
    [Tooltip("speed MULTIPLIER while sprinting")]
    public float SprintSpeed;
    //[Tooltip("how much velocity is carried over from one frame to another")]
    //public float PlayerSlipperyness; //@TODO

    public float JumpForce;

    [Tooltip("m/s of gravity")]
    public float NormalGravitySpeed; //@TODO. might get cut. maybe stupid of me
    [Tooltip("gravity when player is holding jump key")]
    public float JumpingGravitySpeed; //@TODO. might get cut also.
}
