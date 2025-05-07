using System.Collections;
using TMPro;
using UnityEngine;

public class HealthSystemEnemies : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private HurtCollider hurtCollider;
    public Animator animator;
    public Rigidbody rb;

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
        animator.SetBool("die", true);
        //rb.isKinematic = false;

        StartCoroutine(MuerteSecuencia(rb, gameObject));

        //Destroy(gameObject);
    }

    public IEnumerator MuerteSecuencia(Rigidbody rb, GameObject gameObject)
    {
        yield return new WaitForSeconds(2.25f);

        if (rb != null)
            rb.isKinematic = false;

        yield return new WaitForSeconds(0.5f);

        if (gameObject != null)
            Destroy(gameObject);
    }
}
