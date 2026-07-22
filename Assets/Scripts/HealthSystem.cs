using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] int maxHealth = 10;

    int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
