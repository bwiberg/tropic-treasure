using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
public class EditorAutoWallGenerationTest : MonoBehaviour {
    [Range(0.0f, 2 * Mathf.PI)]
    public float AngleStart = 0.0f;

    [Range(0.0f, 2 * Mathf.PI)]
    public float AngleEnd = 1.0f;

    [Range(0.05f, 100.0f)]
    public float InnerRadius = 1.0f;

    [Range(0.1f, 10.0f)]
    public float InnerFaceArcLength = 0.25f;

    [Range(0.05f, 100.0f)]
    public float Thickness = 0.25f;

    [Range(0.05f, 100.0f)]
    public float Height = 0.5f;

    public bool SmoothNormals = false;

    private void OnValidate() {
        var sw = new Stopwatch();
        sw.Start();
        var mesh = WallMeshGenerator.GenerateArcWallMesh(AngleStart, AngleEnd, InnerRadius, Thickness, Height, InnerFaceArcLength, SmoothNormals);
        sw.Stop();
        Debug.LogFormat("Generated arc wall mesh with {0} vertices in {1} milliseconds", mesh.vertexCount, sw.ElapsedMilliseconds);

        var meshfilter = GetComponent<MeshFilter>();
        meshfilter.sharedMesh = mesh;

        var meshcollider = GetComponent<MeshCollider>();
        meshcollider.sharedMesh = mesh;
    }
}