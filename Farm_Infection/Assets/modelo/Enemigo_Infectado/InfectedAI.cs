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
    public float detectionRange = 12f;   
    public float attackRange = 2.5f;     

    [Header("Ataque")]
    public float damagePerHit = 15f;

    [Tooltip("Duración TOTAL del clip de ataque (segundos)")]
    public float attackAnimDuration = 1.4f;   

    [Tooltip("Momento del golpe dentro de la anim (0 = inicio, 1 = final)")]
    [Range(0f, 1f)]
    public float damageNormalizedTime = 1f;  

    [Tooltip("Tiempo extra tras la animación antes de poder atacar otra vez")]
    public float attackCooldown = 0.5f;

    // Internos
    private bool isAttacking;
    private bool canAttack = true;
    private bool playerInMeleeTrigger = false;   

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

        if (isAttacking)
        {
            SetAnimatorMoving(false);
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        bool inRangeForAttack = dist <= attackRange || playerInMeleeTrigger;

        if (inRangeForAttack && canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
        else if (dist <= detectionRange)
        {
            if (agent && agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            SetAnimatorMoving(true);
        }
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

        if (animator)
            animator.SetTrigger("Attack");

        float damageTime = attackAnimDuration * Mathf.Clamp01(damageNormalizedTime);
        yield return new WaitForSeconds(damageTime);

        if (playerStats != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackRange + 0.5f || playerInMeleeTrigger)
            {
                playerStats.TakeDamage(damagePerHit);
            }
        }

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
