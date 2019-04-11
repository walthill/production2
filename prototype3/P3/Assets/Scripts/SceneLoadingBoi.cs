using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ListWrapper
{
    public int lightingScene;
    public List<int> sceneList;
}

public class SceneLoadingBoi : MonoBehaviour
{
    // https://answers.unity.com/questions/242794/inspector-field-for-scene-asset.html
    // Check this out to make it easier to select scenes in editor

        
    /* ----------------------------------------------------
        NOTE - Scene Indeces are based on BUILD NUMBER
    /* ---------------------------------------------------- */


    [SerializeField] LoadingScreen loadingScreen = null;
    [SerializeField] int loadingSceneIndex = 0;

    [SerializeField]
    [Tooltip("Index of scenes that should stay loaded")]
    List<int> mPersistentScenes = new List<int>();
    [SerializeField]
    [Tooltip("Indexes of scenes to be loaded for each level")]
    public List<ListWrapper> mLevelScenes = new List<ListWrapper>();

    //keep track of which level indices have been loaded
    List<int> loadedLevels;
    int currentLevelIndex = 0;

    //Start is called before the first frame update
    void Start()
    {
       //Load all persistent scenes
        foreach (int sceneNum in mPersistentScenes)
        {
            SceneManager.LoadScene(sceneNum, LoadSceneMode.Additive);
        }
        loadedLevels = new List<int>();
        loadLevel(currentLevelIndex);
    }
    //load all scenes in given level index
    void loadLevel(int index)
    {
        foreach (int sceneNum in mLevelScenes[index].sceneList)
        {
            loadingScreen.Show(SceneManager.LoadSceneAsync(sceneNum, LoadSceneMode.Additive));
        }

        loadedLevels.Add(index);

        StartCoroutine(LoadingWait());
    }
    
    IEnumerator LoadingWait()
    {
        Time.timeScale = 0;
        yield return waitForKeyPress(KeyCode.JoystickButton0);
        loadingScreen.Hide();
        Time.timeScale = 1;
    }

    //Stolen from https://forum.unity.com/threads/waiting-for-input-in-a-custom-function.474387/
    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            
            //Attempt to set the lighting scene active until it loads in
            try
            {
                Scene lightScene = SceneManager.GetSceneByBuildIndex(mLevelScenes[currentLevelIndex].lightingScene);
                if (SceneManager.GetActiveScene().buildIndex != lightScene.buildIndex)
                   SceneManager.SetActiveScene(lightScene);
            } catch (System.Exception) {
                Debug.Log("waiting for lighting scene to load");
                // throw; MWAHAHAHAHAH - this is bad and I feel bad
            }

            if (Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
                ScoreBoi.instance.RunGameClock(); //start clock - used to calculate score
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }

        // now this function returns
    }

    public void loadNextLevel()
    {
        currentLevelIndex++;
        loadLevel(currentLevelIndex);
    }
    //unload all scenes in given level index
    void unloadLevel(int index)
    {
        foreach (int sceneNum in mLevelScenes[index].sceneList)
        {
            SceneManager.UnloadSceneAsync(sceneNum);
        }
        loadedLevels.Add(index);
    }
    //unload the previous scene
    public void unloadPrevLevel()
    {
        if (loadedLevels.IndexOf(currentLevelIndex - 1) >= 0)
        {
            unloadLevel(currentLevelIndex - 1);
            loadedLevels.Remove(currentLevelIndex - 1);
        }
    }
    //unload everything but the current scene and persistent levels.
    public void unloadAllPrevLevels()
    {
        foreach (int sceneNum in loadedLevels)
        {
            if (sceneNum != currentLevelIndex)
            {
                unloadLevel(sceneNum);
            }
        }
        loadedLevels.Clear();
        loadedLevels.Add(currentLevelIndex);
    }

    public void UnloadAllScenes()
    {
        unloadLevel(currentLevelIndex);
    }
}
