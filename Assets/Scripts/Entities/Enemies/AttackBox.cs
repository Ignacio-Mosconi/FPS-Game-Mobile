using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
	[SerializeField] [Range(0, 100)] float attackDamage;
	const float attackDamageDelta = 2f;
	void Start()
	{
		gameObject.SetActive(false);
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Debug.Log("Hit");
			Life targetLife = other.transform.GetComponent<Life>();
			targetLife.TakeDamage(attackDamage + Random.Range(-attackDamageDelta, attackDamageDelta));
			gameObject.SetActive(false);
		}
	}
}
