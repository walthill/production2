using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneRelay : MonoBehaviour
{
    public static LoadingSceneRelay instance;

    SceneLoadingBoi boi;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        boi = GameObject.FindGameObjectWithTag("SceneBoi").GetComponent<SceneLoadingBoi>();
    }

    public void UnloadLevelScenes()
    {
        boi.UnloadAllScenes();
    }
}
