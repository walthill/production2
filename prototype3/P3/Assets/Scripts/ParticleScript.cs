using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    ParticleSystem ps;
    

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //this is temporary
        checkInput();
        var main = ps.main;
    }

    void checkInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpeedColor1();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpeedColor2();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SpeedColor3();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpeedColor4();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpeedColor5();
        }
    }



    //colors are listed in order
    public void SpeedColor1()
    {
        //main.startColor = new Color(255, 120, 120);
        //ps.main.startColor=new Color(255, 120, 120);
        Debug.Log("color should be changed");
        gameObject.GetComponent<Renderer>().material.color = new Color(255, 120, 120);
    }
    public void SpeedColor2()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(255, 232, 30);
    }
    public void SpeedColor3()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(186, 255, 36);
    }
    public void SpeedColor4()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(0, 200, 239);
    }
    public void SpeedColor5()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(207, 0, 239);
    }

}
