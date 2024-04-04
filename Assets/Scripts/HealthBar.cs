using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float health;
    public Slider easeHealthSlider;
    public float lerpSpeed = 0.05f;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        // Ensure health slider reflects current health
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        // Ease health slider towards current health
        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }

        // Check if health has reached zero
        if (health <= 0)
        {
            // Reload scene with index 0
            SceneManager.LoadScene(2);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
