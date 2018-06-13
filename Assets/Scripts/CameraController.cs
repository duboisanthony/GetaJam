using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController instance;

	public Transform target;
	public Vector3 offset;
	public float smoothTime = 0.5f;

	Vector3 velocity = Vector3.zero;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		if (target == null)
			return;

		transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, smoothTime);
	}
}
