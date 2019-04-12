using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScaleScr : MonoBehaviour
{
    //ALSO THIS SCRIPT DOES SONG SWITCHING
    
    public Animator crouchanim;
    Rigidbody rb;
    public GameObject WIDTransform;
    bool crouch;
    //public Vector3 down;
    //float gravScaler = 10;

    // Start is called before the first frame update
    void Start()
    {
        //down = WIDTransform.transform.up * -1;
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetButtonDown("JoyGrav"))
        {
            rb.gravityScale*
           
        }
        */
        //TESTTTTTTTTTTTTT
        //https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
        /*
        if (Input.GetButtonDown("JoyGrav"))
        {
            Debug.Log("grav is calling");
            rb.AddForce(0, -100, 0, ForceMode.Acceleration);
        }
        */
        

    }
    private void FixedUpdate()
    {
        if (Input.GetButton("JoyGrav"))
        {
            Debug.Log("grav is calling");
            rb.AddForce(WIDTransform.transform.up * -100, ForceMode.Acceleration);
        }
        if (Input.GetButtonDown("JoyGrav"))
        {
            
            crouchanim.SetTrigger("isCrouching");
        }
        if (Input.GetButtonUp("JoyGrav"))
        {
            
            crouchanim.SetBool("isCrouching",false);
        }

    }

}
