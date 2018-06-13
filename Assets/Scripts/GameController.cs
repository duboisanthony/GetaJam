using UnityEngine;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
	public static GameController instance;

	public int score;
	public int highScore;
	public GameObject playerPrefab;
	public GameObject slimePrefab;
	public GameObject boxPrefab;
	public bool isTutorial = false;
	public float phaseDuration = 10;
	public bool isPlayerPhase = true;
	public GameObject defaultGunPrefab;

	UIController ui;
	CameraController cameraC;
	MusicPlayer music;
	int tutorialTimesClicked = 0;
	float phaseTimer;
	bool isInMenu = true;
	int slimePhasesSurvived = 1;
	string folderPath;
	string highScorePath;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		ui = UIController.instance;
		cameraC = CameraController.instance;
		music = MusicPlayer.instance;

		music.PlayMusic(MusicToPlay.Title);
		phaseTimer = phaseDuration;

		folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slime Shooter");
		highScorePath = Path.Combine(folderPath, "hight-score.txt");

		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

		LoadHighScore();
		ui.SetHightScore(highScore);
	}

	void Update()
	{
		if (isTutorial && Input.GetMouseButtonDown(0))
		{
			if (tutorialTimesClicked == 0)
			{
				ui.TutorialNextPage();
				tutorialTimesClicked++;
			}
			else if (tutorialTimesClicked == 1)
			{
				ui.EndTutorial();
				tutorialTimesClicked = 0;
				isTutorial = false;
			}
		}

		if (phaseTimer <= 0)
		{
			phaseTimer = phaseDuration;

			if (isPlayerPhase)
			{
				isPlayerPhase = false;
				ui.SetSlimePhase();

				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().controllBlocked = true;
				GameObject[] slimes = GameObject.FindGameObjectsWithTag("Monster");

				foreach (GameObject s in slimes)
					s.GetComponent<MonsterController>().controllBlocked = false;

				slimePhasesSurvived++;
			}
			else
			{
				isPlayerPhase = true;
				ui.SetPlayerPhase();

				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().controllBlocked = false;
				GameObject[] slimes = GameObject.FindGameObjectsWithTag("Monster");

				foreach (GameObject s in slimes)
					s.GetComponent<MonsterController>().controllBlocked = true;

				for (int i = 0; i < slimePhasesSurvived; i++)
					SpawnPrefab(slimePrefab);

				int r = UnityEngine.Random.Range(0, 100);
				if (r < 50)
					SpawnPrefab(boxPrefab);
			}
		}

		if (!isInMenu)
			phaseTimer -= Time.deltaTime;
	}

	GameObject SpawnPrefab(GameObject prefab)
	{
		Vector3 pos = new Vector3(0, 0, 0);
		bool ok = false;

		while (!ok)
		{
			Vector3 _pos = new Vector3(UnityEngine.Random.Range(-49, 49), 0, UnityEngine.Random.Range(-49, 49));
			Collider[] colliders = Physics.OverlapSphere(_pos, 0.7f);

			foreach (Collider c in colliders)
			{
				if (c.tag == "SpawnBlocked")
					break;
				else
				{
					ok = true;
					pos = _pos;
					break;
				}
			}
		}

		return Instantiate(prefab, pos, Quaternion.identity);
	}

	public void StartGame()
	{
		GameObject player = SpawnPrefab(playerPrefab);
		PlayerController playerS = player.GetComponent<PlayerController>();
		playerS.ChangeGun(defaultGunPrefab);
		cameraC.target = player.transform;
		ui.SetActivePlayerHealthBar(true);
		ui.UpdateBar(playerS.health, playerS.maxHealth);
		music.PlayMusic(MusicToPlay.Main);

		SpawnPrefab(slimePrefab);
		SpawnPrefab(slimePrefab);

		SpawnPrefab(boxPrefab);

		isInMenu = false;
	}

	public void PlayerDied()
	{
		cameraC.target = null;
		ui.SetActivePlayerHealthBar(false);
		ui.SetGameMenuActive(false);
		ui.SetMainMenuActive(true);
		music.PlayMusic(MusicToPlay.Title);

		isInMenu = true;

		GameObject[] slimes = GameObject.FindGameObjectsWithTag("Monster");

		foreach (GameObject s in slimes)
			Destroy(s);

		GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

		foreach (GameObject b in boxes)
			Destroy(b);

		ui.SetMainMenuScore(score);

		if (score > highScore)
		{
			highScore = score;
			SaveHighScore();
			ui.SetHightScore(highScore);
		}
	}

	public void StartTutorial()
	{
		isTutorial = true;
	}

	public void MonsterKilled(MonsterType type)
	{
		if (type == MonsterType.Slime)
			score += 10;

		ui.UpdateScoreText(score);
	}

	public void Quit()
	{
		Application.Quit();
	}

	void LoadHighScore()
	{
		if (!File.Exists(highScorePath))
			File.WriteAllText(highScorePath, "0");

		highScore = int.Parse(File.ReadAllText(highScorePath));
	}

	void SaveHighScore()
	{
		File.WriteAllText(highScorePath, highScore.ToString());
	}
}
