/*******************************************************************************
 * File Name :         PlayerControler.cs
 * Author(s) :         Toby
 * Creation Date :     3/18/2024
 *
 * Brief Description : responds to input events. 
 *****************************************************************************/

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

    /// <summary>
    /// every frame while move is held
    /// </summary>
    private void ManageMovement()
    {
        if (!InputEvents.Instance.MovePressed) return;

        Vector3 targetV = InputEvents.Instance.InputDirection.normalized * stats.Speed;
        targetV.y = rb.velocity.y;

        Vector3 force = targetV - rb.velocity;

        if (float.IsNaN(force.x) || float.IsNaN(force.y) || float.IsNaN(force.z))
            force = Vector3.zero;

        rb.AddForce(force);
    }

    private void JumpStarted()
    {
        rb.AddForce(0, stats.JumpForce, 0,ForceMode.Impulse);
    }

    private void FixedUpdate()
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

        yMovement += InputEvents.Instance.LookDelta.y * stats.Sensitivity * Time.fixedDeltaTime;
        xMovement += InputEvents.Instance.LookDelta.x * stats.Sensitivity * Time.fixedDeltaTime;
        yMovement = Mathf.Clamp(yMovement, -90, 90);
        cam.transform.localEulerAngles = new Vector3(-yMovement, xMovement, 0f);
    }

    // Start is called every frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<PlayerStats>();
        cam = Camera.main.transform;

        AssignEventListeners();
    }
    
    private void AssignEventListeners()
    {
        InputEvents.Instance.MoveHeld.AddListener( ManageMovement );
        InputEvents.Instance.JumpStarted.AddListener( JumpStarted );
    }
}
