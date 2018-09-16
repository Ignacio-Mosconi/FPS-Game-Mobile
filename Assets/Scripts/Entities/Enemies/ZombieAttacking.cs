using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class ZombieAttacking : MonoBehaviour
{
    [SerializeField] AnimationClip attackAnimation;
    [SerializeField] UnityEvent onAttack;
    GameObject attackBox;
    ZombieMovement zombieMovement;
    Life zombieLife;
    Life targetLife;
    bool isAttacking;

	void Awake()
    {
        zombieMovement = GetComponent<ZombieMovement>();
        zombieLife = GetComponent<Life>();
        attackBox = GetComponentInChildren<AttackBox>().gameObject;
	}

    void Start()
    {
        zombieMovement.OnAttackRange.AddListener(FocusOnTarget);
        zombieMovement.OutOfAttackRange.AddListener(LeaveTarget);
        zombieLife.OnHit.AddListener(LeaveTarget);
        zombieLife.OnDeath.AddListener(LeaveTarget);
    }
	
    void FocusOnTarget()
    {
        if (zombieLife.Health >= 0)
        {
            targetLife = zombieMovement.CurrentTarget.GetComponent<Life>();
            isAttacking = true;
            InvokeRepeating("Attack", attackAnimation.length / 2, attackAnimation.length / 2);
        }
    }

	void Attack()
    {
        onAttack.Invoke();

        Invoke("EnableAttackBox", attackAnimation.length / 4);
	}

    void LeaveTarget()
    {
        isAttacking = false;
        attackBox.SetActive(false);
        CancelInvoke("Attack");
        CancelInvoke("EnableAttackBox");
    }

    void EnableAttackBox()
    {
        attackBox.SetActive(true);
        Invoke("DisableAttackBox", attackAnimation.length / 4);

    }

    void DisableAttackBox()
    {
        attackBox.SetActive(false);
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public UnityEvent OnAttack
    {
        get { return onAttack; }
    }
}
