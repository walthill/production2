using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneRelay : MonoBehaviour
{
    public static LoadingSceneRelay instance;
	LoadingScreen loadingScreen;
    SceneLoadingBoi boi;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
		loadingScreen = GameObject.FindGameObjectWithTag("LoadCanvas").GetComponent<LoadingScreen>();
        boi = GameObject.FindGameObjectWithTag("SceneBoi").GetComponent<SceneLoadingBoi>();
    }

    public void UnloadLevelScenes()
    {
        boi.UnloadAllScenes();
    }
	
	public bool AbleToPause()
	{
		return loadingScreen.AbleToPause();
	}
}
