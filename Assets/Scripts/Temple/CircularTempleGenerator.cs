using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CircularTempleGenerator {
    private static List<Arc> GenerateLevelSegments(int levelIndex, int minSegments = 1) {
        var segments = new List<Arc>();
        var numWallSegments = minSegments + Random.Range(0, levelIndex + 2);

        // generate 2*N segments and choose every other one
        var generatedSegments = new float[2 * numWallSegments];
        var sum = 0.0f;
        for (var i = 0; i < generatedSegments.Length; i++) {
            generatedSegments[i] = Random.Range(1.0f, 4.0f);
            sum += generatedSegments[i];
        }

        var angleOffset = Random.Range(0.0f, 2 * Mathf.PI);
        var cumsum = 0.0f;
        for (var i = 0; i < generatedSegments.Length; i += 2) {
            cumsum += generatedSegments[i];
            var angleStart = angleOffset + 2 * Mathf.PI * cumsum / sum;
            cumsum += generatedSegments[i + 1];
            var angleEnd = angleOffset + 2 * Mathf.PI * cumsum / sum;

            segments.Add(new Arc(angleStart, angleEnd));
        }

        return segments;
    }

    public static CircularTemple Generate(
        int levels,
        float startRadius,
        Tuple<float, float> thicknessRange,
        Tuple<float, float> heightRange,
        float levelPadding = 0.0f) {

        var temple = new CircularTemple();

        var currentRadius = startRadius;
        for (var ilevel = 0; ilevel < levels; ilevel++) {
            var thickness = Random.Range(thicknessRange.Item1, thicknessRange.Item2);
            var height = Random.Range(heightRange.Item1, heightRange.Item2);

            var level = new CircularTemple.Level(currentRadius, thickness, height);
            currentRadius += thickness + levelPadding;

            level.Segments.AddRange(GenerateLevelSegments(ilevel));
            temple.Levels.Add(level);
        }

        return temple;
    }
}