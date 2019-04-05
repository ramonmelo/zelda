using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Crab : MonoBehaviour
{
	enum Direction : int
	{
		DOWN = 0,
		LEFT = 90,
		UP = 180,
		RIGHT = 270,
	}

	public float Speed;
	public int Health;
	public GameObject DeathEffect;

	private Array dirs;
	private System.Random randomizer = new System.Random();
	private Stopwatch movementTimer;
	private float nextMove = 0;
	private Direction currDirection;
	private Vector3 direction;

	void Start()
	{
		dirs = Enum.GetValues(typeof(Direction));
	}

	// Update is called once per frame
	void Update()
	{
		if (Health <= 0)
		{
			Instantiate(DeathEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}

		Movement();
	}

	void Movement()
	{
		nextMove -= Time.deltaTime;

		if (nextMove <= 0)
		{
			NewDirection();
		}

		var velocity = direction * Speed * Time.deltaTime;
		transform.Translate(velocity);
	}

	private void NewDirection()
	{
		nextMove = randomizer.Next(1, 3);
		currDirection = (Direction) dirs.GetValue(randomizer.Next(dirs.Length));

		transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, (float) currDirection));

		switch (currDirection)
		{
			case Direction.DOWN:
				direction = -transform.up;
				break;
			case Direction.RIGHT:
				direction = transform.right;
				break;
			case Direction.UP:
				direction = transform.up;
				break;
			case Direction.LEFT:
				direction = -transform.right;
				break;
		}
	}

	void TakeDamage(int amount)
	{
		Health--;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Sword"))
		{
			TakeDamage(1);
		}
	}

	void HandleCollision(Collision2D other)
	{
		if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy"))
		{
			NewDirection();
		}
	}

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			TakeDamage(1);

			var player = other.gameObject.GetComponent<Player>();
			if (player != null)
			{
				player.TakeDamage(1);
			}
		}

		HandleCollision(other);
	}

	void OnCollisionStay2D(Collision2D other)
	{
		HandleCollision(other);
	}
}