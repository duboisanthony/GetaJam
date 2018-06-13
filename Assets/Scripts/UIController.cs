using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController instance;

	public GameObject playerHealthBarG;
	public Bar playerHealthBarS;
	public Text ammoText;
	public Text scoreText;
	public GameObject mainMenu;
	public GameObject gameMenu;
	public GameObject tutorialMenu;
	public GameObject keyboardPage;
	public GameObject mousePage;
	public Text phaseText;
	public Text menuScoreText;
	public Text hightScoreText;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			playerHealthBarG.SetActive(true);
			playerHealthBarG.transform.position = player.transform.position + new Vector3(0, 1, 0.75f);
		}
		else
			playerHealthBarG.SetActive(false);
	}
	
	public void UpdateBar(float amount, float max)
	{
		playerHealthBarS.UpdateBar(amount, max);
	}

	public void UpdateHealthBar(float current, float max)
	{
		playerHealthBarS.UpdateBar(current, max);
	}

	public void UpdateAmmoText(int ammo, bool isUnlimited)
	{
		if (isUnlimited)
			ammoText.text = "Ammo: unlimited";
		else
			ammoText.text = "Ammo: " + ammo;
	}

	public void UpdateScoreText(int score)
	{
		scoreText.text = score.ToString();
	}

	public void SetActivePlayerHealthBar(bool active)
	{
		playerHealthBarG.SetActive(active);
	}

	public void SetMainMenuActive(bool active)
	{
		mainMenu.SetActive(active);
	}

	public void SetGameMenuActive(bool active)
	{
		gameMenu.SetActive(active);
	}

	public void TutorialNextPage()
	{
		keyboardPage.SetActive(false);
		mousePage.SetActive(true);
	}

	public void EndTutorial()
	{
		mousePage.SetActive(false);
		keyboardPage.SetActive(true);
		tutorialMenu.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void SetPlayerPhase()
	{
		phaseText.text = "Player Phase";
	}

	public void SetSlimePhase()
	{
		phaseText.text = "Slime Phase";
	}

	public void SetMainMenuScore(int score)
	{
		menuScoreText.text = "Score: " + score;
	}

	public void SetHightScore(int score)
	{
		hightScoreText.text = "High Score: " + score;
	}
}
