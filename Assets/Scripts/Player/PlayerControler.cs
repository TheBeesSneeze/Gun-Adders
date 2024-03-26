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
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerControler : MonoBehaviour
{
    private Rigidbody   rb;
    private PlayerStats stats;
    private Transform   cam;

    private float xMovement;
    private float yMovement;

    [Tooltip("The player prefabs feet script")]
    public FeetScript feet;

    public int airJumps = 1;
    private int airJumpCounter;
    private Vector3 cameraPos;
    public Transform cameraHolder;
    public Transform cameraTrackPoint;
    public Transform orientationTracker;
    /// <summary>
    /// every frame while move is held
    /// </summary>
    private void FixedUpdate()
    {
        //if (!InputEvents.MovePressed) return;
        var input = InputEvents.Instance.InputDirection.normalized;
        float goalSpeed = InputEvents.Instance.SprintPressed ? stats.SprintSpeed : stats.Speed;

        bool grounded = feet.touchingGround;
        float airMod = grounded ? 1f : 0.5f;
        
        Vector3 targetV = input * (goalSpeed * airMod);
        targetV.y = rb.velocity.y;
        Vector3 force = targetV - rb.velocity;

        if (float.IsNaN(force.x) || float.IsNaN(force.y) || float.IsNaN(force.z))
            force = Vector3.zero;

        rb.AddForce(force, ForceMode.VelocityChange);
    }
    
    private void JumpStarted()
    {
        if (feet.touchingGround)
        {
            rb.AddForce(0, stats.JumpForce, 0, ForceMode.Impulse);
            airJumpCounter = airJumps;
            return;
        }
        if(airJumpCounter > 0) 
        {
            rb.AddForce(0, stats.JumpForce, 0, ForceMode.Impulse);
            airJumpCounter--;
            return;
        }
    }

    private void Update()
    {
        UpdateCamera();
    }
    private void UpdateCamera()
    {
        /*
        Vector2 delta = InputEvents.Instance.LookDelta;
        Vector3 lookRotation = cam.eulerAngles;

        lookRotation.x += delta.y * stats.Sensitivity * Time.fixedDeltaTime * -1;
        lookRotation.y += delta.x * stats.Sensitivity * Time.fixedDeltaTime;

        lookRotation.x -= 360;
        //lookRotation.x = Mathf.Clamp(lookRotation.x, 80);

        cam.eulerAngles = lookRotation;

        Debug.Log(lookRotation.x);
        */
        Vector2 mouse = InputEvents.Instance.LookDelta;
        float mouseX = mouse.x * stats.Sensitivity * Time.fixedDeltaTime;
        float mouseY = mouse.y * stats.Sensitivity * Time.fixedDeltaTime;
        Vector3 rot = cameraHolder.transform.localRotation.eulerAngles;
        xMovement = rot.y + mouseX;
        yMovement -= mouseY;
        yMovement = Mathf.Clamp(yMovement, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(yMovement,xMovement , 0f);
        orientationTracker.localRotation = Quaternion.Euler(0f, yMovement, 0f);
        cameraHolder.transform.position = cameraTrackPoint.position;
    }
    // Start is called every frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<PlayerStats>();
        cam = Camera.main.transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraPos = cam.transform.localPosition;
        cameraHolder.transform.parent = null;
        airJumpCounter = airJumps;
        AssignEventListeners();
    } 
    private void AssignEventListeners()
    {
       // InputEvents.Instance.MoveHeld.AddListener( ManageMovement );
        InputEvents.Instance.JumpStarted.AddListener( JumpStarted );
    }
}
