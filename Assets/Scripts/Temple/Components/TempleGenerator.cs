using System.Diagnostics;
using UnityEngine;
using Utility;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
public class TempleGenerator : MonoBehaviour {
	public GameObject TemplePrefab;

	public GameObject LevelPrefab;

    public GameObject SegmentPrefab;

	public GameObject ObstaclePrefab;

    [Range(2, 10)] public int TempleLevels = 5;

    [Range(0.1f, 10.0f)] public float InnerRadius = 0.25f;

    [Range(0.01f, 10.0f)] public float MeanThickness = 0.1f;

    [Range(0.0f, 10.0f)] public float ThicknessRange = 0.0f;

    [Range(0.01f, 10.0f)] public float MeanHeight = 0.25f;

    [Range(0.0f, 10.0f)] public float HeightRange = 0.0f;

    [Range(0.0f, 1.0f)] public float LevelPadding = 0.0f;

	public bool SnapWallRotations = true;

	[Range(4, 120)] public int NumWallSnapModes = 12;

	public int RandomSeed = -1;

	private CircularTempleDescription Generate() {
		return CircularTempleGenerator.Generate(
			TempleLevels,
			InnerRadius,
			Tuple.Create(MeanThickness - ThicknessRange / 2, MeanThickness + ThicknessRange / 2),
			Tuple.Create(MeanHeight - HeightRange / 2, MeanHeight + HeightRange / 2),
			LevelPadding);
	}

#if UNITY_EDITOR
    public void GenerateTemple_InEditor() {
        foreach (Transform child in transform) {
//            UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(child.gameObject); };
            DestroyImmediate(child.gameObject);
        }
        transform.DetachChildren();

		if (RandomSeed != -1) {
			Random.InitState(RandomSeed);
		}

        var sw = new Stopwatch();
        sw.Start();

		var temple = Generate();

		// Create wrapper with EditorOnly tag
		var templeContainer = new GameObject("__EditorOnly_TempleContainer__");
		templeContainer.tag = Tags.EditorOnly;
		templeContainer.transform.SetParent(transform);

		var templeGameObject = temple.toGameObject(TemplePrefab, LevelPrefab, SegmentPrefab, ObstaclePrefab);
        templeGameObject.transform.SetParent(templeContainer.transform);

		foreach (var rotationGesture in templeGameObject.GetComponentsInChildren<SingleTouchRotationGesture>()) {
			rotationGesture.ForceSnapping = SnapWallRotations;
			rotationGesture.NumSnapAngles = NumWallSnapModes;
		}

        sw.Stop();

        Debug.LogFormat("Generated temple with {0} levels in {1} milliseconds", temple.Levels.Count,
            sw.ElapsedMilliseconds);
    }
#endif

	public void GenerateTemple_InGame() {
		if (RandomSeed != -1) {
			Random.InitState(RandomSeed);
		}

		var temple = Generate();

		var templeGameObject = temple.toGameObject(TemplePrefab, LevelPrefab, SegmentPrefab, ObstaclePrefab);
		templeGameObject.transform.SetParent(transform);

		foreach (var rotationGesture in templeGameObject.GetComponentsInChildren<SingleTouchRotationGesture>()) {
			rotationGesture.ForceSnapping = SnapWallRotations;
			rotationGesture.NumSnapAngles = NumWallSnapModes;
		}
	}
}