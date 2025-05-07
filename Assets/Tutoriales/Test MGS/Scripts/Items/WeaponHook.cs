using System.Collections;
using UnityEngine;

public class WeaponHook : MonoBehaviour
{
    public int currentAmmo;
    public int allAmmo = 40;
    [HideInInspector]
    public WeaponItem baseItem;
    ParticleSystem[] particles;
    public Transform bulletEmmiter;

    public void Init(WeaponItem weaponItem)
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        baseItem = weaponItem;
        currentAmmo = baseItem.magazineAmmo;
    }

    public void Shoot()
    {
        Debug.Log("Disparo realizado");
        Debug.DrawRay(bulletEmmiter.position, bulletEmmiter.forward * 100f, Color.red, 1f);

        if (particles != null)
        { 
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }

        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(bulletEmmiter.position, bulletEmmiter.forward, out hit, 100f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Jugador alcanzado por disparo enemigo");
                ConsciousnessBar.instance.takeDamage(1);
            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 50, GameReferences.controllersLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            AIController aIController = colliders[i].transform.GetComponent<AIController>();
            if (aIController != null)
            {
                aIController.UpdateLastKnowPosition(transform.position);
            }
        }
    }

    public void Reload()
    {
        if (allAmmo <= baseItem.magazineAmmo)
        {
            currentAmmo = allAmmo;
            allAmmo = 0;
        }
        else
        { 
            currentAmmo = baseItem.magazineAmmo;
            allAmmo -= baseItem.magazineAmmo;
        }
    }
}