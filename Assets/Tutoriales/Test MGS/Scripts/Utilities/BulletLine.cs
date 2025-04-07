using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLine : MonoBehaviour
{
    public LineRenderer lineRendere;
    public float speed = .2f;

    private void OnEnable()
    {
        lineRendere.widthMultiplier = 1; 
    }

    private void Update()
    {
        if (lineRendere.widthMultiplier <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        { 
            lineRendere.widthMultiplier -= Time.deltaTime/speed;
        }
    }
}
