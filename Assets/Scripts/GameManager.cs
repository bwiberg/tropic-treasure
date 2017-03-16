using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using EazyTools.SoundManager;

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
	private CircularTemple temple;
	public GameObject treasure;

	public GameObject windPowerUp;
	public GameObject trapPowerUp;
	public GameObject earthPowerUp;
	public GameObject ballPowerUp;
	public RollingBallOfDeath rollingBall;

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

	private SimpleAgent[] enemies;
    private spikeTrigger[] spikes;


    private Rigidbody[] rigidbodies;
	private Tuple<bool, Vector3, Vector3>[] rigidbodyStates;

	private ParticleSystem[] particleSystems;
	private bool[] particleSystemStates;

	// AUDIO STUFF
	private Audio audioOceanWaves;

	void Awake() {
		if (!Application.isEditor) {
			templeGenerator.RandomSeed = -1;
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
			temple = GameObject.FindGameObjectWithTag(Tags.CircularTemple).GetComponent<CircularTemple>();
		}

		audioOceanWaves = SoundManager.GetAudio(SoundManager.PlaySound(AudioClips.Instance.Ambience.OceanWaves.GetAny(), 0.5f));
		audioOceanWaves.loop = true;
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
		foreach (var templeLevel in temple.Levels) {
			templeLevel.SetRotationsEnabled(false);
		}
		playMenuPanel.SetActive(false);

		// Disable enemy movement
		enemies = Object.FindObjectsOfType<SimpleAgent>();
		for (int i = 0; i < enemies.Length; ++i) {
			enemies[i].Pause();
		}

		// Disable simulation on rigidbodies and save their state
		rigidbodies = Object.FindObjectsOfType<Rigidbody>();
		rigidbodyStates = new Tuple<bool, Vector3, Vector3>[rigidbodies.Length];
		for (int i = 0; i < rigidbodies.Length; ++i) {
			rigidbodyStates[i] = Tuple.Create(rigidbodies[i].isKinematic, rigidbodies[i].velocity, rigidbodies[i].angularVelocity);
			rigidbodies[i].isKinematic = true;
		}

		// Pause particle systems playback
		particleSystems = Object.FindObjectsOfType<ParticleSystem>();
		particleSystemStates = new bool[particleSystems.Length];
		for (int i = 0; i < particleSystems.Length; ++i) {
			particleSystemStates[i] = particleSystems[i].particleCount > 0;
			particleSystems[i].Pause();
		}

		// Disable pirate ship
		pirateShip.enabled = false;

        // Disable spikes
        spikes = Object.FindObjectsOfType<spikeTrigger>();
        for (int i = 0; i < spikes.Length; ++i)
        {
            spikes[i].pause();
            spikes[i].enabled = false;
        }
    }

	public void EnableGameInput() {
		foreach (var templeLevel in temple.Levels) {
			templeLevel.SetRotationsEnabled(true);
		}
		playMenuPanel.SetActive(true);

		// Enable enemy movement
		for (int i = 0; i < enemies.Length; ++i) {
			enemies[i].Resume();
		}

		// Enable simulation on rigidbodies and restore their state
		for (int i = 0; i < rigidbodies.Length; ++i) {
			rigidbodies[i].isKinematic = rigidbodyStates[i].Item1;
			rigidbodies[i].velocity = rigidbodyStates[i].Item2;
			rigidbodies[i].angularVelocity = rigidbodyStates[i].Item3;
		}

		// Resume particle systems playback
		for (int i = 0; i < particleSystems.Length; ++i) {
			if (particleSystemStates[i]) {
				particleSystems[i].Play();
			};
		}

		// Enable pirate ship
		pirateShip.enabled = true;

        // Enable spikes
        for (int i = 0; i < spikes.Length; ++i)
        {
            spikes[i].resume();
            spikes[i].enabled = true;
        }
    }
}
