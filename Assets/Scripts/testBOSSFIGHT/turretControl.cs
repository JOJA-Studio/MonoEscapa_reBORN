using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretControl : MonoBehaviour
{
    Transform _Player;
    float dist;
    public float howClose;
    public Transform head;
    public GameObject _projectile;
    public GameObject spawnPoint;
    public int bulletSpeed = 10;
    public float fireRate, nextFire;

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        dist = Vector3.Distance(_Player.position, transform.position);
        if (dist <= howClose)
        {
            head.LookAt(_Player);
            if (Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject clone = Instantiate(_projectile, spawnPoint.transform.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward * bulletSpeed);
        Destroy(clone, 3);
    }
}
