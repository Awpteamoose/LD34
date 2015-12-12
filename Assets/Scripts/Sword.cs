using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class Sword : MonoBehaviour
{
	public GameObject owner;
	public float damage;

	void Start()
	{
		Assert.IsNotNull(owner);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == owner) return;

		var unit = other.GetComponent<Unit>();
		if (unit)
		{
			unit.Damage(damage);
			Debug.Log("SWORD STRIKE");
		}
	}
}
