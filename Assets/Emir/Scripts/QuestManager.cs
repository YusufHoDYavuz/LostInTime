using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Queue<Quest> queue;
    [SerializeField] private Transform questController;
    private Quest currentQuest;
    [SerializeField] private Quest firstQuest;

    public void Start()
    {
        queue = new Queue<Quest>();
        int childCount = questController.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform quest = questController.GetChild(i);
            quest.gameObject.SetActive(false);
            queue.Enqueue(quest.GetComponent<Quest>());
        }
        questComplete();

        if (!Singleton.Instance.isFirstSceneDone && firstQuest != null)
        {
            queue.Clear();
            queue.Enqueue(firstQuest.GetComponent<Quest>());
            questComplete();
            Singleton.Instance.isFirstSceneDone = true;
            
        }

    }

    public void questComplete()
    {
        if (currentQuest != null)
            currentQuest.gameObject.SetActive(false);
        currentQuest = queue.Dequeue();
        currentQuest.gameObject.SetActive(true);
        Singleton.Instance.SetKnowledge(currentQuest.questDescription.description);

    }


}
