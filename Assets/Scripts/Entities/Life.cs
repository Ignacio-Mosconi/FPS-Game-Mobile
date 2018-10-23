using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HitEvent : UnityEvent<Transform> {}

public class Life : MonoBehaviour 
{
    [SerializeField] float maxHealth;
    [SerializeField] float deadBodyDuration = 60f;
    [SerializeField] AnimationClip deathAnimation;
    [SerializeField] UnityEvent onHit;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] HitEvent onDamagerHit;
    float health = 0;

    public void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount, Transform damager = null)
    {
        if (health > 1f)
        {
            health -= amount;
            if (health < 1f)
            {
                if (deathAnimation)
                    Die(deathAnimation.length);
                else
                    Die();
            }
            else
            {
                onHit.Invoke();
                onDamagerHit.Invoke(damager);
            }
        }
    }

    void Die()
    {
        onDeath.Invoke();
        DisableMainComponents();
    }

    void Die(float deathDuration)
    {
        if (transform.gameObject.layer == LayerMask.NameToLayer("Zombies"))
            transform.SetParent(transform.parent.parent); // The grandfather.
        onDeath.Invoke();
        Destroy(gameObject, deathDuration + deadBodyDuration);
        DisableMainComponents();
    }

    void DisableMainComponents()
    {
        Collider[] colliders = GetComponents<Collider>();
        Collider[] childrenColliders = GetComponentsInChildren<Collider>();
        Canvas canvas = GetComponentInChildren<Canvas>();

        foreach (Collider collider in colliders)
            if (collider)
                collider.enabled = false;
        
        foreach (Collider collider in childrenColliders)
            if (collider)
                collider.enabled = false;

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
    
    public HitEvent OnDamagerHit
    {
        get { return onDamagerHit; }
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
