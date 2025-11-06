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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Activar estado caminar
            isWalking = true;
            isIdle = false;

            // Rotar y mover
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            // Animaciones
            animator.SetBool("EstaCaminando", true);
            animator.SetBool("EstaParado", false);
        }
        else
        {
            // Activar estado quieto
            isWalking = false;
            isIdle = true;

            animator.SetBool("EstaCaminando", false);
            animator.SetBool("EstaParado", true);
        }
    }
}
