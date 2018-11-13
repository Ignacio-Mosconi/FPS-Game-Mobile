using UnityEngine;

public class AttackBox : MonoBehaviour
{
	[SerializeField] [Range(0f, 100f)] float attackDamage;
	
	const float ATTACK_DAMAGE_DELTA = 2f;
	
	void Start()
	{
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Life targetLife = other.transform.GetComponent<Life>();
			targetLife.TakeDamage(attackDamage + Random.Range(-ATTACK_DAMAGE_DELTA, ATTACK_DAMAGE_DELTA));
			gameObject.SetActive(false);
		}
	}
}
