using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="QuestDesc", menuName = "Quest/New Quest Description")]
public class QuestDescription : ScriptableObject
{
    public int priority;
    public string description;
    public bool done;
}
