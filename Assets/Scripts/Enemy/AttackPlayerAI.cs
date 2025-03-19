using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class AttackPlayerAI : MonoBehaviour
{
    public static AttackPlayerAI Instance { get; private set; }
    public GameObject projectile;
    bool alreadyAttacked;
    public float timeBetweenAttacks;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Evita duplicados en la escena
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AttackPlayer()
    {
        //agent.SetDestination(transform.position);
        EnemyAI.Instance.agent.ResetPath();
        transform.LookAt(EnemyAI.Instance.player);
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
}
