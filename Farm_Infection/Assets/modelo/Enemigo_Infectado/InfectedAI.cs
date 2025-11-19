using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfectedAI : MonoBehaviour
{
    [Header("Referencias")]
    public NavMeshAgent agent;
    public Animator animator;
    public Transform player;
    public PlayerStats playerStats;

    [Header("Comportamiento")]
    public float patrolRadius = 15f;
    public float detectionRange = 12f;   // empieza a perseguir
    public float attackRange = 2.5f;     // rango para intentar atacar

    [Header("Ataque")]
    public float damagePerHit = 15f;

    [Tooltip("Duración TOTAL del clip de ataque (segundos)")]
    public float attackAnimDuration = 1.4f;   // 👈 tu clip dura 1.4

    [Tooltip("Momento del golpe dentro de la anim (0 = inicio, 1 = final)")]
    [Range(0f, 1f)]
    public float damageNormalizedTime = 1f;   // 👈 1 = al final del clip

    [Tooltip("Tiempo extra tras la animación antes de poder atacar otra vez")]
    public float attackCooldown = 0.5f;

    // Internos
    private bool isAttacking;
    private bool canAttack = true;
    private bool playerInMeleeTrigger = false;   // trigger delante de la boca

    void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponent<Animator>();

        if (!player)
        {
            var p = FindObjectOfType<Player_Movimiento>();
            if (p) player = p.transform;
        }

        if (!playerStats && player)
            playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!player || !playerStats) return;

        // Si está en pleno ataque, no perseguimos ni patrullamos
        if (isAttacking)
        {
            SetAnimatorMoving(false);
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        bool inRangeForAttack = dist <= attackRange || playerInMeleeTrigger;

        // ───── ATACAR ─────
        if (inRangeForAttack && canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
        // ───── PERSEGUIR ─────
        else if (dist <= detectionRange)
        {
            if (agent && agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            SetAnimatorMoving(true);
        }
        // ───── PATRULLAR ─────
        else
        {
            if (agent && agent.isOnNavMesh)
            {
                if (!agent.hasPath || agent.remainingDistance < 0.5f)
                {
                    Vector3 randomPoint = GetRandomPointAround(transform.position, patrolRadius);
                    agent.SetDestination(randomPoint);
                }
                agent.isStopped = false;
            }
            SetAnimatorMoving(true);
        }
    }

    void SetAnimatorMoving(bool moving)
    {
        if (!animator) return;
        animator.SetBool("IsMoving", moving && !isAttacking);
        animator.SetBool("IsAttacking", isAttacking);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        canAttack = false;

        if (agent && agent.isOnNavMesh)
            agent.isStopped = true;

        // SIEMPRE reproducimos la animación de ataque
        if (animator)
            animator.SetTrigger("Attack");

        // Tiempo exacto del golpe dentro del clip
        float damageTime = attackAnimDuration * Mathf.Clamp01(damageNormalizedTime);
        yield return new WaitForSeconds(damageTime);

        // 👉 AQUÍ se aplica el daño, NO antes
        if (playerStats != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackRange + 0.5f || playerInMeleeTrigger)
            {
                playerStats.TakeDamage(damagePerHit);
            }
        }

        // Esperamos el resto de la animación + cooldown
        float remaining = Mathf.Max(0f, attackAnimDuration - damageTime) + attackCooldown;
        if (remaining > 0f)
            yield return new WaitForSeconds(remaining);

        if (agent && agent.isOnNavMesh)
            agent.isStopped = false;

        isAttacking = false;
        canAttack = true;
    }

    Vector3 GetRandomPointAround(Vector3 center, float radius)
    {
        Vector2 random2D = Random.insideUnitCircle * radius;
        Vector3 point = new Vector3(center.x + random2D.x, center.y, center.z + random2D.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 5f, NavMesh.AllAreas))
            return hit.position;

        return center;
    }

    // ───── Trigger de melee delante de la boca (opcional) ─────
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInMeleeTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInMeleeTrigger = false;
    }
}
