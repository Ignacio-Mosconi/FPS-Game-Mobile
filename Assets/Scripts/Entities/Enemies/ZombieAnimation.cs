using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ZombieMovement))]
[RequireComponent(typeof(Life))]

public class ZombieAnimation : MonoBehaviour
{
    [SerializeField] AnimationClip hitAnimation;
    Animator animator;
    NavMeshAgent agent;
    ZombieMovement zombieMovement;
    ZombieAttacking zombieAttacking;
    Life zombieLife;

	void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        zombieMovement = GetComponent<ZombieMovement>();
        zombieAttacking = GetComponent<ZombieAttacking>();
        zombieLife = GetComponent<Life>();

	}
    
	void Start()
    {
        zombieLife.OnHit.AddListener(HitAnimation);
        zombieLife.OnDeath.AddListener(DeathAnimation);
        zombieAttacking.OnAttack.AddListener(AttackAnimation);
    }

	void Update()
    {
        Vector3 horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        float normalizedVelocity = horizontalVelocity.magnitude / zombieMovement.MaxSpeed;

        animator.SetFloat("Horizontal Velocity", normalizedVelocity, 0.2f, Time.deltaTime);
	}

    void HitAnimation()
    {
        agent.isStopped = true;
        animator.SetTrigger("Was Hit");
        Invoke("WalkAgain", hitAnimation.length);
    }

    void DeathAnimation()
    {
        agent.isStopped = true;
        animator.SetTrigger("Death");
    }

    void WalkAgain()
    {
        if (zombieLife.Health > 0)
            agent.isStopped = false;
    }

    void AttackAnimation()
    {
        animator.SetTrigger("Has Attacked");
    }
}
