using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boomBoxBoomScr : MonoBehaviour
{
    public Animator anim;
    public AudioSource boomsource;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playBoom();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        anim.StopPlayback();
    }

    void playBoom()
    {
        boomsource.Play();
        anim.Play("BoomAnim");
        
    }
}
