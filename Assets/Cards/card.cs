using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Card : MonoBehaviour
{
    public static int count = 0;
    public static int maxCount = 0;

    AudioSource coinSound;

    void Awake()
    {
        maxCount++;
    }

    private void Start()
    {
        transform.DORotate(Vector3.up * 360, 3f).SetRelative().SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        coinSound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            count++;
            transform.DOLocalMoveY(transform.position.y + 2, 0.5f).SetRelative().SetEase(Ease.InOutBounce);
            transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutBounce).OnComplete(() => Destroy(gameObject));
            GetComponent<Collider>().enabled = false;
            if (coinSound != null)
            {
                coinSound.Play();
            }
        }
    }
    public static void ResetCards()
    {
        count = 0;
        maxCount = 0;

        // Eliminar todas las monedas en la escena
        Card[] cards = FindObjectsOfType<Card>();
        foreach (Card card in cards)
        {
            Destroy(card.gameObject);
        }
    }
}
