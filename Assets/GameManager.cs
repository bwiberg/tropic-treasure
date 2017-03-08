using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get {
			if (!instance) {
				instance = GameObject.FindGameObjectWithTag(Tags.GameManager).GetComponent<GameManager>();
			}
			return instance;
		}
	}

	private static GameManager instance;

	public GameObject score;
	public GameObject enterNameScoreDisplay;
	public GameObject gameOverScoreDisplay;
	public GameObject nameInput;
	public GameObject nameMenuPanel;
	public GameObject leaderboardMenuPanel;
	public GameObject playMenuPanel;

	public GameObject temple;
	public GameObject treasure;
	public GameObject enemies;

	public GameObject windPowerUp;
	public GameObject trapPowerUp;
	public GameObject earthPowerUp;
	public GameObject ballPowerUp;

	public PirateShip pirateShip;

	private const string mainNameKey = "HighscoreName";
	private const string mainScoreKey = "HighscoreScore";

	private int place;
	public int accomplishedScore {get; set; }
	public string playerName {get; set;}

	private bool lost;
	private bool isFinished;

	private List<SingleTouchRotationGesture> levels;

	// Use this for initialization
	void Start () {
		lost = false;
		isFinished = false;
		accomplishedScore = 0;
		place = 0;

		if(temple == null) {
			temple = GameObject.FindGameObjectWithTag(Tags.CircularTemple);
		}
	}
	
	// Update is called once per frame
	void Update () {
		lost = treasure.GetComponent<TreasureState>().isEmpty;

		if(lost) {
			FinishGame();
			lost = false;
		}
	}

	void FindLevels()
	{
		levels = new List<SingleTouchRotationGesture>();
		foreach(Transform level in temple.transform)
			levels.Add(level.GetComponent<SingleTouchRotationGesture>());
	}

	void FinishGame()
	{
		if(isFinished)
			return;

		DisableGameInput();
		accomplishedScore = score.GetComponent<DisplayScore>().getScore();

		int trdBestScore = PlayerPrefs.GetInt(mainScoreKey + "3.", 0);
		int sndBestScore = PlayerPrefs.GetInt(mainScoreKey + "2.", 0);
		int fstBestScore = PlayerPrefs.GetInt(mainScoreKey + "1.", 0);

		if(accomplishedScore > trdBestScore) {
			place = 3;
			if(accomplishedScore > sndBestScore) {
				place = 2;
				if(accomplishedScore > fstBestScore) {
					place = 1;
				}
			}
		}

		// Open name enter panel if user achieved a score higher than the third person
		if(place > 0)
		{
			Text scoreText = enterNameScoreDisplay.GetComponent<Text>();
			scoreText.text = "Score: " + accomplishedScore.ToString();
			nameMenuPanel.SetActive(true);
		}
		else
		{
			Text scoreText = gameOverScoreDisplay.GetComponent<Text>();
			scoreText.text = "Score: " + accomplishedScore.ToString();
			leaderboardMenuPanel.SetActive(true);
		}

		isFinished = true;
		return;
	}

	public void StoreScoreAndName() {
		playerName = nameInput.GetComponent<InputField>().text;
		PlayerPrefs.SetInt(mainScoreKey + place.ToString() + ".", accomplishedScore);
		PlayerPrefs.SetString(mainNameKey + place.ToString() + ".", playerName);
	}

	public void DisableGameInput() {
		FindLevels();
		enemies.SetActive(false);
		for(int i=0; i < levels.Count; i++)
		{
			levels[i].enabled = false;
		}
		playMenuPanel.SetActive(false);
	}

	public void EnableGameInput() {
		FindLevels();
		enemies.SetActive(true);
		for(int i=0; i < levels.Count; i++)
		{
			levels[i].enabled = true;
		}
		playMenuPanel.SetActive(true);
	}

	public void FireCannonballAtSegment(GameObject segment, Vector3 target) {
		pirateShip.FireCannonballAtSegment(segment, target);
	}
}
