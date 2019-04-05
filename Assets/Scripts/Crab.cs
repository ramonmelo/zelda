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
			nextMove = randomizer.Next(1, 3);
			currDirection = NewDirection();

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

		var velocity = direction * Speed * Time.deltaTime;
		transform.Translate(velocity);
	}

	private Direction NewDirection()
	{
		return (Direction) dirs.GetValue(randomizer.Next(dirs.Length));
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Sword"))
		{
			Health--;
		}
	}
}