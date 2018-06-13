using UnityEngine;

public class BoxController : MonoBehaviour
{
	public GameObject pistol;
	public GameObject shotgun;
	public GameObject m4;

	void SpawnGun()
	{
		int r = Random.Range(0, 100);

		if (r <= 25)
		{
			GameController game = GameController.instance;
			game.score += 40;
			UIController.instance.UpdateScoreText(game.score);
		}
		else if (r > 25 && r <= 50)
			Instantiate(pistol, transform.position + new Vector3(0, 0.25f), Quaternion.Euler(0, 90, 0)).GetComponent<Gun>().isOnGround = true;
		else if (r > 50 && r <= 75)
			Instantiate(shotgun, transform.position + new Vector3(0, 0.25f), Quaternion.Euler(0, 90, 0)).GetComponent<Gun>().isOnGround = true;
		else if (r > 75)
			Instantiate(m4, transform.position + new Vector3(0, 0.25f), Quaternion.Euler(0, 90, 0)).GetComponent<Gun>().isOnGround = true;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Bullet")
		{
			Destroy(collision.gameObject);
			SpawnGun();
			Destroy(transform.parent.gameObject);
		}
	}
}
