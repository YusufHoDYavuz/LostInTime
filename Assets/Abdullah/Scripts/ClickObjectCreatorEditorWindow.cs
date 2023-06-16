using UnityEditor;
using UnityEngine;

public class ClickObjectCreatorEditorWindow : EditorWindow
{
    private Vector3 pointPosition;
    private bool createObjectEnabled = false;
    private GameObject parentObject;

    [MenuItem("Window/Click Object Creator")]
    public static void ShowWindow()
    {
        ClickObjectCreatorEditorWindow window = GetWindow<ClickObjectCreatorEditorWindow>("Click Object Creator");
        window.minSize = new Vector2(200, 120);
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        Event currentEvent = Event.current;

        if (createObjectEnabled && currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                pointPosition = hit.point;

                if (parentObject != null)
                {
                    GameObject newObject = new GameObject("EmptyObject");
                    newObject.transform.position = pointPosition;
                    newObject.transform.SetParent(parentObject.transform);
                    Selection.activeGameObject = newObject;
                    EditorGUIUtility.PingObject(newObject);
                }
            }

            currentEvent.Use();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(20f);
        EditorGUILayout.LabelField("Click Position: " + pointPosition.ToString());

        EditorGUILayout.Space(10f);
        createObjectEnabled = GUILayout.Toggle(createObjectEnabled, "Create Object");

        EditorGUILayout.Space(10f);
        parentObject = EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true) as GameObject;
    }
}
