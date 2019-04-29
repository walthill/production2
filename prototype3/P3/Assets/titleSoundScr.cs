using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleSoundScr : MonoBehaviour
{
    [Header("Sounds:")]
    public AudioClip HighlightSnd;
    public AudioClip SelectSnd;
    public AudioSource HiSrc;
    public AudioSource SeSrc;


    // Start is called before the first frame update
    void Start()
    {
        HiSrc = gameObject.AddComponent<AudioSource>();
        SeSrc = gameObject.AddComponent<AudioSource>();
        HiSrc.clip = HighlightSnd;
        SeSrc.clip = SelectSnd;
    }

    public void PlayHi()
    {
        HiSrc.Play();
    }
    public void PlaySe()
    {
        SeSrc.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
