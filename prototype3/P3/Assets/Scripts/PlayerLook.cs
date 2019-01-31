using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera Control/Mouse Look")]
public class PlayerLook : MonoBehaviour
{

    public enum RotationAxes { MouseXAndY = 0, MouseX, MouseY };

    [System.Serializable]
    public struct InputData
    {
        public float xRightAxisDead, yRightAxisDead;
    };


    public InputData inputData;

    //  [SerializeField]
    // bool controllerLook;

    [SerializeField]
    RotationAxes axes = RotationAxes.MouseXAndY;

    [SerializeField]
    float sensitivityX, sensitivityY;

    const float minX = -360.0f, maxX = 360.0f;
    const float lowestYLook = -60.0f, highestYLook = 60.0f;

    float rotationX, rotationY = 0.0f;

    string rotationXInput, rotationYInput;


    private void Awake()
    {
            rotationXInput = "Rotate X";
            rotationYInput = "Rotate Y";

    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            /*rotationX = transform.localEulerAngles.y + Input.GetAxis(rotationXInput) * sensitivityX;
            rotationY += Input.GetAxis(rotationYInput) * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, lowestYLook, highestYLook);
            if((rotationX > inputData.xRightAxisDead || rotationX < -inputData.xRightAxisDead) ||
              (rotationY > inputData.yRightAxisDead || rotationY < -inputData.yRightAxisDead))
            {
                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            */

            float rotationX = Input.GetAxis(rotationXInput);
            float rotationY = Input.GetAxis(rotationYInput);

            //          Debug.Log("Xrot, " + rotationX);
            //            Debug.Log("Yrot, " + rotationY);


            if ((rotationX > inputData.xRightAxisDead || rotationX < -inputData.xRightAxisDead) ||
              (rotationY > inputData.yRightAxisDead || rotationY < -inputData.yRightAxisDead))
            {
                float speed = 0;
                if (rotationX < 0)
                    speed = -0.5f;
                else
                    speed = 0.5f;

                rotationX += (transform.localEulerAngles.y * sensitivityX) + speed;

                //BIG S/O to Michael Zheng
                rotationY -= (transform.localEulerAngles.x * sensitivityY) + speed;//solution was to minus rather than add because rotation is negated
                rotationY *= -1;

                //    Debug.Log(rotationY);
                //rotationY = Mathf.Clamp(rotationY, lowestYLook, highestYLook); //there is something wrong with clamp, but your rotation works

                transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
            }


        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis(rotationXInput) * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis(rotationYInput) * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, lowestYLook, highestYLook);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
}