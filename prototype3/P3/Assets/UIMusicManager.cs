using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMusicManager : MonoBehaviour
{
    public Text songAction;
    public Text songTitle;
    public string[] songNames;

    string randomLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    int currentSong = 0;
    bool isChanging = false;

    private void Update()
    {
        if (isChanging)
        {
            string tempThing = "";
            //char[] characters = songTitle.text.ToCharArray();
            //
            //foreach (int num in characters)
            //{
            //    characters[num] = randomLetters[Random.Range(0, randomLetters.Length)];
            //}

            foreach (char c in songTitle.text)
            {
                tempThing += randomLetters[Random.Range(0, randomLetters.Length)];
            }

            songTitle.text = tempThing;
        }
    }

    public void Rewind()
    {
        songAction.text = "<<RW";
        songTitle.text = "-------------";
        currentSong--;
        isChanging = true;
    }
    
    public void FastFoward()
    {
        songAction.text = "FF>>";
        songTitle.text = "-------------";
        currentSong++;
        isChanging = true;
    }

    public void PlayMusic()
    {
        songAction.text = "PLAY";
        songTitle.text = songNames[currentSong];
        isChanging = false;
    }
}
