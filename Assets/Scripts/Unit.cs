﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
	public enum Teams
	{
		Vikings,
		Invaders
	}

	public Teams team;
	public float runningSpeed;
	public float walkingSpeed;
	public float maxHealth;
	public float health;

	public float charge;
	public bool attacking;
	public bool rolling;
	public bool dodging;
	public bool charging;
	public bool hitstun;

	private Animator animator;
	private CharacterController mover;

	//private int baseLayer;

	bool Busy()
	{
		return attacking || rolling || hitstun || !enabled;
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
		animator.SetTrigger("Charge");
		Debug.Log("ATTACK START");
	}

	public void ChargeAttack(float time)
	{
		charge += time;
	}

	public void ReleaseAttack()
	{
		if (charge <= 0 || Busy()) return;
		if (charge > 0.5f)
			Debug.Log("STRONG");
		else
			Debug.Log("WEAK");
		animator.SetTrigger("Attack");
	}

	public void Damage(float damage)
	{
		if (!dodging)
		{
			health -= damage;
			animator.SetTrigger("Damage");
		}

		if (health <= 0)
		{
			animator.SetTrigger("Die");
			GetComponent<CharacterController>().enabled = false;
			this.enabled = false;
		}
	}

	public void Roll(Vector3 wishDir)
	{
		if (Busy()) return;
		animator.SetTrigger("Roll");
	}

	void Update()
	{
		if (charging)
			charge += Time.deltaTime;
		else
			charge = 0;
	}
}
