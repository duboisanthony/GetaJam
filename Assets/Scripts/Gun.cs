using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public GunType type;
	public GameObject bullet;
	public Transform firePoint;
	public float fireRate = 1;
	public float damage = 1;
	public float bulletLifetime = 3;
	public float bulletSpeed = 500;
	public FireMode fireMode = FireMode.Spray;
	public bool isShotgun = false;
	public bool hasUnlimitedAmmo = false;
	public bool isOnGround = false;
	public GameObject pistolPrefab;
	public GameObject shotgunPrefab;
	public GameObject m4Prefab;

	public void Fire(Vector3 direction)
	{
		if (!isShotgun)
		{
			Bullet bulletScript = Instantiate(bullet, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
			bulletScript.speed = bulletSpeed;
			bulletScript.damage = damage;
			bulletScript.lifetime = bulletLifetime;
			bulletScript.direction = direction;
		}
		else
			StartCoroutine(FireFromShotgun(direction));
	}

	IEnumerator FireFromShotgun(Vector3 direction)
	{
		GameObject bulletG = Instantiate(bullet, firePoint.position, Quaternion.identity);
		bulletG.transform.rotation = Quaternion.Euler(0, 15, 0);
		Bullet bulletScript = bulletG.GetComponent<Bullet>();
		bulletScript.speed = bulletSpeed;
		bulletScript.damage = damage;
		bulletScript.lifetime = bulletLifetime;
		bulletScript.direction = direction;

		yield return new WaitForSeconds(0.01f);

		GameObject bulletG2 = Instantiate(bullet, firePoint.position, Quaternion.identity);
		Bullet bulletScript2 = bulletG2.GetComponent<Bullet>();
		bulletScript2.speed = bulletSpeed;
		bulletScript2.damage = damage;
		bulletScript2.lifetime = bulletLifetime;
		bulletScript2.direction = direction;

		yield return new WaitForSeconds(0.01f);

		GameObject bulletG3 = Instantiate(bullet, firePoint.position, Quaternion.identity);
		bulletG3.transform.rotation = Quaternion.Euler(0, -15, 0);
		Bullet bulletScript3 = bulletG3.GetComponent<Bullet>();
		bulletScript3.speed = bulletSpeed;
		bulletScript3.damage = damage;
		bulletScript3.lifetime = bulletLifetime;
		bulletScript3.direction = direction;
	}

	void OnTriggerStay(Collider collider)
	{
		if (collider.tag == "Player" && isOnGround)
		{
			if (Input.GetButtonDown("Pick up"))
			{
				PlayerController player = collider.GetComponent<PlayerController>();

				if (type == GunType.Pistol)
					player.ChangeGun(pistolPrefab);
				else if (type == GunType.Shotgun)
					player.ChangeGun(shotgunPrefab);
				else if (type == GunType.M4)
					player.ChangeGun(m4Prefab);

				Destroy(gameObject);
			}
		}
	}

	public enum FireMode
	{
		Single,
		Spray
	}
}

public enum GunType
{
	Pistol,
	Shotgun,
	M4
}
