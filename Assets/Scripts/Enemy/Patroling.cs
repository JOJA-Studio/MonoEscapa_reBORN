using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolingAI : MonoBehaviour
{
    public static PatrolingAI Instance { get; private set; }
    [SerializeField] public Transform[] patrolpuntos;
    int targetPoint = 0;
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
    public void Patroling()
    {
        EnemyAI.Instance.agent.SetDestination(patrolpuntos[targetPoint].transform.position);
        for (int i = 0; i < patrolpuntos.Length; i++)
        {
            patrolpuntos[i].gameObject.SetActive(true);
        }
        SearchWalkPoint();


    }
    public void SearchWalkPoint()
    {


        //if (transform.position.x == patrolpuntos[targetPoint].transform.position.x && transform.position.z == patrolpuntos[targetPoint].transform.position.z)
        //{
        //    targetPoint++;
        //    if (targetPoint == patrolpuntos.Length)
        //    {
        //        targetPoint = 0;
        //    }
        //}
        if (Vector3.Distance(transform.position, patrolpuntos[targetPoint].position) < 1f) // Mejor comparación de distancia
        {
            targetPoint = (targetPoint + 1) % patrolpuntos.Length; // Hace que el índice vuelva a 0 cuando llegue al final
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
