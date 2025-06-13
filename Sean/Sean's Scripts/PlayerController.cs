using UnityEngine;
using UnityEngine.InputSystem;


//[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 360f;
    private Vector3 input3D;

    public PlayerControls playerControls;
    private InputAction move;
    private CharacterController charController;
    // Start is called before the first frame update
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

    // Update is called once per frame
    private void FixedUpdate()
    {
        GatherInput();
        look();
        Move();
    }
    public void GatherInput()
    {
        Vector2 input2D = move.ReadValue<Vector2>();
        input3D = new Vector3(input2D.x, 0, input2D.y);
    }
    private void Move()
    {
        Vector3 moveDirection = transform.forward * moveSpeed * input3D.magnitude * Time.deltaTime;
        charController.Move(moveDirection);
    }
    private void look()
    {
        if (input3D == Vector3.zero) return;
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        Vector3 multMatrix = isoMatrix.MultiplyPoint3x4(input3D);

        Quaternion rotation = Quaternion.LookRotation(multMatrix, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }
}
