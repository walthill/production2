using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.name == "Player")
        {
            GetComponentInParent<RaliScript>().setOnRail(false);
            Debug.Log("Player Contact");
        }
    }
}
