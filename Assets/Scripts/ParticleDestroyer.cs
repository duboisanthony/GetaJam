using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
	public float duration = 1;

	void Start()
	{
		Destroy(gameObject, duration);
	}
}
