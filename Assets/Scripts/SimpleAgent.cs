using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EazyTools.SoundManager;

public class SimpleAgent : MonoBehaviour {
	private NavMeshAgent agent;

	public Transform target;

	public GameObject DeathParticlesPrefab;

	public static float RemainingDistanceThreshold = 0.5f;

	private bool wasAlerted = false;

	public float YarrProbabilityOnSpawn = 0.1f;
	public float YarrSilenceFactor = 1.0f;
	private static float timeOfLastYarr = 0.0f;

	[SerializeField] private float linearSpeed = 1.0f;

	// Use this for initialization
	private void OnEnable () {
        target = FindObjectOfType<spawnBox>().chest.transform;
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(target.position);
	}

	private void Start() {
		float timeSinceLastYarr = Time.time - timeOfLastYarr;
		float probabilityContribution = (1.0f - YarrProbabilityOnSpawn) * Mathf.Atan(YarrSilenceFactor * timeSinceLastYarr);

		if (Random.value <= YarrProbabilityOnSpawn + probabilityContribution) {
			var clip = AudioClips.Instance.Pirates.VoiceYarr.GetAny();
			SoundManager.PlaySound(clip);
			timeOfLastYarr = Time.time;
		}
	}

	private void Update() {
		if (GameManager.Instance.pirateShip.FireState == PirateShip.CannonFireState.HasNoTarget &&
			agent.pathStatus == NavMeshPathStatus.PathPartial && 
			agent.remainingDistance < RemainingDistanceThreshold || wasAlerted) {
			findObstructionWallAndAlertCannon();
		}
	}

	private void findObstructionWallAndAlertCannon() {
		// Do not alert cannon if ship is moving 
		if (GameManager.Instance.ship.GetComponent<BlowShipAway>().shipIsGone)
		{
			wasAlerted = true;
			return;
		}

		Vector3 agentPosition = transform.position;

		GameObject closestSegment = null;
		float closestDistance = float.PositiveInfinity;

		var segments = GameObject.FindGameObjectsWithTag(Tags.CircularSegment);
		foreach (var segment in segments) {
			foreach (Transform obstacleTransform in segment.transform) {
				NavMeshObstacle obstacle = obstacleTransform.GetComponent<NavMeshObstacle>();

				Vector3 position = obstacleTransform.TransformPoint(obstacle.center);

				float distance = (position - agentPosition).magnitude;
				if (distance < closestDistance) {
					closestDistance = distance;
					closestSegment = segment;
				}
			}
		}

		// Hacky way to find middle of segment
		var allObstacles = closestSegment.GetComponentsInChildren<NavMeshObstacle>();
		var middleObstacle = allObstacles[Mathf.FloorToInt(allObstacles.Length / 2)];

		wasAlerted = false;
		GameManager.Instance.pirateShip.FireCannonballAtSegment(
			closestSegment,
			middleObstacle.transform.TransformPoint(middleObstacle.center)
		);
	}

	public void handleHitByRollingBall(RollingBallOfDeath ball) {
		killSelf();
	}

	public void handleHitByCannonball(Cannonball ball) {
		killSelf();
	}

	public void handleHitBySpikes() {
		killSelf();
	}

	private void killSelf() {
		var blood = GameObject.Instantiate(DeathParticlesPrefab);
		blood.transform.position = transform.position;
		GameObject.Destroy(gameObject);

		var clip = AudioClips.Instance.Pirates.Death.GetAny();
		SoundManager.PlaySound(clip);
	}

	public void Pause() {
		agent.speed = 0.0f;
	}

	public void Resume() {
		agent.speed = linearSpeed;
	}
}
