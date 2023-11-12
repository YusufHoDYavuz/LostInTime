using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestController))]
public class QuestEditor : Editor
{
    GameObject lastCreated = null;

    public override void OnInspectorGUI()
    {
        Transform targetTransform = target.GetComponent<Transform>();
            

        if (GUILayout.Button("Create Quest")){
            GameObject created = new GameObject("quest");
            created.transform.position = targetTransform.position;
            created.transform.SetParent(targetTransform);
            created.AddComponent<Quest>();
            
          
        }
    }
}
