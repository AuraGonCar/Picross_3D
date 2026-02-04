using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PicrossPiece : MonoBehaviour
{
    [SerializeField] public bool correctPiece;
    private bool marked = false;
    private bool wrong = false;

    [SerializeField] MeshRenderer renderer;
    [SerializeField] GameObject particles;

    //Placeholder color values until shader implementation
    [Header("Colors")]
    [SerializeField] private Color markedColor;
    [SerializeField] private Color wrongColor;

    private Color defaultColor;

    [Space]
    [Header("Puzzle hints")]
    [SerializeField] PieceHint xHints;
    [SerializeField] PieceHint yHints;
    [SerializeField] PieceHint zHints;

    public Vector3 currentPosition;

    private void Start()
    {
        defaultColor = renderer.material.color;
    }

    #region Piece options
    public void MarkPiece()
    {
        if (wrong)
            return;

        if (marked)
        {
            marked = false;
            renderer.material.color = defaultColor;
        }
        else
        {
            marked = true;
            renderer.material.color = markedColor;
        }
    }
    private void WrongPiece()
    {
        wrong = true;
        renderer.material.color = wrongColor;
    }

    public void DestroyPiece()
    {
        if (marked || wrong)
            return;

        if (correctPiece)
        {
            WrongPiece();
        }
        else
        {
            GameObject p = Instantiate(particles, transform.position, particles.transform.rotation);
            p.transform.parent = null;
            Destroy(p, 0.5f);
            Destroy(this.gameObject);
        }
    }

    public Vector3 SetPiecePosition()
    {
        currentPosition = new Vector3((int)transform.localPosition.x, (int)transform.localPosition.y, (int)transform.localPosition.z);
        return currentPosition;
    } 
    #endregion

    #region Set hints values
    public void SetHintXValue(int value)
    {
        xHints.hintValue = value;
        xHints.SetTextValue();
    }

    public void SetHintYValue(int value)
    {
        yHints.hintValue = value;
        yHints.SetTextValue();
    }

    public void SetHintZValue(int value)
    {
        zHints.hintValue = value;
        zHints.SetTextValue();
    }

    public void ClearAllValues()
    {
        SetHintXValue(0);
        xHints.HideCircle();
        xHints.HideSquare();
        xHints.needCircle = false;
        xHints.needSquare = false;

        SetHintYValue(0);
        yHints.HideCircle();
        yHints.HideSquare();
        yHints.needCircle = false;
        yHints.needSquare = false;

        SetHintZValue(0);
        zHints.HideCircle();
        zHints.HideSquare();
        zHints.needCircle = false;
        zHints.needSquare = false;
    }

    #endregion

    #region Piece correct checking and setting
    public bool isPieceCorrect()
    {
        return correctPiece;
    }

    public bool isPieceWrong()
    {
        return wrong;
    }

    public void SetPieceCorrect()
    {
        correctPiece = true;
    }

    public void SetPieceEmpty()
    {
        correctPiece = false;
    } 
    #endregion

    #region Show and Hide values

    //Showing and hiding hints
    public void ShowHintsX()
    {
        xHints.ShowHint();
    }
    public void HideHintX()
    {
        xHints.HideHint();
    }
    public void ShowHintsY()
    {
        yHints.ShowHint();
    }
    public void HideHintsY()
    {
        yHints.HideHint();
    }
    public void ShowHintsZ()
    {
        zHints.ShowHint();
    }
    public void HideHintsZ()
    {
        zHints.HideHint();
    }

    //Squares
    public void NeedSquareX()
    {
        xHints.needSquare = true;
    }
    public void NeedSquareY()
    {
        yHints.needSquare = true;
    }
    public void NeedSquareZ()
    {
        zHints.needSquare = true;
    }
    public void ShowSquareX()
    {
        xHints.ShowSquare();
    }
    public void HideSquareX()
    {
        xHints.HideSquare();
    }
    public void ShowSquareY()
    {
        yHints.ShowSquare();
    }
    public void HideSquareY()
    {
        yHints.HideSquare();
    }
    public void ShowSquareZ()
    {
        zHints.ShowSquare();
    }
    public void HideSquareZ()
    {
        zHints.HideSquare();
    }

    //Circles
    public void NeedCircleX()
    {
        xHints.needCircle = true;
    }
    public void NeedCircleY()
    {
        yHints.needCircle = true;
    }
    public void NeedCircleZ()
    {
        zHints.needCircle = true;
    }
    public void ShowCircleX()
    {
        xHints.ShowCircle();
    }
    public void HideCircleX()
    {
        xHints.HideCircle();
    }
    public void ShowCircleY()
    {
        yHints.ShowCircle();
    }
    public void HideCircleY()
    {
        yHints.HideCircle();
    }
    public void ShowCircleZ()
    {
        zHints.ShowCircle();
    }
    public void HideCircleZ()
    {
        zHints.HideCircle();
    }
    #endregion
}

[System.Serializable]
public class PieceHint
{
    [Header("Hint values")]
    [SerializeField] List<TextMeshProUGUI> textHints = new List<TextMeshProUGUI>();
    [Space]
    [SerializeField] GameObject circles;
    [Space]
    [SerializeField] GameObject squares;
    [Space]
    [SerializeField] public int hintValue;

    public bool needSquare;
    public bool needCircle;

    public void SetTextValue()
    {
        foreach(TextMeshProUGUI u in textHints)
        {
            u.SetText(hintValue.ToString());
        }
    }

    #region Hints
    public void HideHint()
    {
        foreach (TextMeshProUGUI u in textHints)
        {
            u.gameObject.SetActive(false);
        }

        HideCircle();
        HideSquare();
    }

    public void ShowHint()
    {
        foreach (TextMeshProUGUI u in textHints)
        {
            u.gameObject.SetActive(true);
        }

        if (needCircle)
            ShowCircle();

        if (needSquare)
            ShowSquare();
    }
    #endregion

    #region Squares
    public void ShowSquare()
    {
        squares.gameObject.SetActive(true);
    }

    public void HideSquare()
    {
        squares.gameObject.SetActive(false);
    }
    #endregion

    #region Circles
    public void ShowCircle()
    {
        circles.gameObject.SetActive(true);
    }

    public void HideCircle()
    {
        circles.gameObject.SetActive(false);
    }
    
    #endregion
}
