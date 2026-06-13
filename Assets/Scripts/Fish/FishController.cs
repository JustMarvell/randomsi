using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FishController : MonoBehaviour
{
    [Header("Movement")]
    public float swimSpeed = 4f;
    public float sprintSpeed = 8f;
    public float verticalSpeed = 3f;
    public float turnSpeed = 90f;
    public float pitchSpeed = 60f;
    public float acceleration = 5f;
    public float avoidDistance = 2f;
    public float avoidStrength = 1f;
    public LayerMask terrainMask;

    [Header("Water")]
    public float seaLevel = 0f;
    public float jumpExitVelocityThreshold = 6f;

    [Header("Refs")]
    public Rigidbody rb;
    public PlayerInput playerInput;
    public FishCameraRig cameraRig;

    InputAction moveAction;
    InputAction ascendAction;
    InputAction descendAction;
    InputAction sprintAction;

    public FishStateMachine StateMachine { get; private set; }
    FishIdleState idleState;
    FishSwimState swimState;
    FishSprintState sprintState;
    FishAirborneState airborneState;
    FishLandState landState;

    public bool IsUnderwater => transform.position.y < seaLevel;

    public FishBaseState AirborneState => airborneState;
    public FishBaseState LandState => landState;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        moveAction = playerInput.actions["Move"];
        ascendAction = playerInput.actions["Jump"];
        descendAction = playerInput.actions["Descend"];
        sprintAction = playerInput.actions["Sprint"];

        StateMachine = new FishStateMachine();
        idleState = new FishIdleState(this);
        swimState = new FishSwimState(this);
        sprintState = new FishSprintState(this);
        airborneState = new FishAirborneState(this);
        landState = new FishLandState(this);
    }

    void Start()
    {
        StateMachine.Initialize(idleState);
    }

    void Update() => StateMachine.Tick(Time.deltaTime);
    void FixedUpdate() => StateMachine.FixedTick(Time.fixedDeltaTime);

    public Vector2 MoveInput => moveAction.ReadValue<Vector2>();
    public bool AscendHeld => ascendAction.IsPressed();
    public bool DescendHeld => descendAction.IsPressed();
    public bool SprintHeld => sprintAction.IsPressed();

    public FishBaseState IdleState => idleState;
    public FishBaseState SwimState => swimState;
    public FishBaseState SprintState => sprintState;

    // Moves and rotates body relative to camera-facing direction
    public void ApplyMovement(float targetSpeed)
    {
        Vector2 move = MoveInput;
        Vector3 dir = cameraRig.Forward * move.y + cameraRig.Right * move.x;

        float vertical = 0f;
        if (AscendHeld) vertical += 1f;
        if (DescendHeld) vertical -= 1f;
        dir += Vector3.up * vertical;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.fixedDeltaTime * Mathf.Deg2Rad);

            if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, avoidDistance, terrainMask))
            {
                dir = Vector3.Slerp(dir.normalized, hit.normal, avoidStrength).normalized;
            }
        }

        Vector3 targetVel = dir.normalized * targetSpeed;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVel, acceleration * Time.fixedDeltaTime);

        if (transform.position.y > controller.seaLevel && rb.linearVelocity.y > 0 && rb.linearVelocity.y < controller.jumpExitVelocityThreshold)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            Vector3 pos = transform.position;
            pos.y = controller.seaLevel;
            transform.position = pos;
        }
    }
}