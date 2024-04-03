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

    [Tooltip ("Movement speed. legs.")]
    public float Speed;
    [Tooltip("How much simulated friction to prevent you from moving forward when stopping input")]
    public float Friction = 0.175f;

    [Tooltip("What percent of normal movement will get applied whilst moving in the air")]
    public float AirMovementMultiplier = 0.5f;

    [Tooltip("The max target speed")]
    public float MaxSpeed = 20f;

    [Tooltip("How much player regens every second.")]
    public int HealthRegen = 1;

    [Tooltip("Seconds until player heals.")]
    public int SecondsUntilHealing = 2;

    [Header("Jumps")]
    //[Tooltip("how much velocity is carried over from one frame to another")]
    //public float PlayerSlipperyness; //@TODO
    public int AirJumps = 1;

    [Tooltip("How high the player jumps")]
    public float JumpHeight = 2.5f;


    [Tooltip("How much gravity to apply to the player. Normal gravity is not used.")]
    public float GravityBoost = 10f;
}
