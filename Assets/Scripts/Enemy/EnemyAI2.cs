using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2 : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;
    [SerializeField] public Transform[] patrolpuntos;
    public LayerMask whatisGround, whatisPlayer;
    int targetPoint = 0;
    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    bool viendoPlayer = false;
    public float walkPointRange;
    //Chase
    public float sightRange, attackRange;
    public bool playerInsightRange, playerInAttackRange;
    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public int health = 3;

    private void Awake()
    {
        player = GameObject.Find("Player_Object").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Patroling()
    {
        viendoPlayer = false;
        if (viendoPlayer == false)
        {
            agent.SetDestination(patrolpuntos[targetPoint].transform.position);
            
                SearchWalkPoint();
            
        }
    }
    private void SearchWalkPoint()
    {
        // float randomZ = Random.Range(-walkPointRange, walkPointRange);
        // float randomX = Random.Range(-walkPointRange, walkPointRange);
        // walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisGround))
        // { 
        //     walkPointSet = true;
        // }

        if (transform.position.x == patrolpuntos[targetPoint].transform.position.x && transform.position.z == patrolpuntos[targetPoint].transform.position.z)
        {
            targetPoint++;
            if (targetPoint == patrolpuntos.Length)
            {
                targetPoint = 0;
            }

        }
    }
    private void Chaseplayer()
    {
        viendoPlayer = true;
        agent.SetDestination(player.position);

    }
    private void Attackplayer()
    {
        viendoPlayer = true;
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            Rigidbody rigidoCuerpo = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rigidoCuerpo.AddForce(transform.forward * 32, ForceMode.Impulse);
            rigidoCuerpo.AddForce(transform.up * 8, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    // Start is called before the first frame update
    void Start()
    {
        SearchWalkPoint();
    }

    // Update is called once per frame
    private void Update()
    {
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatisPlayer);
        if (!playerInsightRange && !playerInAttackRange) Patroling();
        if (playerInsightRange && !playerInAttackRange) Chaseplayer();
        if (playerInsightRange && playerInAttackRange) Attackplayer();
    }
}
