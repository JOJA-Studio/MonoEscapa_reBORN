using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class enemyRestarVidaTest : MonoBehaviour
{
    [SerializeField] GameObject marker;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Chocando con player");
            ConsciousnessBar.instance.takeDamage(10);
        }
    }

    public void ToggleMark()
    {
        marker.SetActive(true);
    }
}
