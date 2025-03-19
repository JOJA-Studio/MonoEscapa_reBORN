using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerAI : MonoBehaviour
{
    public static ChasePlayerAI Instance { get; private set; }

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
    public void ChasePlayer()
    {
        EnemyAI.Instance.agent.SetDestination(EnemyAI.Instance.player.position);
    }
   
}
