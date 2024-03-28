/*******************************************************************************
 * File Name :         PlayerControler.cs
 * Author(s) :         Toby, Tyler
 * Creation Date :     3/18/2024
 *
 * Brief Description : responds to input events.
 *****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerControler : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform camera;
    [SerializeField] private Transform playerOrientationTracker;
    [SerializeField] private Transform cameraFollowPoint;
    private Vector2 input;
    private Rigidbody rb;
    private PlayerStats stats;

    private float xMovement;
    private float yMovement;

    [Tooltip("The player prefabs feet script")]
    public FeetScript feet;

    public int airJumps = 1;
    private int airJumpCounter;
    private bool jumping;

    /// <summary>
    /// every frame while move is held
    /// </summary>
    private void FixedUpdate()
    {
        DoMovement();
    }

    private void Update()
    {
        UpdateCamera();
        if (feet.Grounded)
        {
            airJumpCounter = airJumps;
        }
        input = InputEvents.Instance.InputDirection2D;
    }

    private void DoMovement()
    {
        rb.AddForce(Vector3.down * stats.GravityBoost, ForceMode.Acceleration);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        ApplyCounterMovement(input.x, input.y, mag);

        float maxSpeed = stats.MaxSpeed;

        if (input.x > 0 && xMag > maxSpeed) input.x = 0;
        if (input.x < 0 && xMag < -maxSpeed) input.x = 0;
        if (input.y > 0 && yMag > maxSpeed) input.y = 0;
        if (input.y < 0 && yMag < -maxSpeed) input.y = 0;

        float multiplier = 1f;

        if (!feet.Grounded)
        {
            multiplier = 0.5f;
        }

        //Apply forces to move player
        rb.AddForce(playerOrientationTracker.forward * (input.y * stats.Speed * Time.deltaTime * multiplier));
        rb.AddForce(playerOrientationTracker.right * (input.x * stats.Speed * Time.deltaTime * multiplier));
    }

    private void Jump()
    {
        jumping = true;
        if (airJumpCounter <= 0)
        {
            return;
        }

        airJumpCounter--;

        var grav = (Vector3.down * stats.GravityBoost * rb.mass).magnitude;

        rb.AddForce(
            Vector3.up * (Mathf.Sqrt(2 * stats.JumpHeight * grav)), ForceMode.Impulse);
        rb.AddForce(
                feet.GroundNormal * (Mathf.Sqrt(2 * stats.JumpHeight * grav) * 0.5f),
                ForceMode.Impulse);
    }

    private void ResetJump()
    {
        jumping = false;
    }

    private void ApplyCounterMovement(float x, float y, Vector2 mag)
    {
        if (!feet.Grounded || jumping) return;

        if (Math.Abs(mag.x) > 0.01f && Math.Abs(x) < 0.05f || (mag.x < -0.01f && x > 0) || (mag.x > 0.01f && x < 0))
        {
            rb.AddForce(
                playerOrientationTracker.right * (stats.Speed * Time.deltaTime * -mag.x * stats.Friction));
        }

        if (Math.Abs(mag.y) > 0.01f && Math.Abs(y) < 0.05f || (mag.y < -0.01f && y > 0) || (mag.y > 0.01f && y < 0))
        {
            rb.AddForce(
                playerOrientationTracker.forward * (stats.Speed * Time.deltaTime * -mag.y * stats.Friction));
        }

        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > stats.MaxSpeed)
        {
            float verticalVelocity = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * stats.MaxSpeed;
            rb.velocity = new Vector3(n.x, verticalVelocity, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = playerOrientationTracker.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float mag = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        float yMag = mag * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = mag * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void UpdateCamera()
    {
        var mouse = InputEvents.Instance.LookDelta;
        float mouseX = mouse.x * stats.Sensitivity * Time.fixedDeltaTime;
        float mouseY = mouse.y * stats.Sensitivity * Time.fixedDeltaTime;
        Vector3 rot = cameraHolder.localRotation.eulerAngles;
        xMovement = rot.y + mouseX;
        yMovement -= mouseY;
        yMovement = Mathf.Clamp(yMovement, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(yMovement, xMovement, 0);
        cameraHolder.position = cameraFollowPoint.position;
        playerOrientationTracker.localRotation = Quaternion.Euler(0, xMovement, 0);
    }

    // Start is called every frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        stats = GetComponent<PlayerStats>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraHolder.transform.parent = null;
        InputEvents.Instance.JumpStarted.AddListener(Jump);
        InputEvents.Instance.JumpStarted.AddListener(ResetJump);
        airJumpCounter = airJumps;
    }
}