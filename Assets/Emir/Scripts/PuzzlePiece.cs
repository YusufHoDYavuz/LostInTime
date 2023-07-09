using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzlePiece : MonoBehaviour
{
    bool isRotating = false;
    public enum Direction {Up, Right, Down, Left};
    public Direction correctDirection;
    Direction currentDirection = Direction.Up;
    bool isPrevCorrect = false;
    static int correctPieces = 0;

    public void rotatePiece(){
        if(!isRotating){
            transform.DORotate(new Vector3(0,0,90), 0.5f, RotateMode.LocalAxisAdd).OnComplete(() => {
                isRotating = false;
                currentDirection = (Direction)(((int)currentDirection + 1) % 4);
                if(currentDirection == correctDirection){
                  correctPieces++;
                  isPrevCorrect = true;
                    if(correctPieces == RotationPuzzle.puzzlePiecesLength){
                        Debug.Log("Puzzle solved");
                    }
                }else{
                    if(isPrevCorrect){
                        correctPieces--;
                        isPrevCorrect = false;
                        
                    }
                }
            });
        }
    }
}
