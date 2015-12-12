using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
	public GameObject owner;
	public float damage;

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
