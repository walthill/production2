using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogicScr : MonoBehaviour
{
    public Transform bag;
    public bool WasScanned = false;
    public bool WasBagged = false;
    bool done = false;
    Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
        
        if (WasBagged && WasScanned)
        {
            gameObject.transform.position = new Vector3(3.1f, 3.97f, -6.344004f); //* Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position)
            //done = true;
            addToTimer();
        }



        if (!WasBagged){
            gameObject.transform.position += Vector3.right * Time.deltaTime;
        }
        if (done)
        {
            //addToTimer();
        }
        
    }

    void addToTimer()
    {
        GameStuffScr.instance.addTime();
        WasScanned = false;
        GameStuffScr.instance.addScore();
    }

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
        if (other.tag == "Finish")
        {
            GameStuffScr.instance.subTimer();
            GameStuffScr.instance.subScore();
        }
    }
    
}
