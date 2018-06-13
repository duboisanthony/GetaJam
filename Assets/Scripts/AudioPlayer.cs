using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	public static AudioPlayer instance;

	public AudioClip dash;
	public AudioClip pistol;
	public AudioClip shotgun;
	public AudioClip m4;
	public AudioClip powerUp;
	public AudioClip slimeDie;

	AudioSource audioS;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		audioS = GetComponent<AudioSource>();
	}

	public void PlaySound(AudioToPlay toPlay)
	{
		if (toPlay == AudioToPlay.Dash)
		{
			audioS.clip = dash;
			audioS.Play();
		}
		else if (toPlay == AudioToPlay.Pistol)
		{
			audioS.clip = pistol;
			audioS.Play();
		}
		else if (toPlay == AudioToPlay.Shotgun)
		{
			audioS.clip = shotgun;
			audioS.Play();
		}
		else if (toPlay == AudioToPlay.M4)
		{
			audioS.clip = m4;
			audioS.Play();
		}
		else if (toPlay == AudioToPlay.PowerUp)
		{
			audioS.clip = powerUp;
			audioS.Play();
		}
		else if (toPlay == AudioToPlay.SlimeDie)
		{
			audioS.clip = slimeDie;
			audioS.Play();
		}
	}
}

public enum AudioToPlay
{
	Dash,
	Pistol,
	Shotgun,
	M4,
	PowerUp,
	SlimeDie
}
