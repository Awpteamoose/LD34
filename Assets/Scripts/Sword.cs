using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class Sword : MonoBehaviour
{
	public Unit owner;
	public float damage;

	void Start()
	{
		Assert.IsNotNull(owner);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == owner) return;

		var unit = other.GetComponent<Unit>();
		if (unit && unit.team != owner.team)
		{
			unit.Damage(damage);
			Debug.Log("SWORD STRIKE");
		}
	}
}
