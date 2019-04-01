using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
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
				Destroy(gameObject);
			}
		}
		else if (normalTimer < 0)
		{
			OnDisappear?.Invoke();
			Destroy(gameObject);
		}
	}
}