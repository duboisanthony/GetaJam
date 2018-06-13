using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
	public MonsterType type;
	public float health = 3;
	public float damage = 1;
	public float speed = 2;
	public GameObject hearthPowerUp;
	public GameObject ammoBoxPowerUp;
	public bool controllBlocked = true;

	Transform target;
	NavMeshAgent agent;
	float setDestinationTimer = 0.5f;
	float cooldownTimer = 1.5f;
	bool cooldownTimerActive = false;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Player").transform;

		agent.speed = speed;
	}

	void Update()
	{
		if (health <= 0)
		{
			GameController.instance.MonsterKilled(type);
			SpawnPowerUp();
			AudioPlayer.instance.PlaySound(AudioToPlay.SlimeDie);
			Destroy(gameObject);
		}

		if (setDestinationTimer <= 0)
		{
			agent.SetDestination(target.position);
			setDestinationTimer = 0.5f;
		}

		if (cooldownTimer <= 0)
		{
			agent.speed = speed;
			cooldownTimer = 1.5f;
			cooldownTimerActive = false;
		}

		if (controllBlocked)
			agent.speed = 0;
		else
			agent.speed = speed;

		setDestinationTimer -= Time.deltaTime;

		if (cooldownTimerActive)
			cooldownTimer -= Time.deltaTime;
	}

	public void SpawnPowerUp()
	{
		int r = Random.Range(0, 100);

		if (r <= 10)
			Instantiate(ammoBoxPowerUp, transform.position, Quaternion.identity);
		else if (r > 10 && r <= 20)
			Instantiate(hearthPowerUp, transform.position, Quaternion.identity);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player")
		{
			agent.speed = 0;
			cooldownTimerActive = true;
		}
	}
}

public enum MonsterType
{
	Slime
}
