/*******************************************************************************
 * File Name :         InputEvents.cs
 * Author(s) :         Toby
 * Creation Date :     3/18/2024
 *
 * Brief Description : 
 * Listens for player inputs and invokes events. pressed, unpressed, held (every frame)
 * contains bools for if an input is being held.
 * contains values for inputs that have those (vector2 for move, etc).
 * calculates player in 3d space respective to camera
 * calculates camera direction (does not apply it)
 * 
 * singleton.
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//good amount of this code taken from fish splatters lol
public class InputEvents : Singleton<InputEvents>
{
    //most of these wont get used haha
    public UnityEvent MoveStarted;
    public UnityEvent MoveHeld;
    public UnityEvent MoveCanceled;

    public UnityEvent ShootStarted;
    public UnityEvent ShootHeld;
    public UnityEvent ShootCanceled;

    public UnityEvent SecondaryStarted;
    public UnityEvent SecondaryHeld;
    public UnityEvent SecondaryCanceled;

    public UnityEvent JumpStarted;
    public UnityEvent JumpHeld;
    public UnityEvent JumpCanceled;

    public UnityEvent SprintStarted;
    public UnityEvent SprintHeld;

    public UnityEvent PauseStarted; //@TODO
    public UnityEvent RestartStarted; //@TODO //re start start ed

    public UnityEvent RespawnStarted; 

    [HideInInspector] public Vector2 LookDelta { get { return Look.ReadValue<Vector2>(); } }
    [HideInInspector] public Vector3 InputDirection { get { return movementOrigin.TransformDirection(new Vector3(InputDirection2D.x, 0f, InputDirection2D.y)); } }
    [HideInInspector] public Vector2 InputDirection2D { get { return Move.ReadValue<Vector2>(); } }
    [HideInInspector] public bool MovePressed;
    [HideInInspector] public bool JumpPressed;
    [HideInInspector] public bool ShootPressed;
    [HideInInspector] public bool SprintPressed;

    [HideInInspector] public static bool RespawnPressed; 

    //actions
    private PlayerInput playerInput;
    private InputAction Move;
    private InputAction Shoot;
    private InputAction Jump;
    private InputAction Look;
    private InputAction Sprint;
    private InputAction Respawn;
    private InputAction Secondary;

    //stuff and things
    private Transform movementOrigin; // camera.main

    private void Start()
    {
        movementOrigin = Camera.main.transform;

        playerInput = GetComponent<PlayerInput>();
        playerInput.currentActionMap.Enable();

        Move = playerInput.currentActionMap.FindAction("Move");
        Jump = playerInput.currentActionMap.FindAction("Jump");
        Shoot = playerInput.currentActionMap.FindAction("Shoot");
        Look = playerInput.currentActionMap.FindAction("Look");
        Sprint = playerInput.currentActionMap.FindAction("Sprint");
        Respawn = playerInput.currentActionMap.FindAction("Respawn");
        Secondary = playerInput.currentActionMap.FindAction("Secondary");

        Move .started += context => { MovePressed = true;  MoveStarted.Invoke();  };
        Jump .started += context => { JumpPressed = true;  JumpStarted.Invoke();  };
        Shoot.started += context => { ShootPressed = true; ShootStarted.Invoke(); };
        Sprint.started += context => { SprintPressed = true; };
        Respawn.started += context => { RespawnPressed = true; RespawnStarted.Invoke(); };
        Secondary.started += context => { SecondaryStarted.Invoke(); };
        Secondary.canceled += context => { SecondaryCanceled.Invoke(); };
        /*
        Move.performed += context => { MoveHeld.Invoke(); };
        Jump.performed += context => { JumpHeld.Invoke(); };
        Shoot.performed += context => { ShootHeld.Invoke(); };
        Sprint.performed += context => { SprintPressed = true; };
        */
        Move .canceled += context => { MovePressed = false;  MoveCanceled.Invoke();  };
        Jump .canceled += context => { JumpPressed = false;  JumpCanceled.Invoke();  };
        Shoot.canceled += context => { ShootPressed = false; ShootCanceled.Invoke(); };
        Sprint.canceled += context => { SprintPressed = false; };
    }
    /// <summary>
    /// im ashamed of this code but i couldnt get [input].performed to work so HERE WE ARE
    /// </summary>
    private void Update()
    {
        if(MovePressed)
            MoveHeld.Invoke();

        if (JumpPressed)
            JumpHeld.Invoke();

        if (ShootPressed)
            ShootHeld.Invoke();

        
    }
    private void OnDisable()
    {
        //Debug.LogWarning("i got lazy and didnt make an ondisable function hopefully nothing bad happens");
    }
}
