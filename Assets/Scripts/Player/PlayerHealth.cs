using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public Text healthText;
    public Slider hpSlider;

    [Header("Audio")]
    public AudioClip takeDamageSoundClip;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthUI();

        SEAudio.Instance.PlayOneShot(takeDamageSoundClip,0.25f);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    void Die()
    {
        if (GameManager.Instance.IsGameFinished()) return;

        Debug.Log("Player is Dead!");
        GameManager.Instance.GameOver();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }
    }
}
