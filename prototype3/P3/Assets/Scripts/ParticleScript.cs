using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public static ParticleScript instance;
    ParticleSystem ps;
    ParticleSystem.MainModule MyMainModule;
    ParticleSystem.EmissionModule myEmissionModule;
    [Header("all spark colors")]
    public Color color0;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;
    public Color color6;
    //ParticleSystem ps = GetComponent<ParticleSystem>();
    //var col = ps.colorOverLifetime;
    //col.enabled = true;
    [Header("Current Values")]

    public float EmissionRate;
    public float particleStartSize;
    //this variable holds the size of the particles at the start of the level so that they can be returned back to that afterwards.
    //this value is for resetting the particle size back to normal
    [Header("Values at START")]
    public float EmissionsRateOnStart;
    public float particleStartSizeOnStart;
    


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = false;
        //var bol = ps.startColor;
        var main = ps.main;
        myEmissionModule = ps.emission;
        //main.startSize = particleStartSize;
        //particleStartSize = main.startSize;
        MyMainModule = ps.main;
        particleStartSizeOnStart = MyMainModule.startSize.constant;
        EmissionsRateOnStart = myEmissionModule.rateOverDistance.constant;



    }

    // Update is called once per frame
    void Update()
    {
        //this is temporary
        checkInput();
        
    }

    void checkInput()
    {
       /* if (Input.GetKeyDown(KeyCode.A))
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            MakeBig();

        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            MakeSmall();

        }*/
        
    }

    //the functions below control the size and emission rates of the particles

    public void MakeBig()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        //var emi = ps.emission;

        var main = ps.main;
        
        //multiply the particle's size from whatever it is in the inspector at the start of the scene by 1.5
        particleStartSize = particleStartSizeOnStart*1.5f;
        main.startSize = particleStartSize;

        //double the spawn rate from whatever it says in the inspector at the start of the scene
        EmissionRate = EmissionsRateOnStart * 2;
        myEmissionModule.rateOverDistance = EmissionRate;
        
        
    }

    public void MakeSmall()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        var main = ps.main;
        //particleStartSize = 1;
        //return the values to what they were in the inspector at the begining of the scene
        main.startSize = particleStartSizeOnStart;
        myEmissionModule.rateOverDistance = EmissionsRateOnStart;
        
    }

    //colors are listed in order and are changed useing these functions
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
