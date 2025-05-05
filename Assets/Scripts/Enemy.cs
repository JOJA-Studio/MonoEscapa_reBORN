using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 3;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float aggroRange = 15f;

    [Header("Movement")]
    [SerializeField] float patrolSpeed = 1f;
    [SerializeField] float chaseSpeed = 4f;

    [Header("Patrulla Aleatoria")]
    [SerializeField] float patrolRadius = 5f;
    [SerializeField] float patrolWaitTime = 2f;

    [Header("Attack Hitbox")]
    [SerializeField] HitCollider hitCollider;
    [SerializeField] HurtCollider hurtCollider;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    bool isPatrolling = true;
    float patrolWaitCounter = 0f;
    bool hasDestination = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        AdjustToGround();

        GoToRandomPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (timePassed >= attackCD && distanceToPlayer <= attackRange)
        {
            Attack();
        }

        timePassed += Time.deltaTime;

        if (distanceToPlayer <= aggroRange)
        {
            agent.SetDestination(player.transform.position);
            agent.speed = chaseSpeed; 
            isPatrolling = false;
            hasDestination = false;
        }
        else
        {
            if (!isPatrolling)
            {
                isPatrolling = true;
                agent.speed = patrolSpeed; 
                GoToRandomPatrolPoint();
            }

            PatrolBehavior();
        }
    }

    void PatrolBehavior()
    {
        if (isPatrolling)
        {
            if (hasDestination && agent.remainingDistance < 0.5f)
            {
                patrolWaitCounter += Time.deltaTime;
                if (patrolWaitCounter >= patrolWaitTime)
                {
                    patrolWaitCounter = 0;
                    GoToRandomPatrolPoint();
                }
            }

            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.1f)
            {
                GoToRandomPatrolPoint();
            }
        }
    }

    void GoToRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            hasDestination = true;
        }
        else
        {
            GoToRandomPatrolPoint();
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");
        timePassed = 0;

        Debug.Log("Siguiente paso en el ataque");


        if (hitCollider != null)
        {
            Debug.Log("Ataca");

            hitCollider.gameObject.SetActive(true);
            Invoke(nameof(DeactivateHitCollider), 4f);
        }
    }

    private void DeactivateHitCollider()
    {
        if (hitCollider != null)
        {
            hitCollider.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        //animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //animator.SetTrigger("die");
        agent.enabled = false;
        this.enabled = false;
        Destroy(gameObject, 3f);
    }

    void AdjustToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3f))
        {
            transform.position = hit.point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
