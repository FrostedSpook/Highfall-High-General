using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject swordCollider;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float[] AttackResetTimes;
    [SerializeField] private float[] comboResetTimes;
    private Vector3 moveDirection;
    private Vector3 input3D;
    private Coroutine attackCooldownCoroutine;
    private Coroutine comboResetCoroutine;
    public PlayerControls playerControls;
    private InputAction move;
    private InputAction basicAtk;
    private int comboCount = 1;
    private bool canAttack = true;
    Animator animator;
    PlayerCollision collisionScript;
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
        move.performed += GatherInput;
        move.canceled += GatherInput;
        basicAtk.performed += BasicAttack;
        animator = gameObject.GetComponent<Animator>();
        collisionScript = swordCollider.GetComponent<PlayerCollision>();
    }

    private void OnDisable()
    {
        move.Disable();
        basicAtk.Disable();
    }

    private void Update()
    {
        Look();
        Move();
    }

    private void GatherInput(InputAction.CallbackContext context)
    {
        Vector2 input2D = move.ReadValue<Vector2>();
        Vector3 rawInput = new Vector3(input2D.x, 0, input2D.y);

        // Rotate input to align with isometric perspective (45 degrees)
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, -45, 0));
        input3D = isoMatrix.MultiplyPoint3x4(rawInput);
    }

    private void Move()
    {

        Vector3 moveVelocity = input3D.normalized * moveSpeed;
        rb.velocity = moveVelocity;

        if (rb.velocity.magnitude > 0)
            animator.SetBool("IsMoving", true);
        else
            animator.SetBool("IsMoving", false);
    }

    private void Look()
    {
        if (input3D == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(input3D, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void BasicAttack(InputAction.CallbackContext context)
    {
        if (!canAttack)
            return;

        canAttack = false;
        swordCollider.GetComponent<Collider>().enabled = true;
        
        if (comboCount >= 4)
            comboCount = 1;

        int baseDmg = comboCount * 10;
        collisionScript.minDamage = baseDmg;
        collisionScript.maxDamage = baseDmg + (comboCount * 5);
        Debug.Log(comboCount);
        animator.SetTrigger("Attack" + comboCount);

        if (attackCooldownCoroutine != null)
            StopCoroutine(attackCooldownCoroutine);
        attackCooldownCoroutine = StartCoroutine(AttackCooldown(AttackResetTimes[comboCount - 1]));


        if (comboResetCoroutine != null)
            StopCoroutine(comboResetCoroutine);
        comboResetCoroutine = StartCoroutine(ComboResetTimer(comboResetTimes[comboCount - 1]));
        
        comboCount++;

    }

    private void DisableCollision()
    {
        
    }

    private IEnumerator AttackCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        swordCollider.GetComponent<Collider>().enabled = false;
        canAttack = true;
    }

    private IEnumerator ComboResetTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        comboCount = 1;
        Debug.Log("Combo reset due to inactivity");
    }
}