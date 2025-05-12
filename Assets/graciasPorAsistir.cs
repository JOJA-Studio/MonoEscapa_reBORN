using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class graciasPorAsistir : MonoBehaviour
{
    [SerializeField] GameObject gracias;
    public string sceneName; // Nombre de la escena a cargar
    public float delay = 20f; // Tiempo de espera en segundos

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Card.count >= Card.maxCount)
        { 
            gracias.SetActive(true);
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo indicado
        SceneManager.LoadScene(sceneName); // Carga la nueva escena
    }
}
