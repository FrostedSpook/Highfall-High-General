using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 360f;

    private Vector3 moveDirection;
    private Vector3 input3D;

    public PlayerControls playerControls;
    private InputAction move;
    private CharacterController charController;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        charController = GetComponent<CharacterController>();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Update()
    {
        GatherInput();
        Look();
        Move();
    }

    private void GatherInput()
    {
        Vector2 input2D = move.ReadValue<Vector2>();
        Vector3 rawInput = new Vector3(input2D.x, 0, input2D.y);

        // Rotate input to align with isometric perspective (45 degrees)
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, -45, 0));
        input3D = isoMatrix.MultiplyPoint3x4(rawInput); // isometric-aligned movement direction
    }

    private void Move()
    {
        if (input3D.magnitude < 0.1f) return;

        moveDirection = input3D.normalized * moveSpeed * Time.deltaTime;
        charController.Move(moveDirection);
    }

    private void Look()
    {
        if (input3D == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(input3D, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}