using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public static MusicPlayer instance;

	public AudioClip titleMusic;
	public AudioClip mainMusic;

	AudioSource audioS;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		audioS = GetComponent<AudioSource>();
	}

	public void PlayMusic(MusicToPlay toPlay)
	{
		if (toPlay == MusicToPlay.Title)
		{
			audioS.clip = titleMusic;
			audioS.Play();
		}
		else if (toPlay == MusicToPlay.Main)
		{
			audioS.clip = mainMusic;
			audioS.Play();
		}
	}
}

public enum MusicToPlay
{
	Title,
	Main
}
