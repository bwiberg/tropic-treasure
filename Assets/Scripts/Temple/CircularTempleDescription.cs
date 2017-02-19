using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.AI;

public class CircularTempleDescription {
	public class Level {
		public static readonly float NUM_OBSTACLES_FACTOR = 0.35f;
		public static readonly float OBSTACLE_SIZE_BIAS = 0.025f;

        public readonly List<Arc> Segments = new List<Arc>();
        public readonly float InnerRadius;
        public readonly float Thickness;
        public readonly float Height;

        public Level(float innerRadius, float thickness, float height) {
            InnerRadius = innerRadius;
            Thickness = thickness;
            Height = height;
        }

        public GameObject toGameObject(int levelIndex, GameObject levelPrefab, GameObject levelSegmentPrefab) {
			var gameObject = GameObject.Instantiate(levelPrefab);
			gameObject.name = string.Format("Level {0}", levelIndex);

            var segmentIndex = 0;
            foreach (var segment in Segments) {
                var segmentGameObject = GameObject.Instantiate(levelSegmentPrefab) ;
                segmentGameObject.name = string.Format("Segment {0}", segmentIndex++);
                segmentGameObject.transform.SetParent(gameObject.transform);

				var segmentComponent = segmentGameObject.GetComponent<CircularTempleSegment>();
				segmentComponent.Arc = segment;
				segmentComponent.InnerRadius = InnerRadius;
				segmentComponent.Thickness = Thickness;
				segmentComponent.Height = Height;

				// Create NavMeshObstacles for this segment
				int numObstacles = Mathf.Max(Mathf.RoundToInt(segment.Angle * InnerRadius * NUM_OBSTACLES_FACTOR), 1);

				Vector3 start = new Utility.Polar(InnerRadius + 0.5f * Thickness, segment.AngleStart - OBSTACLE_SIZE_BIAS).Cartesian3D;
				Vector3 end = new Utility.Polar(InnerRadius + 0.5f * Thickness, segment.AngleStart + OBSTACLE_SIZE_BIAS + (segment.Angle / numObstacles)).Cartesian3D;
				Vector3 size = new Vector3(Thickness, Height, (end - start).magnitude);

				for (int i = 0; i < numObstacles; ++i) {
					GameObject obstacleObject = new GameObject(string.Format("Obstacle {0}", i));
					var obstacle = obstacleObject.AddComponent<NavMeshObstacle>();
					obstacle.shape = NavMeshObstacleShape.Box;
					obstacle.size = size;
					obstacle.center = new Vector3((InnerRadius + 0.5f * Thickness), 0.5f * Height, 0.0f);
					obstacle.carving = true;

					obstacleObject.transform.Rotate(new Vector3(0.0f, 
						- Mathf.Rad2Deg * (segment.AngleStart + (segment.Angle / numObstacles) * (i + 0.5f)), 
						0.0f));
					obstacleObject.transform.SetParent(segmentComponent.transform);
				}

                var segmentMesh = WallMeshGenerator.GenerateArcWallMesh(segment.AngleStart,
                    segment.AngleEnd, InnerRadius, Thickness, Height, smoothInnerOuterNormals: true);

                segmentGameObject.GetComponent<MeshCollider>().sharedMesh = segmentMesh;
                segmentGameObject.GetComponent<MeshFilter>().sharedMesh = segmentMesh;
            }

            return gameObject;
        }
    }

	public GameObject toGameObject(GameObject levelPrefab, GameObject levelSegmentPrefab) {
        var gameObject = new GameObject("Temple");

        var levelIndex = 0;
        foreach (var level in Levels) {
            var levelGameObject = level.toGameObject(levelIndex++, levelPrefab, levelSegmentPrefab);
            levelGameObject.transform.SetParent(gameObject.transform);
        }

        return gameObject;
    }

    public readonly List<Level> Levels = new List<Level>();
}