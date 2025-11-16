using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movimiento : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Animator animator;

    // Estados
    public bool isWalking;
    public bool isIdle = true;

    public bool isInteracting;   // <-- nueva línea

    public Transform cameraTransform;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // ⛔ Bloquear movimiento durante la animación de interacción
        if (isInteracting)
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("EstaCaminando", false);
            animator.SetBool("EstaParado", true);
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = (cameraForward * vertical + cameraRight * horizontal).normalized;

        if (direction.magnitude >= 0.1f)
        {
            isWalking = true;
            isIdle = false;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

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
    }
}
