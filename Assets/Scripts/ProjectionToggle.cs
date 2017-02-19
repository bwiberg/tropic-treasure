using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionToggle : MonoBehaviour {
    public float Width = 50.0f;

    public float Height = 30.0f;

    public float MarginRight = 10.0f;

    public float MarginTop = 10.0f;

    private string[] projections = new string[]{"Perspective", "Orthographic"};

    private int projectionIndex = 0;

    void Awake() {
        UpdateProjectionType();
    }

    void OnGUI() {
        Rect rect = new Rect(Screen.width - Width - MarginRight,
                             MarginTop,
                             Width,
                             Height);
        if (GUI.Button(rect, projections[projectionIndex])) {
            projectionIndex = 1 - projectionIndex;
            UpdateProjectionType();
        }
    }

    private void UpdateProjectionType() {
        Camera.main.orthographic = projectionIndex == 1;
    }
}