using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkateMovement : MonoBehaviour
{

    /*
     * CONTROLS
     * Triggers turning
     * Up on stick to accel, down to deccel
     * A to crouch
     */

    float xMove, zMove, xRotate, yRotate;
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
        yRotate = Input.GetAxis("Rotate Y");

       //TODO: Lerp turning?

    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3();
        movement.x = xMove * 5;
        movement.z = zMove * 5;


        rb.velocity = movement; 
    }
}
