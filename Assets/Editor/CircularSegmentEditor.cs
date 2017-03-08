/*
 * @author Benjamin Wiberg
 */

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(CircularSegment))]
public class CircularSegmentEditor : Editor {
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var segment = (CircularSegment) target;
		if(GUILayout.Button("Destroy"))
		{
			segment.handleCannonballHit();
		}
	}
}
#endif