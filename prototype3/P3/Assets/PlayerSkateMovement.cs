using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkateMovement : MonoBehaviour
{

    float xMove, zMove;
    bool jump, jumpReleased;

    Rigidbody rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        xMove = Input.GetAxis("JoyHorizontal");
        zMove = Input.GetAxis("JoyVertical");
        jump = Input.GetButtonDown("JoyJump");
        jumpReleased = Input.GetButtonUp("JoyJump");    
    }

    private void FixedUpdate()
    {
    }
}
