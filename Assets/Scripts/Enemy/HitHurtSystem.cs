using System;
using UnityEngine;
using UnityEngine.Events;

public class HitHurtSystem : MonoBehaviour
{
    int Vida = 3;
    int VidaEnemy = 1;
    //UnityEvent<void,void> onHitDelibered;
    //string[] HittableTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VidaEnemy--;
            Console.WriteLine("VidaEnemy" + VidaEnemy);
        }
        else 
        {
            Vida--;
            Console.WriteLine("VidaPlayer"+Vida);
        }
    }
    void HitCollider()
    {
   //     onHitDelibered.Invoke();
    }
    void HurtCollider() 
    {
    
    }
}
