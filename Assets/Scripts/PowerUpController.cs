using UnityEngine;

public class PowerUpController : MonoBehaviour
{
	public PowerUpType type;

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			if (type == PowerUpType.Hearth)
			{
				PlayerController player = collider.GetComponent<PlayerController>();
				player.health = (player.health + 2) - ((player.health + 2) - player.maxHealth);
				UIController.instance.UpdateHealthBar(player.health, player.maxHealth);
				Destroy(gameObject);
			}
			else if (type == PowerUpType.AmmoBox)
			{
				PlayerController player = collider.GetComponent<PlayerController>();
				player.ammo += 40;
				UIController.instance.UpdateAmmoText(player.ammo, false);
				Destroy(gameObject);
			}

			AudioPlayer.instance.PlaySound(AudioToPlay.PowerUp);
		}
	}
}

public enum PowerUpType
{
	Hearth,
	AmmoBox
}
