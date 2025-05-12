using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConsciousnessBar : MonoBehaviour
{
    public static ConsciousnessBar instance
    {
        get; private set;
    }

    public Slider ConsciousnessSlider;
    public Slider easeConsciousnessSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.03f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        if (ConsciousnessSlider.value != health)
        {
            ConsciousnessSlider.value = health;
        }

        //if (Input.GetKeyDown(KeyCode.J)) 
        //{
        //    takeDamage(10);
        //}

        if (ConsciousnessSlider.value != easeConsciousnessSlider.value)
        {
            easeConsciousnessSlider.value = Mathf.Lerp(easeConsciousnessSlider.value, health, lerpSpeed);
        }

        if (health > 100)
        {
            health = 100;
        }
        else if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void takeDamage(float v)
    {
        health -= v;
    }

    public void restoreHealt(float v)
    {
        health += v;
    }
}
