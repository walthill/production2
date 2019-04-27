using UnityEngine;
using UnityEngine.EventSystems;

//FROM https://forum.unity.com/threads/left-mouse-click-breaks-ui-buttons-why-solved.417769/

// If there is no selected item, set the selected item to the event system's first selected item
public class ControllerRefocus : MonoBehaviour
{
    [SerializeField] GameObject lastselect;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }

}