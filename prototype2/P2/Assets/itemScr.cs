using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemScr : MonoBehaviour
{
    public Transform bag;
    public bool WasScanned = false;
    public bool WasBagged = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (WasBagged&&WasScanned)
        {
            gameObject.transform.position = new Vector3(3.1f, 3.97f, -6.344004f)* Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position)
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "scanner" && collision.collider.tag == "selector")
        {
            WasScanned = true;
            Debug.Log("scanned");
        }
        if (collision.collider.tag == "bagger" && collision.collider.tag == "selector")
        {
            WasBagged = true;
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "scanner")
        {
            WasScanned = true;
            Debug.Log("scanned");
        }
        if (other.tag == "bagger")
        {
            WasBagged = true;
            Debug.Log("bagged");
        }
    }
}
