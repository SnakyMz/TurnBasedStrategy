using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] Image healthBar;
    [SerializeField] int maxHealth = 10;

    int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(int amount)
    {
        health -= amount;
        UpdateHealthBar();

        if (health <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)health / maxHealth;
    }
}
