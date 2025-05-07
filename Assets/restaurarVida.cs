using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class restaurarVida : MonoBehaviour
{
    public float lifeToRestore = 50;
    //public GameObject hPack;

    private void Start()
    {
        transform.DORotate(Vector3.up * 360, 1f).SetRelative().SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Chocando con player");
            ConsciousnessBar.instance.restoreHealt(lifeToRestore);
            Destroy(this.gameObject);
        }
    }
}
