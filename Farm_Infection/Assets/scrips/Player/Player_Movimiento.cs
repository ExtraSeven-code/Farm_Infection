using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player_Movimiento : MonoBehaviour
{
    [Header("Referencias")]
    public Transform orientation;      // empty alineado con la cámara
    public Transform cameraTransform;  // Main Camera

    private Rigidbody rb;
    private Animator animator;
    private PlayerStats stats;

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 720f; // grados por segundo

    [Header("Salto")]
    public float jumpForce = 7f;
    public float groundedVelocityThreshold = 0.05f;

    // Estados
    public bool isRunning;
    public bool isWalking;
    public bool isIdle = true;

    public bool canMove = true;
    public bool isInteracting = false;
    public bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();

        rb.freezeRotation = true;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Rotación independiente de la cámara → orientación
        Vector3 viewDir = cameraTransform.forward;
        viewDir.y = 0f;
        viewDir.Normalize();

        if (viewDir.sqrMagnitude > 0.001f)
        {
            orientation.forward = viewDir;
        }

        // Correr
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (stats == null || stats.UseStamina(10f * Time.deltaTime))
                isRunning = true;
            else
                isRunning = false;
        }
        else
        {
            isRunning = false;
        }

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (!canMove || isInteracting || isAttacking)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            animator.SetBool("EstaCaminando", false);
            animator.SetBool("EstaParado", true);
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = orientation.forward * vertical + orientation.right * horizontal;
        moveDir.Normalize();

        if (moveDir.magnitude >= 0.1f)
        {
            isWalking = true;
            isIdle = false;

            // Rotación suave
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);


            float speed = isRunning ? runSpeed : moveSpeed;
            Vector3 move = moveDir * speed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + move);

            animator.SetBool("EstaCaminando", true);
            animator.SetBool("EstaParado", false);
        }
        else
        {
            isWalking = false;
            isIdle = true;
            animator.SetBool("EstaCaminando", false);
            animator.SetBool("EstaParado", true);
        }

        UpdateAnimatorGrounded();
    }

    // --- Suelo basado en velocidad vertical ---
    bool IsGroundedByVelocity()
    {
        return Mathf.Abs(rb.velocity.y) < groundedVelocityThreshold;
    }

    void UpdateAnimatorGrounded()
    {
        bool grounded = IsGroundedByVelocity();
        animator.SetBool("Grounded", grounded);
    }

    bool CanJump()
    {
        if (!canMove) return false;
        if (isInteracting) return false;
        if (isAttacking) return false;

        if (!stats.UseStamina(20f)) return false;

        return IsGroundedByVelocity();
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (animator != null)
        {
            animator.SetTrigger("Saltar");
        }
    }
}
