using UnityEngine;

public class Bullet : MonoBehaviour
{
	[HideInInspector]
	public float speed;
	[HideInInspector]
	public float damage;
	[HideInInspector]
	public float lifetime;
	[HideInInspector]
	public Vector3 direction;

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		rb.velocity = transform.rotation * direction * speed * Time.deltaTime;
	}

	void Update()
	{
		if (lifetime <= 0)
			Destroy(gameObject);

		lifetime -= Time.deltaTime;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Obstacle")
			Destroy(gameObject);
		else if (collision.transform.tag == "Monster")
		{
			collision.transform.GetComponent<MonsterController>().health -= damage;
			Destroy(gameObject);
		}
	}
}
