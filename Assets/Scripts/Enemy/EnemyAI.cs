using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance { get; private set; }
    public NavMeshAgent agent;

    public Transform player;
    [SerializeField]public Transform[] patrolpuntos;
    public LayerMask whatisGround, whatisPlayer;
    public float sightRange, attackRange;
    public bool playerInsightRange, playerInAttackRange = false;
    //Attack
    public int health = 3;

    private void Awake()
    {
        player = GameObject.Find("Player_Object").transform;
        agent = GetComponent<NavMeshAgent>();
        if (Instance == null) { Instance = this; }
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
        Gizmos.DrawWireSphere(transform.position,attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        if (PatrolingAI.Instance.patrolpuntos.Length == 0)
        {
            ChasePlayerAI.Instance.ChasePlayer();
        }
        PatrolingAI.Instance.Patroling();
    }
    
    // Update is called once per frame
    public void Update()
    {
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatisPlayer);
        if (!playerInsightRange && !playerInAttackRange) PatrolingAI.Instance.Patroling();
        if (playerInsightRange && !playerInAttackRange) ChasePlayerAI.Instance.ChasePlayer();
        if (playerInsightRange && playerInAttackRange) AttackPlayerAI.Instance.AttackPlayer();
    }
}
