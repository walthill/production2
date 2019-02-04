using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlaceObstacle : MonoBehaviour
{
    public ObstacleType obstacleType;
    public SpeedChannel speedChannel;

    [SerializeField]
    Mesh[] modelList;

    MeshFilter mesh;

    void Update()
    {
        //Number of possible meshes should match number of obstacles
        if (modelList.Length != (int)ObstacleType.NUM_OBSTACLES)
            Debug.LogWarning("The number of possible meshes do not equal the number of obstacle types");

        if ((int)obstacleType < 0)
            gameObject.GetComponent<MeshFilter>().mesh = null;
        else
           gameObject.GetComponent<MeshFilter>().mesh = modelList[(int)obstacleType];

        Renderer rend = gameObject.GetComponent<Renderer>();

        if (speedChannel == SpeedChannel.QUICK)
        {   
            rend.sharedMaterial.color = Color.red;
        }
        else if (speedChannel == SpeedChannel.SPEEDY)
        {
            rend.sharedMaterial.color = Color.green;
        }
        else if (speedChannel == SpeedChannel.FAST)
        {
            rend.sharedMaterial.color = Color.blue;
        }
        else if (speedChannel == SpeedChannel.BLUR)
        {
            rend.sharedMaterial.color = Color.yellow;
        }
        else if (speedChannel == SpeedChannel.LIGHTNING)
        {
            rend.sharedMaterial.color = Color.magenta;
        }
    }
}
