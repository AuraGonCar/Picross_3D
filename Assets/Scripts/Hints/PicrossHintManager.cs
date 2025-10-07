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
    [Header("Face sliders")]
    [SerializeField] Slider minXSlider1;
    [SerializeField] Slider minXSlider2;
    [SerializeField] Slider maxXSlider1;
    [SerializeField] Slider maxXSlider2;
    [Space]
    [SerializeField] Slider minZSlider1;
    [SerializeField] Slider minZSlider2;
    [SerializeField] Slider maxZSlider1;
    [SerializeField] Slider maxZSlider2;
    [Header("Puzzle hint values")]
    public PicrossPiece selectedPiece;
    public int randomCorrectPieces;


    [HideInInspector] public InstanceManager parent;
    public PicrossLayerDetectionManagement layerDetection;

    public List<LayerSlider> currentSelectedAxis = new List<LayerSlider>();
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

        SetSlidersX();
        SetSlidersZ();
    }

    private void SetSlidersX()
    {
        minXSlider1.minValue = minXPosition;
        minXSlider1.maxValue = maxXPosition;

        minXSlider2.minValue = minXPosition;
        minXSlider2.maxValue = maxXPosition;

        maxXSlider1.minValue = minXPosition;
        maxXSlider1.maxValue = maxXPosition;

        maxXSlider2.minValue = minXPosition;
        maxXSlider2.maxValue = maxXPosition;
    }
    private void SetSlidersZ()
    {
        minZSlider1.minValue = minXPosition;
        minZSlider1.maxValue = maxXPosition;

        minZSlider2.minValue = minXPosition;
        minZSlider2.maxValue = maxXPosition;

        maxZSlider1.minValue = minXPosition;
        maxZSlider1.maxValue = maxXPosition;

        maxZSlider2.minValue = minXPosition;
        maxZSlider2.maxValue = maxXPosition;
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
            int rowHintValue = 0;
            foreach(PicrossPiece e in rowPieces)
            {
                if (e.isPieceCorrect())
                    rowHintValue++;
            }

            foreach (PicrossPiece e in rowPieces)
            {
                e.SetHintXValue(rowHintValue);
            }
        }

        //Y
        foreach (PicrossPiece p in minYPieces)
        {
            List<PicrossPiece> rowPieces = GetYRow(p);
            int rowHintValue = 0;
            foreach (PicrossPiece e in rowPieces)
            {
                if (e.isPieceCorrect())
                    rowHintValue++;
            }

            foreach (PicrossPiece e in rowPieces)
            {
                e.SetHintYValue(rowHintValue);
            }
        }

        //Z
        foreach (PicrossPiece p in minZPieces)
        {
            List<PicrossPiece> rowPieces = GetZRow(p);
            int rowHintValue = 0;
            foreach (PicrossPiece e in rowPieces)
            {
                if (e.isPieceCorrect())
                    rowHintValue++;
            }

            foreach (PicrossPiece e in rowPieces)
            {
                e.SetHintZValue(rowHintValue);
            }
        }
    }
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
    


    private void ClearMinMaxValues()
    {
        minXPosition = 0;
        minYPosition = 0;
        minZPosition = 0;

        maxXPosition = 0;
        maxYPosition = 0;
        maxZPosition = 0;
    }


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


    public void SetFaceValueX(LayerSlider slider)
    {
        if (slider.isMax)
        {
            int value = (int)(slider.currentSlider.value *-1);

            for (int i = xFaces.Count-1; i >= 0; i--)
            {
                if (xFaces[i].assignedValue > value)
                {
                    xFaces[i].DeactivateFace();
                }

                if (xFaces[i].assignedValue <= value)
                {
                    xFaces[i].ActivateFace();
                }
            }
        }
        else
        {
            for (int i = 0; i < xFaces.Count; i++)
            {
                if (xFaces[i].assignedValue < slider.currentSlider.value)
                {
                    xFaces[i].DeactivateFace();
                }

                if (xFaces[i].assignedValue >= slider.currentSlider.value)
                {
                    xFaces[i].ActivateFace();
                }
            }
        }
        
    }

    public void SetFaceValueZ(LayerSlider slider)
    {
        if (slider.isMax)
        {
            int value = (int)(slider.currentSlider.value * -1);

            for (int i = zFaces.Count-1; i >= 0; i--)
            {
                if (zFaces[i].assignedValue > value)
                {
                    zFaces[i].DeactivateFace();
                }

                if (xFaces[i].assignedValue <= value)
                {
                    zFaces[i].ActivateFace();
                }
            }
        }
        else
        {
            for (int i = 0; i < zFaces.Count; i++)
            {
                if (zFaces[i].assignedValue < slider.currentSlider.value)
                {
                    zFaces[i].DeactivateFace();
                }

                if (zFaces[i].assignedValue >= slider.currentSlider.value)
                {
                    zFaces[i].ActivateFace();
                }
            }
        }
        
    }

    public void BeginLayerControl()
    {
        parent.controls.ChangeControls(PicrossMode.Layer);
    }

    public void EndLayerControl()
    {
        parent.controls.ChangeControls(PicrossMode.None);
    }


    //Hint showing and hiding

    public void ShowHintsX()
    {
        if(selectedPiece != null)
        {
            List<PicrossPiece> pieces = GetXRow(selectedPiece);

            foreach(PicrossPiece p in pieces)
            {
                p.ShowHintsX();
            }
        }
    }

    public void HideHintsX()
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

    public void ShowHintsY()
    {
        if (selectedPiece != null)
        {
            List<PicrossPiece> pieces = GetYRow(selectedPiece);

            foreach (PicrossPiece p in pieces)
            {
                p.ShowHintsY();
            }
        }
    }

    public void HideHintsY()
    {
        if (selectedPiece != null)
        {
            List<PicrossPiece> pieces = GetYRow(selectedPiece);

            foreach (PicrossPiece p in pieces)
            {
                p.HideHintsY();
            }
        }
    }

    public void ShowHintsZ()
    {
        if (selectedPiece != null)
        {
            List<PicrossPiece> pieces = GetZRow(selectedPiece);

            foreach (PicrossPiece p in pieces)
            {
                p.ShowHintsZ();
            }
        }
    }

    public void HideHintsZ()
    {
        if (selectedPiece != null)
        {
            List<PicrossPiece> pieces = GetZRow(selectedPiece);

            foreach (PicrossPiece p in pieces)
            {
                p.HideHintsZ();
            }
        }
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
