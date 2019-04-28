using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    public float bounceHeight;
    [Range(0.1f, 2.0f)]
    public float bounceSpeed;
    [Range(10.0f, 20.0f)]
    public float rotationSpeed;

    public CollectableTracker.collectableType collectableType;

    float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Bounce();
    }

    void Rotate()
    {
        transform.Rotate( 0,  Time.deltaTime * rotationSpeed, 0, Space.World);

    }

    void Bounce()
    {
        float bounce = bounceHeight * Mathf.Sin(Time.time * bounceSpeed);
        transform.position = new Vector3(transform.position.x, startY + bounce , transform.position.z);
    }

    void OnTriggerEnter(Collider col)
    {
        //MAKE LINK TO SOUND AND/OR PLAYER HERE
        //
        //
        //
        //
        //MAKE LINK TO SOUND AND/OR PLAYER HERE
        if(col.name == "Player")
        {
            col.GetComponent<CollectableTracker>().addCollectable(collectableType);
            Destroy(gameObject);
        }
    }
}
