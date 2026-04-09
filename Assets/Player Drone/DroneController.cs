using UnityEngine;
using UnityEngine.InputSystem;
public class DroneController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 6f;
    public float verticalSpeed = 5f;

    [Header("Collision")]
    public float collisionRadius = 0.5f;
    public LayerMask collisionMask;

    [Header("Tilt")]
    public float tiltAmount = 10f;
    public float tiltSmooth = 5f;

    [Header("Rotation")]
    public float mouseSensitivity = 2f;
    public float maxPitch = 80f;

    private float pitch;
    private Vector3 currentVelocity;
    private float currentVertical;
    private float tiltX;
    private float tiltZ;

    public Transform cameraPivot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleTilt();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 targetMove =
            (transform.forward * v + transform.right * h) * moveSpeed;

        currentVelocity = Vector3.Lerp(
            currentVelocity,
            targetMove,
            acceleration * Time.deltaTime
        );

        Vector3 horizontalMove = currentVelocity * Time.deltaTime;

        TryMove(horizontalMove);

        // Vertical
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.LeftShift)) verticalInput = 1f;
        if (Input.GetKey(KeyCode.LeftControl)) verticalInput = -1f;

        currentVertical = Mathf.Lerp(
            currentVertical,
            verticalInput * verticalSpeed,
            acceleration * Time.deltaTime
        );

        Vector3 verticalMove = Vector3.up * currentVertical * Time.deltaTime;

        TryMove(verticalMove);

        // Tilt targets
        tiltZ = -h * tiltAmount;
        tiltX = v * tiltAmount;
    }

    void TryMove(Vector3 move)
    {
        if (move == Vector3.zero) return;

        RaycastHit hit;

        if (Physics.SphereCast(
            transform.position,
            collisionRadius,
            move.normalized,
            out hit,
            move.magnitude,
            collisionMask))
        {
            Vector3 slideDirection = Vector3.ProjectOnPlane(move, hit.normal);
            transform.position += slideDirection;
        }
        else
        {
            transform.position += move;
        }
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        transform.Rotate(0f, mouseX, 0f, Space.World);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void HandleTilt()
    {
        Quaternion targetRotation = Quaternion.Euler(
            tiltX,
            transform.eulerAngles.y,
            tiltZ
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            tiltSmooth * Time.deltaTime
        );
    }
}