using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ListWrapper
{
    public List<int> sceneList;
}

public class SceneLoadingBoi : MonoBehaviour
{
    // https://answers.unity.com/questions/242794/inspector-field-for-scene-asset.html
    // Check this out to make it easier to select scenes in editor

    [SerializeField] LoadingScreen loadingScreen;
    [SerializeField] int loadingSceneIndex;

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
        yield return new WaitForSeconds(1);

        if (loadingScreen.Hide())
        {
            SceneManager.UnloadSceneAsync(loadingSceneIndex);
        }
        else
        {
            StartCoroutine(LoadingWait());
        }
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
}
