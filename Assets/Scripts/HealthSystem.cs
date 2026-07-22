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

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        UpdateHealthBar();
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

    public bool IsHealthLow()
    {
        return health <= maxHealth / 2;
    }

    public float GetHealthNormalize()
    {
        return (float)health / maxHealth;
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = GetHealthNormalize();
    }
}
