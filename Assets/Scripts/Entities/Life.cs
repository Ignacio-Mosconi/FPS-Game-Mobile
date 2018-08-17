using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour 
{
    [SerializeField] float health;
    [SerializeField] AnimationClip deathAnimation;
    [SerializeField] UnityEvent onHit;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] AudioSource hitSound;

    public void TakeDamage(float amount)
    {
        if (health > 0)
        {
            hitSound.Play();
            health -= amount;
            if (health <= 0)
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
}
