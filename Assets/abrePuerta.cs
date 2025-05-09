using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class abrePuerta : MonoBehaviour
{
    [Header("Puerta a rotar")]
    public Transform doorTransform;

    [Header("Ángulos de rotación")]
    public Vector3 closedRotation = new Vector3(88.45f, -90, -90); // Rotación cerrada (posición inicial)
    public Vector3 openRotation = new Vector3(-1.45f, -90, -90); // Rotación abierta (ej. 90 grados en Y)

    [Header("Ajustes de animación")]
    public float animationDuration = 1f;
    public Ease animationEase = Ease.InOutQuad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entró al Trigger - Abriendo puerta");
            doorTransform.DOKill(); // Evita animaciones duplicadas
            doorTransform.DORotate(openRotation, animationDuration).SetEase(animationEase);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador salió del Trigger - Cerrando puerta");
            doorTransform.DOKill(); // Evita animaciones duplicadas
            doorTransform.DORotate(closedRotation, animationDuration).SetEase(animationEase);
        }
    }
}
