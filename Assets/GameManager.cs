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

	public TempleGenerator templeGenerator;
	private GameObject temple;
	public GameObject treasure;
	public GameObject enemies;

	public GameObject windPowerUp;
	public GameObject trapPowerUp;
	public GameObject earthPowerUp;
	public GameObject ballPowerUp;

	public PirateShip pirateShip;

	public GameObject windHelpText;
	public GameObject windHelpTextEmpty;
	public GameObject windHelpTextFull;

	public List<GameObject> trees;
	public GameObject wind;
	public GameObject ship;

	private float windLoadLevel = 0.0f;
	public float windMaxLevel = 10.0f;

	private const string mainNameKey = "HighscoreName";
	private const string mainScoreKey = "HighscoreScore";

	private int place;
	public int accomplishedScore {get; set; }
	public string playerName {get; set;}

	private bool lost;
	private bool isFinished;
	private bool blinkedOnce = false;

	private List<SingleTouchRotationGesture> levels;

	void Awake() {
		if (!Application.isEditor) {
			templeGenerator.GenerateTemple_InGame();
		}
	}

	// Use this for initialization
	void Start () {
		lost = false;
		isFinished = false;
		accomplishedScore = 0;
		place = 0;

		if(!temple) {
			temple = GameObject.FindGameObjectWithTag(Tags.CircularTemple);
		}
	}
	
	// Update is called once per frame
	void Update () {
		lost = treasure.GetComponent<TreasureState>().isEmpty;

		var isWind = windPowerUp.GetComponent<ChargePowerUp>().isActive;
		if(isWind) {
			ActivateWindPowerUp();
		}
		else {
			DeactivateWindPowerUp();
		}

		if(lost) {
			FinishGame();
			lost = false;
		}
	}

	void DeactivateWindPowerUp()
	{
		if(!windHelpText.GetComponent<Blink>().isBlinking) {
			// Deactivate Help Text
			windHelpText.SetActive(false);

			// Move ship back
			var shipMovement = ship.GetComponent<BlowShipAway>();

			// Reset wind level if ship was gone else keep it
			if(shipMovement.shipIsGone)
				windLoadLevel = 0.0f;

			shipMovement.MoveShipBack();

			// Reset blinking
			blinkedOnce = false;
		}
	}

	void ActivateWindPowerUp() 
	{
		if(!windHelpText.GetComponent<Blink>().isBlinking) {
			// Activate Help Text
			windHelpText.SetActive(true);
		}

		// Integrate Mic loudness
		windLoadLevel += MicInput.loudness;

		// Determine how far the max wind level has been reached
		float partialLoadLevel = windLoadLevel / windMaxLevel;

		// Move ship based on partial level
		var shipMovement = ship.GetComponent<BlowShipAway>();
		shipMovement.MoveShipAway(partialLoadLevel);

		// Once ship moved blink and remove text
		if(shipMovement.shipIsGone && !blinkedOnce){ 
			windHelpText.GetComponent<Blink>().StartBlink();
			blinkedOnce = true;
		}
		if(!windHelpText.GetComponent<Blink>().isBlinking && blinkedOnce)
			windHelpText.SetActive(false);

		// Update charging bar based on partial level
		windHelpTextFull.GetComponent<Image>().fillAmount = partialLoadLevel;

		// Animate wind based on trees
		var ps = wind.GetComponent<ParticleSystem>();
		ps.Emit(Mathf.FloorToInt(MicInput.loudness * 20));

		// Move trees based on Mic Input
		float torque = MicInput.loudness * 20;
		for(int i = 0; i < trees.Count; i++) 
		{
			var rb = trees[i].GetComponent<Rigidbody>();
			rb.AddTorque(new Vector3(torque, torque, torque));
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
}
