using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float textSpeed;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private GameObject textPanelObject;

    void Start()
    {
       
        Invoke("StartDemo", 2);
    }
    void StartDemo()
    {
       
        ShowTextBox(new string[] { "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent vel ante at mi euismod accumsan.", "Duis gravida sapien tincidunt odio maximus, ac gravida lorem condimentum.\r\nCurabitur vel orci mattis, sollicitudin purus quis, tristique magna.", "Nunc fringilla leo a dolor porttitor auctor.\r\nAliquam ornare lectus nec metus condimentum, vitae tempus metus dignissim.\r\nEtiam ut erat ut leo facilisis pharetra.\r\nPhasellus sed erat convallis, sodales ligula non."});
    }

    void ShowTextBox(string[] dialogues)
    {
        textPanelObject.SetActive(true);
        textBox.text = "";
        textPanelObject.transform.DOScale(Vector3.zero, 0.25f).From().OnComplete(()=>StartCoroutine(StartDialogue(dialogues)));
    }

    void CloseTextBox()
    {
        textPanelObject.transform.DOScale(Vector3.zero, 0.25f).OnComplete(()=>textPanelObject.SetActive(false));
    }


    IEnumerator StartDialogue(string[] dialogueStrings,int dialogueIndex = 0)
    {
        if (dialogueIndex == dialogueStrings.Length) {
            yield return new WaitForSeconds(2);
            CloseTextBox();
        }
        else {
        string text = "";
        float dialogueTime = dialogueStrings[dialogueIndex].Length / textSpeed;
        DOTween.To(() => text, x => text = x, dialogueStrings[dialogueIndex], dialogueTime).SetEase(Ease.Linear)
            .OnUpdate(() => { textBox.text = text; });

        yield return new WaitForSeconds(dialogueTime+1);
        StartCoroutine( StartDialogue(dialogueStrings, dialogueIndex + 1));
        } 
    }
}
