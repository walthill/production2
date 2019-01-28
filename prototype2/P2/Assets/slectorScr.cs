using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slectorScr : MonoBehaviour
{
    public Transform Spot0;
    public Transform Spot1;
    public bool atSpot0 = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (atSpot0)
        {
            gameObject.transform.position = new Vector3(Spot0.position.x, Spot0.position.y, Spot0.position.z);
        }
        if (!atSpot0)
        {
            gameObject.transform.position = new Vector3(Spot1.position.x, Spot1.position.y, Spot1.position.z);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (atSpot0)
            {
                atSpot0 = false;
            }
            else if (!atSpot0)
            {
                atSpot0 = true;
            }

            /*

            if (selected)
            {
                Color colorR = Color.red;
                r.material.color = colorR;

            }
            else if (!selected)
            {
                Color colorW = Color.white;
                r.material.color = colorW;
            }
            */
        }


    }
}
