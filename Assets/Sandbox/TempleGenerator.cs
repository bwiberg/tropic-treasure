using System.Diagnostics;
using UnityEngine;
using Utility;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
public class TempleGenerator : MonoBehaviour {
    public GameObject LevelSegmentPrefab;

    [Range(2, 10)] public int TempleLevels = 5;

    [Range(0.1f, 10.0f)] public float InnerRadius = 0.25f;

    [Range(0.01f, 10.0f)] public float MeanThickness = 0.1f;

    [Range(0.0f, 10.0f)] public float ThicknessRange = 0.0f;

    [Range(0.01f, 10.0f)] public float MeanHeight = 0.25f;

    [Range(0.0f, 10.0f)] public float HeightRange = 0.0f;

    [Range(0.0f, 1.0f)] public float LevelPadding = 0.0f;

    public void GenerateTemple() {
#if UNITY_EDITOR
        foreach (Transform child in transform) {
//            UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(child.gameObject); };
            DestroyImmediate(child.gameObject);
        }
        transform.DetachChildren();


        var sw = new Stopwatch();
        sw.Start();

        var temple = CircularTempleGenerator.Generate(
            TempleLevels,
            InnerRadius,
            Tuple.Create(MeanThickness - ThicknessRange / 2, MeanThickness + ThicknessRange / 2),
            Tuple.Create(MeanHeight - HeightRange / 2, MeanHeight + HeightRange / 2),
            LevelPadding);

        var templeGameObject = temple.toGameObject(LevelSegmentPrefab);
        templeGameObject.transform.SetParent(transform);

        sw.Stop();

        Debug.LogFormat("Generated temple with {0} levels in {1} milliseconds", temple.Levels.Count,
            sw.ElapsedMilliseconds);
#endif
    }
}