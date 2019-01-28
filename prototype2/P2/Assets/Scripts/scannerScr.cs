using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scannerScr : MonoBehaviour
{
    
    private float Timer;

    public bool baggingOrScanning;
    Renderer r;
    public GameObject particles;

    public bool selected;
    public bool entered;
    // Start is called before the first frame update
    void Start()
    {
        particles.SetActive(false);
        r = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (entered)
        {
            
        }
        
        else
        {
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
        }
        if (entered && selected)
        {
            Color colorY = Color.yellow;
            r.material.color = colorY;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //either scan or bag
                baggingOrScanning = true;
            }

        }
        
        if (baggingOrScanning)
        {
            particles.SetActive(true);
            //Timer += Time.deltaTime;
        }
        if (!entered)
        {
            particles.SetActive(false);
        }
        

        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (selected)
            {
                
                selected = false;
            }
            else if (!selected)
            {
                selected = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            baggingOrScanning = false;
            
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        entered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        entered = false;
    }

}
