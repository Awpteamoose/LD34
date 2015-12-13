using UnityEngine;
using System.Collections.Generic;

public class AIController : MonoBehaviour
{
	public float minSeparation;
	public float alignment;
	public float cohesion;
	public float separationCoef;

	public List<Unit> controlled;
	public Unit player;

	private float chargeTo;

	// Update is called once per frame
	void Update ()
	{
		var center = Vector3.zero;
		var direction = Vector3.zero;

		for (var i = 0; i < controlled.Count; i++)
		{
			var unit = controlled[i];
			if (unit.health <= 0)
			{
				var last = controlled[controlled.Count - 1];
				controlled[i] = last;
				controlled.RemoveAt(controlled.Count - 1);
				i--;
				continue;
			}
			center += unit.transform.position;
			direction += unit.transform.rotation * Vector3.forward;
		}
		center /= controlled.Count;
		center.y = 0;

		direction.y = 0;
		direction.Normalize();

		foreach (var unit in controlled)
		{
			Debug.DrawLine(unit.transform.position, center, Color.red);
			Debug.DrawRay(center, direction, Color.green);

			var desired = player.transform.position - unit.transform.position;
			desired.y = 0;
			var toPlayer = desired.magnitude;
			desired.Normalize();

			var toCenter = center - unit.transform.position;
			toCenter.y = 0;
			toCenter.Normalize();
			desired += toCenter * alignment;
			desired += direction * cohesion;

			foreach (var other in controlled)
			{
				if (other == unit) continue;
				var separation = other.transform.position - unit.transform.position;
				Debug.DrawRay(unit.transform.position, separation.normalized, Color.blue);
				if (separation.magnitude >= minSeparation) continue;
				var separatingDir = separation.normalized;
				desired += (separatingDir / separation.magnitude) * separationCoef;
			}

			desired.Normalize();
			unit.Look(desired);

			if (toPlayer < 1f)
			{
				if (unit.charge <= 0)
				{
					unit.StartAttack();
					chargeTo = Random.Range(0.2f, 5f);
				}
			}
			else if (unit.charge <= 0)
			{
				unit.Move(desired, true);
			}

			if (unit.charge > 0)
			{
				if (unit.charge >= chargeTo)
					unit.ReleaseAttack();
				unit.Move(desired, true);
			}
		}
	}
}
