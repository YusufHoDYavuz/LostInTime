using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPuzzle : MonoBehaviour
{
    // Start is called before the first frame update

    private bool isInteracting = false;
    GameObject [] puzzlePieces;
    

    void Start()
    {
        puzzlePieces = GameObject.FindGameObjectsWithTag("PuzzlePiece");

    }

    // Update is called once per frame
    void Update()
    {
        interactPiece();
       
            if(!isInteracting){
                 foreach(GameObject piece in puzzlePieces){
                    Outline objectOutline = piece.GetComponent<Outline>();
                    if(objectOutline != null){
                        objectOutline.enabled = false;
                    }
            }
        }
    }

    void interactPiece(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition+new Vector3(0,0.5f,0));
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.gameObject.tag == "PuzzlePiece"){
                Outline objectOutline = hit.collider.gameObject.GetComponent<Outline>();
                if(objectOutline != null){
                    objectOutline.enabled = true;
                    isInteracting = true;
                    if(Input.GetKeyDown(KeyCode.E)){
                    hit.collider.gameObject.GetComponent<PuzzlePiece>().rotatePiece();
                }
                }
                
            }
            else
                isInteracting = false;
        }
    }

  
}
