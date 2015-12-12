using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
	public float runningSpeed;
	public float walkingSpeed;
	public float maxHealth;
	public float health;

	public bool attacking;
	public bool rolling;
	public bool dodging;

	private Animator animator;
	private CharacterController mover;
	private float charge;

	//private int baseLayer;

	bool Busy()
	{
		return attacking || rolling;
	}

	void Awake()
	{
		mover = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		//baseLayer = animator.GetLayerIndex("Base Layer");
	}

	public void Move(Vector3 wishDir, bool running)
	{
		if (Busy()) return;
		if (running && charge <= 0)
		{
			animator.SetFloat("Speed", runningSpeed);
			mover.SimpleMove(wishDir * runningSpeed);
		}
		else
		{
			animator.SetFloat("Speed", walkingSpeed);
			mover.SimpleMove(wishDir * walkingSpeed);
		}
	}

	public void Stop()
	{
		animator.SetFloat("Speed", 0);
	}

	public void Look(Vector3 wishDir)
	{
		if (Busy()) return;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wishDir), Time.deltaTime * 25f);
	}

	public void StartAttack()
	{
		if (Busy()) return;
		Debug.Log("CHARGING ATTACK");
	}

	public void ChargeAttack(float time)
	{
		charge += time;
	}

	public void ReleaseAttack()
	{
		if (charge <= 0 && !attacking && !rolling) return;
		if (charge > 0.5f)
			Debug.Log("STRONG");
		else
			Debug.Log("WEAK");
		animator.SetTrigger("Attack");
		charge = 0;
	}

	public void Damage(float damage)
	{
		if (!dodging)
			health -= damage;
	}

	public void Roll(Vector3 wishDir)
	{
		animator.SetTrigger("Roll");
	}
}
