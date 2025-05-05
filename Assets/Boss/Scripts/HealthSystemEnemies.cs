using TMPro;
using UnityEngine;

public class HealthSystemEnemies : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private HurtCollider hurtCollider;

    private void Awake()
    {
        currentHealth = maxHealth;
        hurtCollider = GetComponent<HurtCollider>();
        if (hurtCollider != null)
        {
            hurtCollider.onHitRecived.AddListener(TakeDamage);
        }
    }

    private void TakeDamage(IHitter hitter, HurtCollider hurtCollider)
    {
        float damage = hitter.GetDamage();
        currentHealth -= damage;
        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        Destroy(gameObject);
    }
}
