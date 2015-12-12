using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
	public float runningSpeed;
	public float walkingSpeed;
	public float rollingSpeed;
	public float maxHealth;
	public float health;
	public bool attacking;
	public bool rolling;

	private Animator animator;
	private CharacterController mover;
	private float charge;

	private int baseLayer;

	void Awake()
	{
		mover = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		baseLayer = animator.GetLayerIndex("Base Layer");
	}

	public void Move(Vector3 wishDir, bool running)
	{
		if (attacking || rolling) return;
		if (running && charge <= 0)
			mover.SimpleMove(wishDir * runningSpeed);
		else
			mover.SimpleMove(wishDir * walkingSpeed);
	}

	public void Look(Vector3 wishDir)
	{
		if (attacking || rolling) return;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wishDir), Time.deltaTime * 25f);
	}

	public void StartAttack()
	{
		if (attacking || rolling) return;
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
		if (!rolling)
			health -= damage;
	}

	public void Roll(Vector3 wishDir)
	{
		animator.SetTrigger("Roll");
	}

	void Update()
	{
		if (rolling)
		{
			float rollPoint = animator.GetCurrentAnimatorStateInfo(baseLayer).normalizedTime;
			mover.SimpleMove((transform.rotation * Vector3.forward) * Mathf.Lerp(rollingSpeed, 0, rollPoint));
		}
	}
}
