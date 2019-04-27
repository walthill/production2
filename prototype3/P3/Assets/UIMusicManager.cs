using UnityEngine;
using UnityEngine.UI;

public class UIMusicManager : MonoBehaviour
{
    public Text songAction;
    public Text songTitle;
    public string[] songNames;

    public GameObject unlockNotification;
    Text unlockText;

    string randomLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    int currentSong = 0;
    bool isChanging = false;

    private void Start()
    {
        unlockText = unlockNotification.transform.Find("UnlockText").gameObject.GetComponent<Text>();
    }

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

    public void ManagerUnlockSong(int song)
    {
        unlockNotification.SetActive(false);
        unlockText.text = "'" + songNames[song].ToString() + "' TRACK UNLOCKED!";
        unlockNotification.SetActive(true);
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
        // if song isnt unlocked
        {

        }
        // else if it is
        {
            songTitle.text = songNames[currentSong];
        }
        isChanging = false;
    }
}
