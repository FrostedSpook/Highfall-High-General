using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;


//[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float[] AttackResetTimes;
    [SerializeField] private float[] comboResetTimes;
    private Vector3 input3D;
    private Coroutine attackCooldownCoroutine;
    private Coroutine comboResetCoroutine;
    public PlayerControls playerControls;
    private InputAction move;
    private InputAction basicAtk;
    private int comboCount = 1;
    private bool canAttack = true;

    Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        playerControls = new PlayerControls();


    }
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        basicAtk = playerControls.Player.BasicAttack;
        move.Enable();
        basicAtk.Enable();
        basicAtk.performed += BasicAttack;
        move.performed += GatherInput;
        move.canceled += GatherInput;
        animator = gameObject.GetComponent<Animator>();
    }
    private void OnDisable()
    {
        move.canceled -= GatherInput;
        move.performed -= GatherInput;
        move.Disable();
        basicAtk.Disable();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        look();
        Move();
    }
    public void GatherInput(InputAction.CallbackContext context)
    {
        Vector2 input2D = move.ReadValue<Vector2>();
        input3D = new Vector3(input2D.x, 0, input2D.y);
    }
    private void Move()
    {
        rb.velocity = transform.forward * moveSpeed * input3D.magnitude * Time.deltaTime;

        if (rb.velocity.magnitude > 0)
            animator.SetBool("IsMoving", true);
        else
            animator.SetBool("IsMoving", false);

    }
    private void look()
    {
        if (input3D == Vector3.zero) return;
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        Vector3 multMatrix = isoMatrix.MultiplyPoint3x4(input3D);

        Quaternion rotation = Quaternion.LookRotation(multMatrix, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }
    private void BasicAttack(InputAction.CallbackContext context)
    {
        if (canAttack == true)
        {
            canAttack = false;
            swordCollider.enabled = true;
            if (comboCount >= 4)
                comboCount = 1;
            Debug.Log(comboCount);
            animator.SetTrigger("Attack" + comboCount);
            
            if (attackCooldownCoroutine != null)
                StopCoroutine(attackCooldownCoroutine);
            attackCooldownCoroutine = StartCoroutine(AttackCooldown(AttackResetTimes[comboCount - 1]));


            if (comboResetCoroutine != null)
                StopCoroutine(comboResetCoroutine);
            comboResetCoroutine = StartCoroutine(ComboResetTimer(comboResetTimes[comboCount - 1]));

            
            Invoke(nameof(DisableCollision), 0.7f);
            comboCount++;
        }
    }

    private void DisableCollision()
    {
        swordCollider.enabled = false;
    }

    private IEnumerator AttackCooldown(float duration)
{
    yield return new WaitForSeconds(duration);
    canAttack = true;
}

private IEnumerator ComboResetTimer(float duration)
{
    yield return new WaitForSeconds(duration);
    comboCount = 1;
    Debug.Log("Combo reset due to inactivity");
}
}
