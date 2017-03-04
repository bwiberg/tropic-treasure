using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(TempleGenerator))]
public class TempleGeneratorEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var generator = (TempleGenerator) target;
        if(GUILayout.Button("Generate temple"))
        {
			generator.GenerateTemple_InEditor();
        }
    }
}
#endif