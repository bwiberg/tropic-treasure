using System.Collections.Generic;
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

        public GameObject toGameObject(int levelIndex, Material wallMaterial) {
            var gameObject = new GameObject(string.Format("Level {0}", levelIndex));

            var segmentIndex = 0;
            foreach (var segment in Segments) {
                var segmentGameObject = new GameObject(string.Format("Segment {0}", segmentIndex++));
                segmentGameObject.transform.SetParent(gameObject.transform);

                var segmentMesh = WallMeshGenerator.GenerateArcWallMesh(segment.AngleStart,
                    segment.AngleEnd, InnerRadius, Thickness, Height, smoothInnerOuterNormals: true);

                segmentGameObject.AddComponent<MeshCollider>().sharedMesh = segmentMesh;
                segmentGameObject.AddComponent<MeshFilter>().sharedMesh = segmentMesh;
                segmentGameObject.AddComponent<MeshRenderer>().material = wallMaterial;
            }

            return gameObject;
        }
    }

    public GameObject toGameObject(Material wallMaterial) {
        var gameObject = new GameObject("Temple");

        var levelIndex = 0;
        foreach (var level in Levels) {
            var levelGameObject = level.toGameObject(levelIndex++, wallMaterial);
            levelGameObject.transform.SetParent(gameObject.transform);
        }

        return gameObject;
    }

    public readonly List<Level> Levels = new List<Level>();
}