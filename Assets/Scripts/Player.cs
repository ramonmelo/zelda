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

	private Animator anim;
	private int currHealth;
	private int currDir;

	// Start is called before the first frame update
	void Start()
	{
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

		if (Input.GetKeyDown(KeyCode.F))
		{
			Attack();
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
			if (currHealth <= maxHealth)
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

		canMove = currHealth == maxHealth;
		canAttack = false;

		var newSword = Instantiate(sword, transform.position, sword.transform.rotation);
		newSword.transform.Rotate(Vector3.back * currDir * 90);

		void recoverMovement() { 
			canMove = true; 
			canAttack = true;	
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

	void TakeDamage(int amount)
	{
		currHealth -= amount;

		if (currHealth < 0)
		{
			currHealth = 0;
		}
	}

	void Heal(int amount)
	{
		currHealth += amount;

		if (currHealth > maxHealth)
		{
			currHealth = maxHealth;
		}
	}
}