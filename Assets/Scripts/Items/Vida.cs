using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Vida : MonoBehaviour
{
    [SerializeField] float duracion;
    [SerializeField] float jumpPower;
    [SerializeField] int numSaltos;
    [SerializeField] GameObject SpawnEndValue;
    [SerializeField] GameObject meAnItem;

    private void Start()
    {
        meAnItem.transform.DOJump(SpawnEndValue.transform.position, jumpPower, numSaltos, duracion, false);
        transform.DORotate(Vector3.up * 360, 1f).SetRelative().SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    //public void DotweenAnimationHeart()
    //{
    //}
}
