using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public GameObject sword;
	public float thrustPower;
	public float speed = 10;
	public Image[] hearths;
	[Range(1, 5)]
	public int maxHealth = 1;

	private bool canMove;
	private bool canAttack;

	private SpriteRenderer spriteRenderer;
	private Animator anim;
	private int currHealth;
	private int currDir;

	private bool invencible;
	private float invencibleTimer = 1f;

	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		currHealth = maxHealth;
		canMove = true;
		canAttack = true;
	}

	// Update is called once per frame
	void Update()
	{
		UpdateHealth();
		Movement();

		if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space))
		{
			Attack();
		}

		if (invencible)
		{
			spriteRenderer.enabled = Random.Range(0, 100) < 50;

			invencibleTimer -= Time.deltaTime;

			if (invencibleTimer <= 0)
			{
				invencible = false;
				invencibleTimer = 1f;
				spriteRenderer.enabled = true;
			}
		}
	}

	private void Movement()
	{
		if (canMove == false)
		{
			return;
		}

		var direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
		var velocity = direction * speed * Time.deltaTime;

		anim.speed = direction.magnitude > 0 ? 1 : 0;

		if (direction.x > 0)
		{
			currDir = 1; // RIGHT
		}
		else if (direction.x < 0)
		{
			currDir = 3; // LEFT
		}

		if (direction.y > 0)
		{
			currDir = 0; // UP
		}
		else if (direction.y < 0)
		{
			currDir = 2; // DOWN
		}

		anim.SetInteger("dir", currDir);

		transform.Translate(velocity);
	}

	void UpdateHealth()
	{
		for (int i = 0; i < maxHealth; i++)
		{
			if (i < currHealth)
			{
				hearths[i].gameObject.SetActive(true);
			}
			else
			{
				hearths[i].gameObject.SetActive(false);
			}
		}
	}

	void Attack()
	{
		if (canAttack == false) { return; }

		anim.SetTrigger("atk");

		canMove = currHealth == maxHealth;
		canAttack = false;

		var newSword = Instantiate(sword, transform.position, sword.transform.rotation);
		newSword.transform.Rotate(Vector3.back * currDir * 90);

		void recoverMovement()
		{
			canMove = true;
			canAttack = true;

			anim.ResetTrigger("atk");
		};

		Sword swordComp = newSword.GetComponent<Sword>();

		swordComp.OnDisappear += recoverMovement;
		swordComp.special = currHealth == maxHealth;

		var rb = newSword.GetComponent<Rigidbody2D>();

		if (rb != null)
		{
			rb.AddForce(newSword.transform.up * thrustPower);
		}
	}

	public void TakeDamage(int amount)
	{
		if (invencible) { return; }

		currHealth -= amount;
		invencible = true;

		if (currHealth < 0)
		{
			currHealth = 0;
		}
	}

	public void Heal(int amount)
	{
		currHealth += amount;

		if (currHealth > maxHealth)
		{
			currHealth = maxHealth;
		}
	}

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.LogFormat("on trigger {0}", other);

		if (other.gameObject.CompareTag("Potion"))
		{
			if (maxHealth < 5)
			{
				maxHealth++;
			}

			currHealth = maxHealth;
			Destroy(other.gameObject);
		}
	}
}