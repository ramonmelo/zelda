using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject destroyEffect;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			var player = other.gameObject.GetComponent<Player>();
			if (player != null)
			{
				player.TakeDamage(1);
			}

			Destroy(gameObject);
			Instantiate(destroyEffect, transform.position, transform.rotation);
		}
	}
}