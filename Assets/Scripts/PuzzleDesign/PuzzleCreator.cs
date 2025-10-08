using UnityEngine;
public class PuzzleCreator : MonoBehaviour
{
    [Header("Puzzle pieces")]
    [SerializeField] GameObject puzzleModel;
    [SerializeField] GameObject puzzleHintsPrefab;
    [SerializeField] GameObject puzzlePiecePrefab;

    private Vector3 minDimensions;
    private Vector3 maxDimensions;

    [SerializeField] Vector3 chosenMinDimentions;
    [SerializeField] Vector3 chosenMaxDimensions;

    private GameObject currentPuzzle;

    public bool sameAsMinValues;
    public void MakeNewPuzzle()
    {
        if (!sameAsMinValues)
        {
            if (!AreChosenDimensionsValid())
            {
                Debug.Log("Dimensions not valid");
                return;
            }
        }
        else
        {
            SetMinAndMaxDimensions();
        }

        if (currentPuzzle != null)
            DestroyImmediate(currentPuzzle);

        currentPuzzle = Instantiate(puzzleHintsPrefab, Vector3.zero, Quaternion.identity);
        currentPuzzle.transform.parent = null;

        for (int x = (int)chosenMinDimentions.x; x <= chosenMaxDimensions.x; x++)
        {
            for (int y = (int)chosenMinDimentions.y; y <= chosenMaxDimensions.y; y++)
            {
                for (int z = (int)chosenMinDimentions.z; z <= chosenMaxDimensions.z; z++)
                {
                    Vector3 pos = new Vector3((float)x, (float)y, (float)z);
                    GameObject piece = Instantiate(puzzlePiecePrefab, pos, puzzlePiecePrefab.transform.rotation);
                    piece.transform.parent = currentPuzzle.transform;

                    if (DoesPositionMatchWithPuzzle(pos))
                    {
                        if (piece.TryGetComponent<PicrossPiece>(out PicrossPiece p))
                            p.SetPieceCorrect();
                    }
                }
            }
        }

        if(currentPuzzle.TryGetComponent<PicrossHintManager>(out PicrossHintManager hints))
        {
            hints.SetPieces();
            hints.SetHintValues();
        }
    }

    private bool AreChosenDimensionsValid()
    {
        if (minDimensions == Vector3.zero || maxDimensions == Vector3.zero)
            SetMinAndMaxDimensions();

        if (chosenMaxDimensions.x < maxDimensions.x || chosenMaxDimensions.y < maxDimensions.y || chosenMaxDimensions.z < maxDimensions.z)
            return false;

        if (chosenMinDimentions.x > minDimensions.x || chosenMinDimentions.y > minDimensions.y || chosenMinDimentions.z > minDimensions.z)
            return false;

        return true;
    }

    private void SetMinAndMaxDimensions()
    {
        float maxValueX = 0;
        float maxValueY = 0;
        float maxValueZ = 0;


        float minValueX = 0;
        float minValueY = 0;
        float minValueZ = 0;

        foreach (Transform p in puzzleModel.transform)
        {
            if (maxValueX <= p.position.x)
                maxValueX = p.position.x;
            if (minValueX > p.position.x)
                minValueX = p.position.x;


            if (maxValueY <= p.position.y)
                maxValueY = p.position.y;
            if (minValueY > p.position.y)
                minValueY = p.position.y;


            if (maxValueZ <= p.position.z)
                maxValueZ = p.position.z;
            if (minValueZ > p.position.z)
                minValueZ = p.position.z;
        }

        maxDimensions = new Vector3(maxValueX, maxValueY, maxValueZ);

        minDimensions = new Vector3(minValueX, minValueY, minValueZ);

        if (sameAsMinValues)
        {
            chosenMinDimentions = minDimensions;
            chosenMaxDimensions = maxDimensions;
        }
    }

    public bool DoesPositionMatchWithPuzzle(Vector3 pos)
    {
        foreach (Transform p in puzzleModel.transform)
        {
            if (p.transform.position == pos)
                return true;
        }

        return false;
    }

}
