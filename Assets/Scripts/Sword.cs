using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	public GameObject swordParticle;

	public float normalTimer = 0.15f;
	public float specialTimer = 1.0f;
	public bool special;

	public Action OnDisappear;

	void Update()
	{
		normalTimer -= Time.deltaTime;
		specialTimer -= Time.deltaTime;

		if (special)
		{
			if (specialTimer < 0)
			{
				OnDisappear?.Invoke();
				Destroy();

			}
		}
		else if (normalTimer < 0)
		{
			OnDisappear?.Invoke();
			Destroy();
		}
	}

	void Destroy()
	{
		Destroy(gameObject);
		if (special)
		{
			Instantiate(swordParticle, transform.position, Quaternion.identity);
		}
		OnDisappear?.Invoke();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") == false)
		{
			Destroy();
		}
	}
}