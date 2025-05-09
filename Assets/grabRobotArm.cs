using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class grabRobotArm : MonoBehaviour
{
    [SerializeField] GameObject robotArm; 
    [SerializeField] AudioSource weaponTaket;
    float delay = 0.5f;
    private void Start()
    {
        transform.DORotate(Vector3.up * 360, 1f).SetRelative().SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weaponTaket.Play();
            robotArm.SetActive(true);
            StartCoroutine(DestroyObject());
        }
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo indicado
        Destroy(this.gameObject); // Destruye el objeto
    }
}
