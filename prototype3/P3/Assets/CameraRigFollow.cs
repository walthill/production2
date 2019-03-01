using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigFollow : MonoBehaviour
{
    [SerializeField] Transform rigAnchor;

    void Awake()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = rigAnchor.position;
    }
}
