using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TempleGenerator))]
public class TempleGeneratorEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var generator = (TempleGenerator) target;
        if(GUILayout.Button("Generate temple"))
        {
            generator.GenerateTemple();
        }
    }
}