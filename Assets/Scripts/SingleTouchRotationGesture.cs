/*
 * @author Benjamin Wiberg
 */

using System.Collections.Generic;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Gestures.Base;
using TouchScript.Layers;
using TouchScript.Utils;
#if TOUCHSCRIPT_DEBUG
using TouchScript.Utils.Debug;
#endif
using UnityEngine;

/// <summary>
/// Recognizes a transform gesture, i.e. translation, rotation, scaling or a combination of these.
/// </summary>
public class SingleTouchRotationGesture : TransformGestureBase, ITransformGesture {
    #region Public properties

    [SerializeField]
    public bool ForceSnapping = true;

    [SerializeField, Range(4, 120)]
    public int NumSnapAngles = 12;

    /// <summary>
    /// Gets or sets transform's projection plane normal.
    /// </summary>
    /// <value> Projection plane normal. </value>
    public Vector3 ProjectionPlaneNormal {
        get { return projectionPlaneNormal; }
        set {
            value.Normalize();
            if (projectionPlaneNormal == value) return;
            projectionPlaneNormal = value;
            if (Application.isPlaying) updateProjectionPlane();
        }
    }

    /// <summary>
    /// Plane where transformation occured.
    /// </summary>
    public Plane TransformPlane {
        get { return transformPlane; }
    }

    /// <summary>
    /// Gets delta position in local coordinates.
    /// </summary>
    /// <value>Delta position between this frame and the last frame in local coordinates.</value>
    public Vector3 LocalDeltaPosition {
        get { return TransformUtils.GlobalToLocalVector(cachedTransform, DeltaPosition); }
    }

    /// <summary>
    /// Gets rotation axis of the gesture in world coordinates.
    /// </summary>
    /// <value>Rotation axis of the gesture in world coordinates.</value>
    public Vector3 RotationAxis {
        get { return transformPlane.normal; }
    }

    #endregion

    #region Private variables

    [SerializeField] private Vector3 projectionPlaneNormal = Vector3.up;

    private TouchLayer projectionLayer;
    private Plane transformPlane;

	private float cumulativeAngle = 0.0f;

	private float snappedRotationAngle = 0.0f;

	private float previousSnappedAngle = 0.0f;

	public float PreviousAngle {
		set {
			previousSnappedAngle = value;
			cumulativeAngle = value;
		}
	}

	private float SnapAngle {
		get { return 360.0f / NumSnapAngles; }
	}

    #endregion

    #region Public methods

	/// <inheritdoc />
	public void ApplyTransform(Transform target) {
		if (ForceSnapping) {
			target.rotation = Quaternion.AngleAxis(snappedRotationAngle, RotationAxis);
		} else {
			// Delta rotation
			if (!Mathf.Approximately(DeltaRotation, 0f))
				target.rotation = Quaternion.AngleAxis(DeltaRotation, RotationAxis) * target.rotation;
		}
	}

    #endregion

    #region Unity methods

    /// <inheritdoc />
    protected override void Awake() {
        base.Awake();
        transformPlane = new Plane();
    }

    /// <inheritdoc />
    protected override void OnEnable() {
        base.OnEnable();
        updateProjectionPlane();
    }

    #endregion

    #region Gesture callbacks

    /// <inheritdoc />
    protected override void touchesBegan(IList<TouchPoint> touches) {
        base.touchesBegan(touches);

        if (State != GestureState.Possible) return;
        if (NumTouches == touches.Count) {
            projectionLayer = activeTouches[0].Layer;
            updateProjectionPlane();
        }

		cumulativeAngle = previousSnappedAngle;
    }

    #endregion

    #region Protected methods

    protected override void touchesMoved(IList<TouchPoint> touches) {
        base.touchesMoved(touches);

        var projectionParams = activeTouches[0].ProjectionParams;
        var dP = deltaPosition = Vector3.zero;
        var dR = deltaRotation = 0;
        var dS = deltaScale = 1f;

#if TOUCHSCRIPT_DEBUG
            drawDebugDelayed(getNumPoints());
#endif

        if (getNumPoints() < 1) return;

        var newScreenPos1 = getPointScreenPosition(0);

// Here we can't reuse last frame screen positions because points 0 and 1 can change.
// For example if the first of 3 fingers is lifted off.
        var oldScreenPos1 = getPointPreviousScreenPosition(0);

        dR = doSingleTouchRotation(oldScreenPos1, newScreenPos1, projectionParams);
		cumulativeAngle += dR;

		snappedRotationAngle = SnapAngle * Mathf.Round((cumulativeAngle / (360.0f)) * NumSnapAngles);

        if (State == GestureState.Possible) setState(GestureState.Began);
        switch (State) {
            case GestureState.Began:
            case GestureState.Changed:
                deltaRotation = dR;
                setState(GestureState.Changed);
                break;
        }
    }

	protected override void touchesEnded (IList<TouchPoint> touches) {
		base.touchesEnded (touches);
		cumulativeAngle = 0.0f;
		previousSnappedAngle = snappedRotationAngle;
	}

    protected float doSingleTouchRotation(Vector2 oldScreenPos, Vector2 newScreenPos,
        ProjectionParams projectionParams) {
        var newVector = projectionParams.ProjectTo(newScreenPos, TransformPlane);
        var oldVector = projectionParams.ProjectTo(oldScreenPos, TransformPlane);
        var angle = Vector3.Angle(oldVector, newVector);
        if (Vector3.Dot(Vector3.Cross(oldVector, newVector), TransformPlane.normal) < 0)
            angle = -angle;
        return angle;
    }

#if TOUCHSCRIPT_DEBUG
        protected override void clearDebug()
        {
            base.clearDebug();

            GLDebug.RemoveFigure(debugID + 3);
        }

        protected override void drawDebug(int touchPoints)
        {
            base.drawDebug(touchPoints);

            if (!DebugMode) return;
            switch (touchPoints)
            {
                case 1:
                    if (projection == ProjectionType.Global || projection == ProjectionType.Object)
                    {
                        GLDebug.DrawPlaneWithNormal(debugID + 3, cachedTransform.position, RotationAxis, 4f, GLDebug.MULTIPLY, float.PositiveInfinity);
                    }
                    break;
                default:
                    if (projection == ProjectionType.Global || projection == ProjectionType.Object)
                    {
                        GLDebug.DrawPlaneWithNormal(debugID + 3, cachedTransform.position, RotationAxis, 4f, GLDebug.MULTIPLY, float.PositiveInfinity);
                    }
                    break;
            }
        }
#endif

    #endregion

    #region Private functions

    /// <summary>
    /// Updates projection plane based on options set.
    /// </summary>
    private void updateProjectionPlane() {
        if (!Application.isPlaying) return;
        transformPlane = new Plane(projectionPlaneNormal, cachedTransform.position);
    }

    #endregion
}