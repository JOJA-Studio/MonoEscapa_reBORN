using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEditor;

public class restaurarVida : MonoBehaviour
{
    [SerializeField] float duracion;
    [SerializeField] float jumpPower;
    [SerializeField] int numSaltos;
    [SerializeField] bool isGroundHealthPack;
    [SerializeField] GameObject SpawnEndValue;
    [SerializeField] GameObject meAnItem;
    public float lifeToRestore = 50;
    float delay = 0.5f;
    public AudioSource healsound;

    private void Start()
    {
        if (isGroundHealthPack)
        {
            transform.DORotate(Vector3.up * 360, 1f).SetRelative().SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
        else
        {
            meAnItem.transform.DOJump(SpawnEndValue.transform.position, jumpPower, numSaltos, duracion, false);
            transform.DORotate(Vector3.up * 360, 1f).SetRelative().SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
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
