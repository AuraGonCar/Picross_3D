using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(PicrossHintManager))]
public class PicrossHintManagerEditor : Editor
{
    public PicrossHintManager current
    {
        get
        {
            return (PicrossHintManager)target;
        }
    }

    private void OnSceneGUI()
    {
        if(Selection.activeGameObject!= null)
        {
            if(Selection.activeGameObject != current.selectedPiece)
            {
                if(Selection.activeGameObject.TryGetComponent<PicrossPiece>(out PicrossPiece p))
                {
                    current.selectedPiece = p;
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Reset pieces"))
        {
            current.SetPieces();
        }
        if (GUILayout.Button("Random"))
        {
            current.RandomCorrectPieces();
        }
        if (GUILayout.Button("Set dictionary"))
        {
            current.SetDictionary();
        }
        GUILayout.Space(5);

        if (GUILayout.Button("Set correct piece"))
        {
            if(current.selectedPiece != null)
            {
                current.selectedPiece.SetPieceCorrect();
            }
        }

        if (GUILayout.Button("Set empty piece"))
        {
            if (current.selectedPiece != null)
            {
                current.selectedPiece.SetPieceEmpty();
            }
        }

        GUILayout.Space(5);


        
        if (GUILayout.Button("Set hint values"))
        {
            current.SetHintValues();          
        }
        
        GUILayout.Space(5);

        if(GUILayout.Button("Show hints x"))
        {
            current.ShowHintsX();
        }
        if (GUILayout.Button("Hide hints x"))
        {
            current.HideHintsX();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Show hints y"))
        {
            current.ShowHintsY();
        }
        if (GUILayout.Button("Hide hints y"))
        {
            current.HideHintsY();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Show hints z"))
        {
            current.ShowHintsZ();
        }
        if (GUILayout.Button("Hide hints z"))
        {
            current.HideHintsZ();
        }

    }
}
