using UnityEngine;
using UnityEngine.InputSystem;

public class FishCameraRig : MonoBehaviour
{
    public Transform target;
    public PlayerInput playerInput;
    public float lookSpeed = 120f;
    public float distance = 6f;
    public float minPitch = -60f;
    public float maxPitch = 60f;

    InputAction lookAction;
    float yaw;
    float pitch;

    void Awake()
    {
        lookAction = playerInput.actions["Look"];
    }

    void LateUpdate()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();
        yaw += look.x * lookSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch - look.y * lookSpeed * Time.deltaTime, minPitch, maxPitch);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = target.position - rot * Vector3.forward * distance;
        transform.rotation = rot;
    }

    public Vector3 Forward => Quaternion.Euler(0, yaw, 0) * Vector3.forward;
    public Vector3 Right => Quaternion.Euler(0, yaw, 0) * Vector3.right;
}