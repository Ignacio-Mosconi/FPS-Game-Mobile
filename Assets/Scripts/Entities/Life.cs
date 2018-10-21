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
        if (transform.gameObject.layer == LayerMask.NameToLayer("Zombies"))
            transform.SetParent(transform.parent.parent); // The grandfather.
        onDeath.Invoke();
        Destroy(gameObject, deathDuration + deadBodyDuration);
        DisableComponents();
    }

    void DisableComponents()
    {
        Collider[] colliders = GetComponents<Collider>();
        Collider[] childrenColliders = GetComponentsInChildren<Collider>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        Canvas canvas = GetComponentInChildren<Canvas>();
        CharacterController characterController = GetComponent<CharacterController>();

        foreach (Collider collider in colliders)
            if (collider)
                collider.enabled = false;
        
        foreach (Collider collider in childrenColliders)
            if (collider)
                collider.enabled = false;

        foreach (AudioSource audioSource in audioSources)
            if (audioSource)
                audioSource.enabled = false;

        if (canvas)
            canvas.enabled = false;

        if (characterController)
            characterController.enabled = false;
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
