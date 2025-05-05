using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class puertas : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] int cardsToOpen = 4;
    bool isAlreadyOpen = false;

    [Header("References")]
    [SerializeField] Transform left;
    [SerializeField] Transform right;
    // Update is called once per frame
    void Update()
    {
        if (!isAlreadyOpen && (cardsToOpen == Card.count))
        {
            isAlreadyOpen = true;
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        left.DORotate(Vector3.up * -90f, 1f, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.OutBounce);
        right.DORotate(Vector3.up * 90f, 1f, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.OutBounce);
    }
}
