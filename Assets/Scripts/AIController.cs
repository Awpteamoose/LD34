using UnityEngine;
using System.Collections.Generic;

public class AIController : MonoBehaviour
{
	public List<Unit> controlled;
	public Unit player;

	private float chargeTo;
	// Update is called once per frame
	void Update ()
	{
		foreach (var unit in controlled)
		{
			var unitToPlayer = player.transform.position - unit.transform.position;
			unitToPlayer.y = 0;
			unit.Look(unitToPlayer.normalized);

			if (unitToPlayer.magnitude < 1f)
			{
				if (unit.charge <= 0)
				{
					unit.StartAttack();
					chargeTo = Random.Range(0.2f, 1f);
				}
			}
			else if (unit.charge <= 0)
			{
				var dir = unitToPlayer.normalized;
				unit.Move(dir, true);
			}

			if (unit.charge > 0)
			{
				if (unit.charge >= chargeTo)
					unit.ReleaseAttack();
				var dir = unitToPlayer.normalized;
				unit.Move(dir, true);
			}
		}
	}
}
