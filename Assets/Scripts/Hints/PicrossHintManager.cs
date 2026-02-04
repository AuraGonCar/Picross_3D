using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PicrossHintManager : MonoBehaviour
{
    public Dictionary<Vector3, PicrossPiece> piecesInPuzzleDictionary = new Dictionary<Vector3, PicrossPiece>();
    public List<PicrossPiece> piecesInPuzzle = new List<PicrossPiece>();

    [Header("Puzzle data")]
    public int minXPosition;
    public int maxXPosition;
    [Space]
    public int minYPosition;
    public int maxYPosition;
    [Space]
    public int minZPosition;
    public int maxZPosition;

    [Space]
    [Header("Faces")]
    [SerializeField] List<PicrossFace> xFaces = new List<PicrossFace>();
    [SerializeField] List<PicrossFace> zFaces = new List<PicrossFace>();
    [Space]
    
    [Header("Puzzle hint values")]
    public PicrossPiece selectedPiece;
    public int randomCorrectPieces;


    [HideInInspector] public InstanceManager parent;
    public PicrossLayerDetectionManagement layerDetection;

    [SerializeField] private bool multipleSelect;
    public List<PicrossPiece> currentSelection;

    public void SetDictionary()
    {
        if (piecesInPuzzleDictionary.Count <= 0)
        {
            foreach (Transform o in transform)
            {
                if (o.TryGetComponent<PicrossPiece>(out PicrossPiece p))
                {

                    piecesInPuzzle.Add(p);
                    piecesInPuzzleDictionary.Add(p.SetPiecePosition(), p);
                    p.gameObject.name = "Piece( " + p.currentPosition.x + ", " + p.currentPosition.y + ", " + p.currentPosition.z + " )";
                }
            }
        }
    }
    public void SetPieces()
    {
        //Clearing piece list, dictionary and min max values if we're resetting the puzzles
        if(piecesInPuzzle.Count > 0)
        {
            foreach(PicrossPiece p in piecesInPuzzle)
            {
                p.SetPieceEmpty();
                p.ClearAllValues();
            }
        }
        piecesInPuzzle.Clear();
        piecesInPuzzleDictionary.Clear();
        ClearMinMaxValues();

        //Setting the pieces position, list and dictionary
        foreach(Transform o in transform)
        {
            if(o.TryGetComponent<PicrossPiece>(out PicrossPiece p))
            {
                piecesInPuzzle.Add(p);
                piecesInPuzzleDictionary.Add(p.SetPiecePosition(), p);
                p.gameObject.name = "Piece( "+p.currentPosition.x+", "+p.currentPosition.y+", "+p.currentPosition.z+" )";
            }
        }

        //Setting min max values
        foreach(PicrossPiece p in piecesInPuzzle)
        {
            if (p.currentPosition.x < minXPosition)
                minXPosition = (int)p.currentPosition.x;
            else if (p.currentPosition.x > maxXPosition)
                maxXPosition = (int)p.currentPosition.x;

            if (p.currentPosition.y < minYPosition)
                minYPosition = (int)p.currentPosition.y;
            else if (p.currentPosition.y > maxYPosition)
                maxYPosition = (int)p.currentPosition.y;

            if (p.currentPosition.z < minZPosition)
                minZPosition = (int)p.currentPosition.z;
            else if (p.currentPosition.z > maxZPosition)
                maxZPosition = (int)p.currentPosition.z;
        }


        //Setting faces in puzzle

        //X faces

        if (xFaces.Count > 0)
            xFaces.Clear();
        

        for (int i = minXPosition; i <= maxXPosition; i++)
        {
            PicrossFace f = new PicrossFace();

            List<PicrossPiece> newXFace = new List<PicrossPiece>();
            foreach(PicrossPiece p in piecesInPuzzle)
            {
                if (p.currentPosition.x == i && !newXFace.Contains(p))
                    newXFace.Add(p);
            }

            f.SetFace(newXFace, i);
            xFaces.Add(f);
        }


        //Z faces

        if (zFaces.Count > 0)
            zFaces.Clear();

        for (int i = minZPosition; i <= maxZPosition; i++)
        {
            PicrossFace f = new PicrossFace();

            List<PicrossPiece> newZFace = new List<PicrossPiece>();
            foreach (PicrossPiece p in piecesInPuzzle)
            {
                if (p.currentPosition.z == i && !newZFace.Contains(p))
                    newZFace.Add(p);
            }

            f.SetFace(newZFace, i);
            zFaces.Add(f);
        }
    }
    
    public void SetHintValues()
    {
        List<PicrossPiece> minXPieces = new List<PicrossPiece>();
        List<PicrossPiece> minYPieces = new List<PicrossPiece>();
        List<PicrossPiece> minZPieces = new List<PicrossPiece>();

        foreach (PicrossPiece p in piecesInPuzzle)
        {
            if (p.currentPosition.x == minXPosition)
                minXPieces.Add(p);

            if (p.currentPosition.y == minYPosition)
                minYPieces.Add(p);

            if (p.currentPosition.z == minZPosition)
                minZPieces.Add(p);
        }

        //X
        foreach(PicrossPiece p in minXPieces)
        {
            List<PicrossPiece> rowPieces = GetXRow(p);
            List<PicrossPiece> correctPieces = new List<PicrossPiece>();

            foreach(PicrossPiece e in rowPieces)
            {
                if (e.isPieceCorrect())
                    correctPieces.Add(e);
            }

            if (correctPieces.Count >= 2)
            {
                int numberOfGroups = 1;

                for (int i = 0; i < correctPieces.Count; i++)
                {
                    if (i + 1 >= correctPieces.Count)
                        break;
                    if (correctPieces[i + 1].currentPosition.x - correctPieces[i].currentPosition.x > 1)
                        numberOfGroups++;
                }

                if (numberOfGroups == 2)
                {
                    foreach (PicrossPiece e in rowPieces)
                    {
                        e.NeedCircleX();
                    }
                }

                if (numberOfGroups > 2)
                {
                    foreach (PicrossPiece e in rowPieces)
                    {
                        e.NeedSquareX();
                    }
                }
            }

            foreach (PicrossPiece e in rowPieces)
            {
                e.SetHintXValue(correctPieces.Count);
                e.ShowHintsX();
            }

            
        }

        //Y
        foreach (PicrossPiece p in minYPieces)
        {
            List<PicrossPiece> rowPieces = GetYRow(p);
            List<PicrossPiece> correctPieces = new List<PicrossPiece>();

            foreach (PicrossPiece e in rowPieces)
            {
                if (e.isPieceCorrect())
                    correctPieces.Add(e);
            }
            if (correctPieces.Count >= 2)
            {
                int numberOfGroups = 1;

                for (int i = 0; i < correctPieces.Count; i++)
                {
                    if (i + 1 >= correctPieces.Count)
                        break;
                    if (correctPieces[i + 1].currentPosition.y - correctPieces[i].currentPosition.y > 1)
                        numberOfGroups++;
                }

                if (numberOfGroups == 2)
                {
                    foreach (PicrossPiece e in rowPieces)
                    {
                        e.NeedCircleY();
                    }
                }

                if (numberOfGroups > 2)
                {
                    foreach (PicrossPiece e in rowPieces)
                    {
                        e.NeedSquareY();
                    }
                }
            }

            foreach (PicrossPiece e in rowPieces)
            {
                e.SetHintYValue(correctPieces.Count);
                e.ShowHintsY();
            }

            
        }

        //Z
        foreach (PicrossPiece p in minZPieces)
        {
            List<PicrossPiece> rowPieces = GetZRow(p);
            List<PicrossPiece> correctPieces = new List<PicrossPiece>();

            foreach (PicrossPiece e in rowPieces)
            {
                if (e.isPieceCorrect())
                {
                    correctPieces.Add(e);
                }
            }
            if (correctPieces.Count >= 2)
            {
                int numberOfGroups = 1;

                for (int i = 0; i < correctPieces.Count; i++)
                {
                    if (i + 1 >= correctPieces.Count)
                        break;
                    if (correctPieces[i + 1].currentPosition.z - correctPieces[i].currentPosition.z > 1)
                        numberOfGroups++;
                }

                if (numberOfGroups == 2)
                {
                    foreach (PicrossPiece e in rowPieces)
                    {
                        e.NeedCircleZ();
                    }
                }

                if (numberOfGroups > 2)
                {
                    foreach (PicrossPiece e in rowPieces)
                    {
                        e.NeedSquareZ();
                    }
                }
            }
            foreach (PicrossPiece e in rowPieces)
            {
                e.SetHintZValue(correctPieces.Count);
                e.ShowHintsZ();
            }  
        }
    }

    #region Get Rows
    private List<PicrossPiece> GetXRow(PicrossPiece basePiece)
    {
        List<PicrossPiece> pieces = new List<PicrossPiece>();
        pieces.Add(basePiece);

        float yValue = basePiece.currentPosition.y;
        float zValue = basePiece.currentPosition.z;

        for (int i = minXPosition; i <= maxXPosition; i++)
        {
            Vector3 pos = new Vector3(i, yValue, zValue);
            PicrossPiece p = piecesInPuzzleDictionary[pos];

            if (!pieces.Contains(p))
                pieces.Add(p);

        }

        return pieces;
    }

    private List<PicrossPiece> GetYRow(PicrossPiece basePiece)
    {
        List<PicrossPiece> pieces = new List<PicrossPiece>();
        pieces.Add(basePiece);

        float xValue = basePiece.currentPosition.x;
        float zValue = basePiece.currentPosition.z;

        for (int i = minYPosition; i <= maxYPosition; i++)
        {
            Vector3 pos = new Vector3(xValue, i, zValue);
            PicrossPiece p = piecesInPuzzleDictionary[pos];

            if (!pieces.Contains(p))
                pieces.Add(p);

        }

        return pieces;
    }

    private List<PicrossPiece> GetZRow(PicrossPiece basePiece)
    {
        List<PicrossPiece> pieces = new List<PicrossPiece>();
        pieces.Add(basePiece);

        float xValue = basePiece.currentPosition.x;
        float yValue = basePiece.currentPosition.y;

        for (int i = minZPosition; i <= maxZPosition; i++)
        {
            Vector3 pos = new Vector3(xValue, yValue, i);
            PicrossPiece p = piecesInPuzzleDictionary[pos];

            if (!pieces.Contains(p))
                pieces.Add(p);

        }

        return pieces;
    } 
    #endregion

    private void ClearMinMaxValues()
    {
        minXPosition = 0;
        minYPosition = 0;
        minZPosition = 0;

        maxXPosition = 0;
        maxYPosition = 0;
        maxZPosition = 0;
    }

    //Placeholder method
    public void RandomCorrectPieces()
    {
        if (piecesInPuzzle.Count > 0)
        {
            foreach (PicrossPiece p in piecesInPuzzle)
            {
                p.SetPieceEmpty();
            }
        }
        for (int i = 0; i < randomCorrectPieces; i++)
        {
            piecesInPuzzle[Random.Range(0, piecesInPuzzle.Count)].SetPieceCorrect();
        }

        SetHintValues();
    }


    #region Face updating
    public void SetFaceValueX(PicrossLayerControl currentLayer, int inputValue)
    {
        UpdateFaces(currentLayer, inputValue, xFaces);
    }

    public void SetFaceValueZ(PicrossLayerControl currentLayer, int inputValue)
    {
        UpdateFaces(currentLayer, inputValue, zFaces);
    }

    public void UpdateFaces(PicrossLayerControl currentLayer, int inputValue, List<PicrossFace> faces)
    {
        Debug.Log(inputValue);
        if (currentLayer.isMax)
        {
            currentLayer.RemoveValue(inputValue);

            if (currentLayer.IsCurrentValueOriginal())
            {
                faces[faces.Count - 1].ActivateFace();
                currentLayer.Deactivate();
                return;
            }

            currentLayer.SetActive();

            for (int i = zFaces.Count - 1; i >= 0; i--)
            {
                if (faces[i].assignedValue > currentLayer.currentValue)
                    faces[i].DeactivateFace();

                else
                    faces[i].ActivateFace();
            }
        }
        else
        {
            currentLayer.AddValue(inputValue);

            if (currentLayer.IsCurrentValueOriginal())
            {
                faces[0].ActivateFace();
                currentLayer.Deactivate();
                return;
            }

            currentLayer.SetActive();

            for (int i = 0; i < faces.Count; i++)
            {
                if (faces[i].assignedValue < currentLayer.currentValue)
                    faces[i].DeactivateFace();

                else
                    faces[i].ActivateFace();
            }
        }
    } 
    #endregion

    //Check if unused
    #region Layer control
    public void BeginLayerControl()
    {
        parent.controls.ChangeControls(PicrossMode.Layer);
    }

    public void EndLayerControl()
    {
        parent.controls.ChangeControls(PicrossMode.None);
    } 
    #endregion

    public void CheckLayers(PicrossLayerControl currentActiveLayer, int value)
    {
        if (currentActiveLayer.layerX)
            SetFaceValueX(currentActiveLayer, value);
        if (currentActiveLayer.layerZ)
            SetFaceValueZ(currentActiveLayer, value);
    }

    #region Showi8ng and hiding hints
    public void ShowHintsX()
    {
        if (multipleSelect)
        {
            if(currentSelection.Count > 0)
            {
                foreach(PicrossPiece p in currentSelection)
                {
                    List<PicrossPiece> pieces = GetXRow(p);

                    foreach (PicrossPiece e in pieces)
                    {
                        e.ShowHintsX();
                    }
                }
            }
        }
        else
        {
            if (selectedPiece != null)
            {
                List<PicrossPiece> pieces = GetXRow(selectedPiece);

                foreach (PicrossPiece p in pieces)
                {
                    p.ShowHintsX();
                }
            }
        }
        
    }

    public void HideHintsX()
    {
        if (multipleSelect)
        {
            if (currentSelection.Count > 0)
            {
                foreach (PicrossPiece p in currentSelection)
                {
                    List<PicrossPiece> pieces = GetXRow(p);

                    foreach (PicrossPiece e in pieces)
                    {
                        e.HideHintX();
                    }
                }
            }
        }
        else
        {
            if (selectedPiece != null)
            {
                List<PicrossPiece> pieces = GetXRow(selectedPiece);

                foreach (PicrossPiece p in pieces)
                {
                    p.HideHintX();
                }
            }
        }
    }

    public void ShowHintsY()
    {
        if (multipleSelect)
        {
            if (currentSelection.Count > 0)
            {
                foreach (PicrossPiece p in currentSelection)
                {
                    List<PicrossPiece> pieces = GetXRow(p);

                    foreach (PicrossPiece e in pieces)
                    {
                        e.ShowHintsY();
                    }
                }
            }
        }
        else
        {
            if (selectedPiece != null)
            {
                List<PicrossPiece> pieces = GetXRow(selectedPiece);

                foreach (PicrossPiece p in pieces)
                {
                    p.ShowHintsY();
                }
            }
        }
    }

    public void HideHintsY()
    {
        if (multipleSelect)
        {
            if (currentSelection.Count > 0)
            {
                foreach (PicrossPiece p in currentSelection)
                {
                    List<PicrossPiece> pieces = GetXRow(p);

                    foreach (PicrossPiece e in pieces)
                    {
                        e.HideHintsY();
                    }
                }
            }
        }
        else
        {
            if (selectedPiece != null)
            {
                List<PicrossPiece> pieces = GetXRow(selectedPiece);

                foreach (PicrossPiece p in pieces)
                {
                    p.HideHintsY();
                }
            }
        }
    }

    public void ShowHintsZ()
    {
        if (multipleSelect)
        {
            if (currentSelection.Count > 0)
            {
                foreach (PicrossPiece p in currentSelection)
                {
                    List<PicrossPiece> pieces = GetXRow(p);

                    foreach (PicrossPiece e in pieces)
                    {
                        e.ShowHintsZ();
                    }
                }
            }
        }
        else
        {
            if (selectedPiece != null)
            {
                List<PicrossPiece> pieces = GetXRow(selectedPiece);

                foreach (PicrossPiece p in pieces)
                {
                    p.ShowHintsZ();
                }
            }
        }
    }

    public void HideHintsZ()
    {
        if (multipleSelect)
        {
            if (currentSelection.Count > 0)
            {
                foreach (PicrossPiece p in currentSelection)
                {
                    List<PicrossPiece> pieces = GetXRow(p);

                    foreach (PicrossPiece e in pieces)
                    {
                        e.HideHintsZ();
                    }
                }
            }
        }
        else
        {
            if (selectedPiece != null)
            {
                List<PicrossPiece> pieces = GetXRow(selectedPiece);

                foreach (PicrossPiece p in pieces)
                {
                    p.HideHintsZ();
                }
            }
        }
    } 
    #endregion

    public void ActivateMultipleSelection()
    {
        multipleSelect = true;
        selectedPiece = null;
    }

    public void DeactivateMultipleSelection()
    {
        multipleSelect = false;
        currentSelection.Clear();
    }

    public bool isMultiple()
    {
        return multipleSelect;
    }
}

[System.Serializable]
public class PicrossFace
{
    public List<PicrossPiece> piecesInFace = new List<PicrossPiece>();
    public bool faceActive = true;

    public int assignedValue;

    public void SetFace(List<PicrossPiece> newFace, int newValue)
    {
        assignedValue = newValue;
        foreach(PicrossPiece p in newFace)
        {
            piecesInFace.Add(p);
        }
    }

    public void DeactivateFace()
    {
        if (!faceActive)
            return;

        foreach(PicrossPiece p in piecesInFace)
        {
            if(p != null)
                p.gameObject.SetActive(false);
        }

        faceActive = false;
    }

    public void ActivateFace()
    {
        if (faceActive)
            return;

        foreach (PicrossPiece p in piecesInFace)
        {
            if(p!= null)
                p.gameObject.SetActive(true);
        }

        faceActive = true;
    }
}
