using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrab : MonoBehaviour
{
    Ray mouseRay;
    RaycastHit hit;

    GameObject grabbedItem;
    bool hasGrabbedItem = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(hasGrabbedItem)
        {
            //grabbedItem.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }

        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out hit))
        {
            if(hit.collider.tag == "Selectable")
                if(Input.GetMouseButton(0))
                {
                    Debug.Log("Selected");
                    //hasGrabbedItem = true;
                   // grabbedItem = hit.collider.gameObject;
                }
        }
    }
}
