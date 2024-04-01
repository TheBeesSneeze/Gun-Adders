using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputEvents : Singleton<InputEvents>
{
    // Events
    public UnityEvent MoveStarted, MoveHeld, MoveCanceled;
    public UnityEvent ShootStarted, ShootHeld, ShootCanceled;
    public UnityEvent SecondaryStarted, SecondaryHeld, SecondaryCanceled;
    public UnityEvent JumpStarted, JumpHeld, JumpCanceled;
    public UnityEvent SprintStarted, SprintHeld, SprintCanceled;
    public UnityEvent PauseStarted, PauseCanceled;
    public UnityEvent RestartStarted, RespawnStarted;

    // Input values and flags
    public Vector2 LookDelta => Look.ReadValue<Vector2>();
    public Vector3 InputDirection => movementOrigin.TransformDirection(new Vector3(InputDirection2D.x, 0f, InputDirection2D.y));
    public Vector2 InputDirection2D => Move.ReadValue<Vector2>();
    public bool MovePressed, JumpPressed, ShootPressed, SprintPressed;

    public static bool RespawnPressed, PausePressed;

    private PlayerInput playerInput;
    private InputAction Move, Shoot, Jump, Look, Sprint, Respawn, Secondary, Pause;

    private Transform movementOrigin;

    private void Start()
    {
        movementOrigin = Camera.main.transform;
        playerInput = GetComponent<PlayerInput>();
        InitializeActions();
    }

    void InitializeActions()
    {
        var map = playerInput.currentActionMap;
        Move = map.FindAction("Move");
        Jump = map.FindAction("Jump");
        Shoot = map.FindAction("Shoot");
        Look = map.FindAction("Look");
        Sprint = map.FindAction("Sprint");
        Respawn = map.FindAction("Respawn");
        Secondary = map.FindAction("Secondary");
        Pause = map.FindAction("Pause");

        // Subscribe to action events
        Move.started += ctx => ActionStarted(ref MovePressed, MoveStarted);
        Jump.started += ctx => ActionStarted(ref JumpPressed, JumpStarted);
        Shoot.started += ctx => ActionStarted(ref ShootPressed, ShootStarted);
        Sprint.started += ctx => SprintPressed = true;
        Respawn.started += ctx => { RespawnPressed = true; RespawnStarted.Invoke(); };
        Secondary.started += ctx => SecondaryStarted.Invoke();
        Secondary.canceled += ctx => SecondaryCanceled.Invoke();
        Pause.started += ctx => { PausePressed = true; PauseStarted.Invoke(); };

        Move.canceled += ctx => ActionCanceled(ref MovePressed, MoveCanceled);
        Jump.canceled += ctx => ActionCanceled(ref JumpPressed, JumpCanceled);
        Shoot.canceled += ctx => ActionCanceled(ref ShootPressed, ShootCanceled);
        Sprint.canceled += ctx => SprintPressed = false;
        Pause.canceled += ctx => { PausePressed = false; PauseCanceled.Invoke(); };
        Pause.performed += OnPause;
    }

    void ActionStarted(ref bool pressedFlag, UnityEvent actionEvent)
    {
        pressedFlag = true;
        actionEvent?.Invoke();
    }

    void ActionCanceled(ref bool pressedFlag, UnityEvent actionEvent)
    {
        pressedFlag = false;
        actionEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Pause event triggered");
            FindObjectOfType<PauseMenu>().TogglePause();
        }
    }

    private void Update()
    {
        if (MovePressed) MoveHeld.Invoke();
        if (JumpPressed) JumpHeld.Invoke();
        if (ShootPressed) ShootHeld.Invoke();
    }

    private void OnDisable()
    {
        // Unsubscribe from all action events to prevent memory leaks
        Move.started -= ctx => ActionStarted(ref MovePressed, MoveStarted);
        Jump.started -= ctx => ActionStarted(ref JumpPressed, JumpStarted);
        Shoot.started -= ctx => ActionStarted(ref ShootPressed, ShootStarted);
        Sprint.started -= ctx => SprintPressed = true;
        Respawn.started -= ctx => { RespawnPressed = true; RespawnStarted.Invoke(); };
        Secondary.started -= ctx => SecondaryStarted.Invoke();
        Secondary.canceled -= ctx => SecondaryCanceled.Invoke();
        Pause.started -= ctx => { PausePressed = true; PauseStarted.Invoke(); };

        Move.canceled -= ctx => ActionCanceled(ref MovePressed, MoveCanceled);
        Jump.canceled -= ctx => ActionCanceled(ref JumpPressed, JumpCanceled);
        Shoot.canceled -= ctx => ActionCanceled(ref ShootPressed, ShootCanceled);
        Sprint.canceled -= ctx => SprintPressed = false;
        Pause.canceled -= ctx => { PausePressed = false; PauseCanceled.Invoke(); };
        Pause.performed -= OnPause;
    }
}
