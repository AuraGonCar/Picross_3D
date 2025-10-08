using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PuzzleCreator))]
public class PuzzleCreatorEditor : Editor
{
    public PuzzleCreator current
    {
        get
        {
            return (PuzzleCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Make new puzzle"))
        {
            current.MakeNewPuzzle();
        }
    }
}
