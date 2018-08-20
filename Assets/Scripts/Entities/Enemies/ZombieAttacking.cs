using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class ZombieAttacking : MonoBehaviour
{
    [SerializeField] float attackDamage;
    [SerializeField] AnimationClip attackAnimation;
    [SerializeField] AudioSource zombieAttackSound;
    ZombieMovement zombieMovement;
    Life zombieLife;
    Life targetLife;
    bool isAttacking;

	void Awake()
    {
        zombieMovement = GetComponent<ZombieMovement>();
        zombieLife = GetComponent<Life>();

        zombieMovement.OnAttackRange.AddListener(FocusOnTarget);
        zombieMovement.OutOfAttackRange.AddListener(LeaveTarget);
	}
	
    void FocusOnTarget()
    {
        targetLife = zombieMovement.FollowingTarget.GetComponent<Life>();
        isAttacking = true;
        zombieAttackSound.Play();

        InvokeRepeating("Attack", attackAnimation.length / 2, attackAnimation.length / 2);
    }

	void Attack()
    {
        if (zombieLife.Health <= 0)
            LeaveTarget();
        else
            targetLife.TakeDamage(attackDamage);
	}

    void LeaveTarget()
    {
        isAttacking = false;
        zombieAttackSound.Stop();
        CancelInvoke("Attack");
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
    }
}
