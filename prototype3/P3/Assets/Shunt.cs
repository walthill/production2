using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shunt : MonoBehaviour
{
    private Rigidbody playerRb;
    [SerializeField] float shuntForce = 5f;

    private void Awake()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
    }

    //NOTE: WALL TAGS NOT PLACED IN LEVEL -- YET

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            // Calculate Angle Between the collision point and the player
            Vector3 dir = collision.contacts[0].point - gameObject.transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            playerRb.AddForce(dir * shuntForce, ForceMode.Impulse);
         
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            // Calculate Angle Between the collision point and the player
            Vector3 dir = collision.contacts[0].point - gameObject.transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            playerRb.AddForce(dir * shuntForce, ForceMode.Impulse);
        }
    }

}
