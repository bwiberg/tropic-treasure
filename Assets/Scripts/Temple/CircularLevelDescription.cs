using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.AI;

public class CircularLevelDescription {
	public static readonly float NUM_OBSTACLES_FACTOR = 0.75f;
	public static readonly float OBSTACLE_SIZE_BIAS = 0.22f;

	public readonly List<Arc> Segments = new List<Arc>();
	public readonly float InnerRadius;
	public readonly float Thickness;
	public readonly float Height;

	public CircularLevelDescription(float innerRadius, float thickness, float height) {
		InnerRadius = innerRadius;
		Thickness = thickness;
		Height = height;
	}

	public GameObject toGameObject(int levelIndex, GameObject levelPrefab, GameObject segmentPrefab, GameObject obstaclePrefab) {
		var gameObject = GameObject.Instantiate(levelPrefab);
		gameObject.name = string.Format("Level {0}", levelIndex);
		var circularLevel = gameObject.GetComponent<CircularLevel>();
		circularLevel.InnerRadius = InnerRadius;
		circularLevel.Thickness = Thickness;
		circularLevel.Height = Height;

		var particleColliderPlanes = GameObject.FindGameObjectsWithTag(Tags.ParticleCollider);

		var segmentIndex = 0;
		foreach (var segment in Segments) {
			var segmentGameObject = GameObject.Instantiate(segmentPrefab) ;
			segmentGameObject.name = string.Format("Segment {0}", segmentIndex);
			segmentGameObject.transform.SetParent(gameObject.transform);

			var segmentComponent = segmentGameObject.GetComponent<CircularSegment>();
			segmentComponent.SegmentIndex = segmentIndex;
			segmentComponent.AngleStart = segment.AngleStart;
			segmentComponent.AngleEnd = segment.AngleEnd;

			// Create NavMeshObstacles for this segment
			int numObstacles = Mathf.Max(Mathf.RoundToInt(segment.Angle * InnerRadius * NUM_OBSTACLES_FACTOR), 1);

			Vector3 start = new Utility.Polar(InnerRadius + 0.5f * Thickness, segment.AngleStart).Cartesian3D;
			Vector3 end = new Utility.Polar(InnerRadius + 0.5f * Thickness, segment.AngleStart + (segment.Angle / numObstacles)).Cartesian3D;
			Vector3 size = new Vector3(Thickness, Height, (end - start).magnitude + OBSTACLE_SIZE_BIAS);

			for (int i = 0; i < numObstacles; ++i) {
				var obstacleObject = GameObject.Instantiate(obstaclePrefab);
				obstacleObject.name = string.Format("Obstacle {0}", i);
				var obstacle = obstacleObject.GetComponent<NavMeshObstacle>();
				obstacle.shape = NavMeshObstacleShape.Box;
				obstacle.size = size;
				obstacle.center = new Vector3((InnerRadius + 0.5f * Thickness), 0.5f * Height, 0.0f);
				obstacle.carving = true;

				obstacleObject.transform.Rotate(new Vector3(0.0f, 
					- Mathf.Rad2Deg * (segment.AngleStart + (segment.Angle / numObstacles) * (i + 0.5f)), 
					0.0f));
				obstacleObject.transform.SetParent(segmentComponent.transform);

				// Setup particle shapes for the obstacle	
				SegmentObstacle obstacleComponent = obstacleObject.GetComponent<SegmentObstacle>();
				var shape = obstacleComponent.destroyedParticles.shape;
				shape.box = obstacle.size;
				obstacleComponent.destroyedParticles.transform.localPosition = obstacle.center;
				obstacleComponent.destroyedParticles.randomSeed = (uint) Random.Range(uint.MinValue, uint.MaxValue);

				shape = obstacleComponent.dustParticles.shape;
				var box = obstacle.size;
				box.z = 0.1f;
				shape.box = box;
				obstacleComponent.dustParticles.transform.localPosition = obstacle.center - new Vector3(0.0f, obstacle.size.y / 2.5f, 0.0f);
				obstacleComponent.dustParticles.randomSeed = (uint) Random.Range(uint.MinValue, uint.MaxValue);

				for (int iplane = 0; iplane < obstacleComponent.destroyedParticles.collision.maxPlaneCount && iplane < particleColliderPlanes.Length; ++iplane) {
					obstacleComponent.destroyedParticles.collision.SetPlane(iplane, particleColliderPlanes[iplane].transform);
				}
			}

			var segmentMesh = WallMeshGenerator.GenerateArcWallMesh(segment.AngleStart,
				segment.AngleEnd, InnerRadius, Thickness, Height, smoothInnerOuterNormals: true);

			segmentGameObject.GetComponent<MeshCollider>().sharedMesh = segmentMesh;
			segmentGameObject.GetComponent<MeshFilter>().sharedMesh = segmentMesh;

			++segmentIndex;
		}

		return gameObject;
	}
}
