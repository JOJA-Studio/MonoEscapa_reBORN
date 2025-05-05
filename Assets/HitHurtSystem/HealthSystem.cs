using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private HurtCollider hurtCollider;

    public TextMeshProUGUI healthText;
    public Image healthBar;
    float lerpSpeed;

    private void Awake()
    {
        currentHealth = maxHealth;
        hurtCollider = GetComponent<HurtCollider>();
        if (hurtCollider != null)
        {
            hurtCollider.onHitRecived.AddListener(TakeDamage);
        }
    }

    void Update()
    {
        healthText.text = "Health: " + currentHealth + "%";
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
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

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, lerpSpeed);
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (currentHealth / maxHealth));

        healthBar.color = healthColor;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        Destroy(gameObject);
    }
}
