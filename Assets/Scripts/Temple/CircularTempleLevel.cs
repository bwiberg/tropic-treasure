using System;
using cakeslice;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine;

[RequireComponent(typeof(Transformer),
    typeof(SingleTouchRotationGesture))]
public class CircularTempleLevel : MonoBehaviour {
    private Outline outline;

    void Awake() {
        GetComponent<SingleTouchRotationGesture>().TransformStarted += transformStartedHandler;
        GetComponent<SingleTouchRotationGesture>().TransformCompleted += transformCompletedHandler;
    }

    void transformStartedHandler(object sender, EventArgs e) {
        foreach (Transform child in transform) {
            child.GetComponent<Outline>().enabled = true;
        }
    }

    void transformCompletedHandler(object sender, EventArgs e) {
        foreach (Transform child in transform) {
            child.GetComponent<Outline>().enabled = false;
        }
    }
}