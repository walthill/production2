using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogicScr : MonoBehaviour
{

    Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.right * Time.deltaTime;
    }

    //make this determin where the object is and detect if it has been prossessed.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "bagger")
        {

        }
    }
}
