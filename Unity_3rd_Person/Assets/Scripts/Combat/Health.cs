using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;

    [SerializeField] private int maxHealth = 100;

    private int health = 0;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if (health == 0) return;

        health = Mathf.Max(health - damage, 0);

        OnTakeDamage?.Invoke();

        if(health == 0)
            OnDie?.Invoke();
    }
}
