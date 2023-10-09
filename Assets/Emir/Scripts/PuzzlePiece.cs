using UnityEngine;
using DG.Tweening;

public class PuzzlePiece : MonoBehaviour
{
    public Direction correctDirection;
    public Direction currentDirection = Direction.Up;
    
    private bool isRotating;
    private bool isPrevCorrect;
    static int correctPieces;

    public void RotatePiece()
    {
        if (!isRotating)
        {
            transform.DORotate(new Vector3(0, 0, -90), 0.5f, RotateMode.LocalAxisAdd).OnComplete(() =>
            {
                isRotating = false;
                currentDirection = (Direction)(((int)currentDirection + 1) % 4);
                if (currentDirection == correctDirection)
                {
                    correctPieces++;
                    isPrevCorrect = true;
                    if (correctPieces == RotationPuzzle.puzzlePiecesLength)
                    {
                        Singleton.Instance.rotationFinished = true;
                        Debug.Log("Rotation Finished");
                    }
                }
                else
                {
                    if (isPrevCorrect)
                    {
                        correctPieces--;
                        isPrevCorrect = false;
                        Singleton.Instance.rotationFinished = false;
                        Debug.Log(correctPieces);
                    }
                }
            });
        }
    }
    
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}