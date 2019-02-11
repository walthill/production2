using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    ParticleSystem ps;

    public Color color0;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;
    //ParticleSystem ps = GetComponent<ParticleSystem>();
    //var col = ps.colorOverLifetime;
    //col.enabled = true;


    // Start is called before the first frame update
    void Start()
    {

        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = false;
        //var bol = ps.startColor;
    }

    // Update is called once per frame
    void Update()
    {
        //this is temporary
        checkInput();
        
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
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpeedColor6();
            //ps.main.ColoroverLifetime();

        }
    }

    public void MakeBig()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var emi = ps.emission;
        

    }

    public void MakeSmall()
    {

    }

    //colors are listed in order
    public void SpeedColor1()
    {
        //main.startColor = new Color(255, 120, 120);
        //ps.main.startColor=new Color(255, 120, 120);
        Debug.Log("color should be changed");

        //gameObject.GetComponent<Renderer>().material.color = Color.red;//new Color(255, 120, 120);
        
        //gameObject.GetComponent<Renderer>().material.color = color0;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = false;
        var main = ps.main;
        main.startColor = color0;
        
    }
    public void SpeedColor2()
    {
        //gameObject.GetComponent<Renderer>().material.color = new Color(255, 232, 30);
        //gameObject.GetComponent<Renderer>().material.color = color1;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = false;
        var main = ps.main;
        main.startColor = color1;
    }
    public void SpeedColor3()
    {
        //gameObject.GetComponent<Renderer>().material.color = new Color(186, 255, 36);
        //gameObject.GetComponent<Renderer>().material.color = color2;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = false;
        var main = ps.main;
        main.startColor = color2;
    }
    public void SpeedColor4()
    {
        //gameObject.GetComponent<Renderer>().material.color = new Color(0, 200, 239);
        //gameObject.GetComponent<Renderer>().material.color = color3;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = false;
        var main = ps.main;
        main.startColor = color3;
    }
    public void SpeedColor5()
    {
        //gameObject.GetComponent<Renderer>().material.color = new Color(207, 0, 239);
        //gameObject.GetComponent<Renderer>().material.color = color4;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = false;
        var main = ps.main;
        main.startColor = color4;
    }
    public void SpeedColor6()
    {
        //gameObject.GetComponent<Renderer>().material.color = new Color(207, 0, 239);

        gameObject.GetComponent<Renderer>().material.color = color5;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        
        //var main = ps.main;
        col.enabled = true;
        var main = ps.main;
        main.startColor = color5;
    }
}
