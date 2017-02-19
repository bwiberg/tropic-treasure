/*
 * @author Benjamin Wiberg
 */

using TouchScript.Editor.Gestures.Base;
using TouchScript.Gestures;
using UnityEditor;
using UnityEngine;

namespace TouchScript.Editor.Gestures
{
	[CustomEditor(typeof(SingleTouchRotationGesture), true)]
	internal class SingleTouchRotationGestureEditor : TransformGestureBaseEditor
	{
		SerializedProperty forceSnapping;
		SerializedProperty snapAngles;
		SerializedProperty projectionPlaneNormal;

		protected override void OnEnable()
		{
			base.OnEnable();

			forceSnapping = serializedObject.FindProperty("ForceSnapping");
			snapAngles = serializedObject.FindProperty("NumSnapAngles");
			projectionPlaneNormal = serializedObject.FindProperty("projectionPlaneNormal");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.PropertyField(forceSnapping, true);
			EditorGUILayout.PropertyField(snapAngles, true);
			EditorGUILayout.PropertyField(projectionPlaneNormal, true);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
