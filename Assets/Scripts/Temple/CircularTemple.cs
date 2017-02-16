using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class CircularTemple {
    public class Level {
        public readonly List<Arc> Segments = new List<Arc>();
        public readonly float InnerRadius;
        public readonly float Thickness;
        public readonly float Height;

        public Level(float innerRadius, float thickness, float height) {
            InnerRadius = innerRadius;
            Thickness = thickness;
            Height = height;
        }

        public GameObject toGameObject(int levelIndex, GameObject levelSegmentPrefab) {
            var gameObject = new GameObject(string.Format("Level {0}", levelIndex));
            gameObject.AddComponent<CircularTempleLevel>();

            var segmentIndex = 0;
            foreach (var segment in Segments) {
                var segmentGameObject = GameObject.Instantiate(levelSegmentPrefab) ;
                segmentGameObject.name = string.Format("Segment {0}", segmentIndex++);
                segmentGameObject.transform.SetParent(gameObject.transform);

                var segmentMesh = WallMeshGenerator.GenerateArcWallMesh(segment.AngleStart,
                    segment.AngleEnd, InnerRadius, Thickness, Height, smoothInnerOuterNormals: true);

                segmentGameObject.GetComponent<MeshCollider>().sharedMesh = segmentMesh;
                segmentGameObject.GetComponent<MeshFilter>().sharedMesh = segmentMesh;
            }

            return gameObject;
        }
    }

    public GameObject toGameObject(GameObject levelSegmentPrefab) {
        var gameObject = new GameObject("Temple");

        var levelIndex = 0;
        foreach (var level in Levels) {
            var levelGameObject = level.toGameObject(levelIndex++, levelSegmentPrefab);
            levelGameObject.transform.SetParent(gameObject.transform);
        }

        return gameObject;
    }

    public readonly List<Level> Levels = new List<Level>();
}