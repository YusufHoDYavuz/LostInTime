using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimePassWithButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TimePass timePass;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject[] buttons; 
    [SerializeField] private float hoverDelay; 
    private int currentIndex = 0;
    public static bool isHovering = false;

    private void Start()
    {
        buttons[currentIndex].GetComponent<Image>().color = Color.red;
    }

    private void Update()
    {
        if (isHovering)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        if (horizontalInput > 0)
        {
            HoverNextButton();
        }
        else if (horizontalInput < 0)
        {
            HoverPreviousButton();
        }
        else if (verticalInput > 0)
        {
            HoverUpButton();
        }

        if (Input.GetMouseButtonDown(0))
        {
            ClickButton();
        }
    }

    private void HoverNextButton()
    {
        StartCoroutine(HoverDelayCoroutine(currentIndex + 1));
    }

    private void HoverPreviousButton()
    {
        StartCoroutine(HoverDelayCoroutine(currentIndex - 1));
    }

    private void HoverUpButton()
    {
        StartCoroutine(HoverDelayCoroutine(currentIndex - 2));
    }

    private IEnumerator HoverDelayCoroutine(int index)
    {
        index = Mathf.Clamp(index, 0, buttons.Length - 1);
        isHovering = true;
        
        buttons[currentIndex].GetComponent<Image>().color = Color.white;
        buttons[index].GetComponent<Image>().color = Color.green;

        currentIndex = index;
        yield return new WaitForSeconds(hoverDelay);
        isHovering = false;
    }

    private void ClickButton()
    {
        timePass.ShakeKamera(int.Parse(buttons[currentIndex].name));
        uiManager.SetActiveTransitionPanel();
        Debug.Log("Button clicked: " + buttons[currentIndex].name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentIndex = System.Array.IndexOf(buttons, eventData.selectedObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentIndex = 0;
    }
}
