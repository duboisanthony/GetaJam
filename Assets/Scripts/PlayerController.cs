using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 250;
	public Transform gunPosition;
	public float maxHealth = 10;
	public float health;
	public float dashForce = 2;
	public float dashCooldown = 1;
	public float dashDuration = 0.5f;
	public GameObject dashTrail;
	public GameObject dashParticles;
	public int ammo = 12;
	public bool controllBlocked = false;

	Rigidbody rb;
	UIController ui;
	AudioPlayer audioP;
	SwitchWeapon weaponImage;
	Gun gunS = null;
	float fireTimer;
	GameObject equippedGun = null;
	Vector3 direction;
	float dashTimer;
	DashState dashState = DashState.Ready;
	Vector3 dashVelocity;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		ui = UIController.instance;
		audioP = AudioPlayer.instance;
		weaponImage = GameObject.FindGameObjectWithTag("WeaponImage").GetComponent<SwitchWeapon>();

		health = maxHealth;
		weaponImage.ChangeWeapon(GunType.Pistol);
	}

	void Update()
	{
		if (health <= 0)
		{
			GameController.instance.PlayerDied();
			Destroy(gameObject);
		}

		if (controllBlocked)
		{
			rb.velocity = new Vector3(0, 0, 0);
			return;
		}

		Movement();
		RotateCharacter();
		Dash();
		Shooting();

		if (equippedGun != null)
			equippedGun.transform.position = gunPosition.position;
		
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
	}

	void Dash()
	{
		if (dashState == DashState.Ready)
		{
			if (Input.GetButtonDown("Dash") && rb.velocity != Vector3.zero)
			{
				dashState = DashState.Dashing;
				dashVelocity = rb.velocity;
				rb.AddForce(dashVelocity * dashForce);
				dashTimer = dashDuration;
				dashTrail.SetActive(true);
				Instantiate(dashParticles, transform.position, Quaternion.identity);
				audioP.PlaySound(AudioToPlay.Dash);
			}
		}
		else if (dashState == DashState.Dashing)
		{
			rb.velocity = dashVelocity * dashForce * Time.deltaTime;

			if (dashTimer <= 0)
			{
				dashState = DashState.Cooldown;
				dashTimer = dashCooldown;
				dashTrail.SetActive(false);
			}

			dashTimer -= Time.deltaTime;
		}
		else if (dashState == DashState.Cooldown)
		{
			if (dashTimer <= 0)
				dashState = DashState.Ready;

			dashTimer -= Time.deltaTime;
		}
	}

	void RotateCharacter()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			Vector3 lookPosition = hit.point;
			lookPosition.y = transform.position.y;
			direction = (lookPosition - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			transform.rotation = lookRotation;
			if (equippedGun != null)
				equippedGun.transform.rotation = lookRotation;
		}
	}

	public void ChangeGun(GameObject gunPrefab)
	{
		Destroy(equippedGun);
		equippedGun = Instantiate(gunPrefab, gunPosition.position, Quaternion.identity);
		gunS = equippedGun.GetComponent<Gun>();

		if (gunS.hasUnlimitedAmmo)
			UIController.instance.UpdateAmmoText(0, true);
		else
			UIController.instance.UpdateAmmoText(ammo, false);

		GameObject.FindGameObjectWithTag("WeaponImage").GetComponent<SwitchWeapon>().ChangeWeapon(gunS.type);
	}

	void Shooting()
	{
		if (gunS == null)
			return;

		if (gunS.fireMode == Gun.FireMode.Single)
		{
			if (Input.GetButtonDown("Fire1") && fireTimer <= 0)
			{
				Fire();
				fireTimer = 1 / gunS.fireRate;
			}
		}
		else if (gunS.fireMode == Gun.FireMode.Spray)
		{
			if (Input.GetButton("Fire1") && fireTimer <= 0)
			{
				Fire();
				fireTimer = 1 / gunS.fireRate;
			}
		}

		fireTimer -= Time.deltaTime;
	}

	void Fire()
	{
		if (gunS.hasUnlimitedAmmo)
			gunS.Fire(direction);
		else if (gunS.isShotgun)
		{
			if ((ammo - 3) >= 0)
			{
				ammo -= 3;
				ui.UpdateAmmoText(ammo, false);
				gunS.Fire(direction);
			}
		}
		else if (!gunS.isShotgun)
		{
			if ((ammo - 1) >= 0)
			{
				ammo--;
				ui.UpdateAmmoText(ammo, false);
				gunS.Fire(direction);
			}
		}

		if (gunS.type == GunType.Pistol)
			audioP.PlaySound(AudioToPlay.Pistol);
		else if (gunS.type == GunType.Shotgun)
			audioP.PlaySound(AudioToPlay.Shotgun);
		else if (gunS.type == GunType.M4)
			audioP.PlaySound(AudioToPlay.M4);
	}

	void Movement()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		rb.velocity = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Monster")
		{
			health -= collision.transform.GetComponent<MonsterController>().damage;
			UIController.instance.UpdateHealthBar(health, maxHealth);
		}
	}

	enum DashState
	{
		Ready,
		Dashing,
		Cooldown
	}
}
