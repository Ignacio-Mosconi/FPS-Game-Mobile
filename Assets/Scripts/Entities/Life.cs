using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour 
{
    [SerializeField] float maxHealth;
    [SerializeField] AnimationClip deathAnimation;
    [SerializeField] UnityEvent onHit;
    [SerializeField] UnityEvent onDeath;
    float health = 0;

    public void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (health > 1)
        {
            health -= amount;
            if (health < 1)
            {
                if (deathAnimation)
                    Die(deathAnimation.length);
                else
                    Die();
            }
            else
                onHit.Invoke();
        }
    }

    void Die()
    {
        onDeath.Invoke();
    }

    void Die(float deathDuration)
    {
        onDeath.Invoke();
        Destroy(gameObject, deathDuration);
    }
    
    public UnityEvent OnHit
    {
        get { return onHit; }
    }

    public UnityEvent OnDeath
    {
        get { return onDeath; }
    }

    public float Health
    {
        get { return health; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }
}
