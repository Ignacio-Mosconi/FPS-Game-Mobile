using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour 
{
    [SerializeField] float maxHealth;
    [SerializeField] float deadBodyDuration = 60f;
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
        DisableComponents();
    }

    void Die(float deathDuration)
    {
        onDeath.Invoke();
        Destroy(gameObject, deathDuration + deadBodyDuration);
        DisableComponents();
    }

    void DisableComponents()
    {
        Collider collider = GetComponent<Collider>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        Canvas canvas = GetComponentInChildren<Canvas>();

        if (collider)
            collider.enabled = false;

        foreach (AudioSource audioSource in audioSources)
            if (audioSource)
                audioSource.enabled = false;

        if (canvas)
            canvas.enabled = false;
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
