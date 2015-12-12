using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public Unit player;
	public Camera camera;

	private float leftHeld;

	private Vector3 wishDir;
	private float wishLength;

	public static string[] debug = new string[10];

	void CountHeldTime()
	{
		if (Input.GetMouseButtonDown(0))
			leftHeld = 0f;

		if (Input.GetMouseButton(0))
			leftHeld += Time.deltaTime;
	}

	// Update is called once per frame
	void Update ()
	{
		CountHeldTime();

		var mouseDirection = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y) - new Vector3(Screen.width * 0.5f, 0, Screen.height * 0.5f);
		wishLength = new Vector3(mouseDirection.x / Screen.width * 2f, mouseDirection.z / Screen.height * 2f).magnitude;
		wishDir = mouseDirection.normalized;

		player.Look(wishDir);

		if (Input.GetMouseButton(0))
		{
			player.Move(wishDir, (wishLength >= 0.25f));
			Debug.DrawRay(player.transform.position, wishDir);
		}

		if (Input.GetMouseButtonUp(0))
		{
			player.Stop();
			if (leftHeld <= 0.2f)
				player.Roll(wishDir);
		}

		if (Input.GetMouseButtonDown(1))
			player.StartAttack();

		if (Input.GetMouseButton(1))
			player.ChargeAttack(Time.deltaTime);

		if (Input.GetMouseButtonUp(1))
			player.ReleaseAttack();

		// Uncomment to each timestep look at player
		camera.transform.position = player.transform.position;
		camera.transform.position += 15 * (camera.transform.rotation * Vector3.back);

		debug[0] = "Attacking? " + player.attacking + "; HP: " + player.health;
		debug[1] = "Rolling? " + player.rolling;
	}

	void OnGUI()
	{
		if (Application.isEditor)
		{
			for (var i = 0; i < debug.Length; i++)
				GUI.Label(new Rect(new Vector2(0, i * 25), new Vector2(500, 25)), debug[i]);
		}
	}
}
