using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class grabRobotArm : MonoBehaviour
{
    [SerializeField] GameObject robotArm; 
    private void Start()
    {
        transform.DORotate(Vector3.up * 360, 1f).SetRelative().SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            robotArm.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
