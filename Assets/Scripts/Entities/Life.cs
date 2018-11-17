using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HitEvent : UnityEvent<Transform> {}

public class Life : MonoBehaviour 
{
    [SerializeField] float maxHealth;
    [SerializeField] float deadBodyDuration = 60f;
    [SerializeField] AnimationClip deathAnimation;
    
    float health = 0;

    UnityEvent onHit = new UnityEvent();
    UnityEvent onDeath = new UnityEvent();
    HitEvent onDamagerHit = new HitEvent();

    public void Awake()
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

    public void Heal(float amount)
    {
        health = (health + amount < maxHealth) ? health + amount : maxHealth;
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

    public float Health
    {
        get { return health; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
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
}
