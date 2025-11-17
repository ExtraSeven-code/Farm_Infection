using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Player_Movimiento movement;
    public HotbarSelector hotbar;

    [Header("Ataque")]
    public float attackDuration = 0.6f;   // duración total de la anim de atacar (segundos)
    public float hitTime = 0.35f;        // en qué segundo del ataque pega el golpe

    private bool isAttacking = false;
    private ChoppableTree currentTree;   // árbol dentro del trigger

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            // Solo tiene sentido atacar si hay algo y tiene herramienta
            ItemData tool = hotbar.GetSelectedItem();
            if (tool != null && tool.isTool && currentTree != null)
            {
                StartCoroutine(AttackRoutine(tool));
            }
        }
    }

    IEnumerator AttackRoutine(ItemData tool)
    {
        isAttacking = true;
        movement.canMove = false;          // ❌ bloquear movimiento

        animator.SetTrigger("Atacar");     // ▶ reproducir animación

        // ⏱ esperar hasta el momento del impacto
        yield return new WaitForSeconds(hitTime);

        // 💥 aplicar daño AQUÍ
        if (currentTree != null)
        {
            currentTree.Hit(tool);
        }

        // ⏱ esperar el resto de la animación
        float remaining = Mathf.Max(0f, attackDuration - hitTime);
        yield return new WaitForSeconds(remaining);

        movement.canMove = true;           // ✅ volver a mover
        isAttacking = false;
    }

    // ───────── DETECCIÓN DEL ÁRBOL POR TRIGGER ─────────

    private void OnTriggerEnter(Collider other)
    {
        var tree = other.GetComponent<ChoppableTree>();
        if (tree != null)
            currentTree = tree;
    }

    private void OnTriggerExit(Collider other)
    {
        var tree = other.GetComponent<ChoppableTree>();
        if (tree != null && tree == currentTree)
            currentTree = null;
    }
}
