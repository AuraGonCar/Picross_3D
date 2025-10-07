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
        SetHintYValue(0);
        SetHintZValue(0);
    }

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

    #region Show and Hide hints
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

    #endregion
}

[System.Serializable]
public class PieceHint
{
    [SerializeField] List<TextMeshProUGUI> textHints = new List<TextMeshProUGUI>();
    [SerializeField] public int hintValue;


    public void SetTextValue()
    {
        foreach(TextMeshProUGUI u in textHints)
        {
            u.SetText(hintValue.ToString());
        }
    }

    public void HideHint()
    {
        foreach (TextMeshProUGUI u in textHints)
        {
            u.gameObject.SetActive(false);
        }
    }

    public void ShowHint()
    {
        foreach (TextMeshProUGUI u in textHints)
        {
            u.gameObject.SetActive(true);
        }
    }
}
