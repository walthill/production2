using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class WinCollisionBoi : MonoBehaviour
{
    public static WinCollisionBoi instance;

    [SerializeField] Transform playerRespawnPoint = null;
    [SerializeField] SpeedChannel respawnEnabledChannel = SpeedChannel.QUICK;

    Animator fadeCanvasAnim;
    bool resetPlayer = false;
   

    private void Awake()
    {
        instance = this;
        fadeCanvasAnim = gameObject.GetComponent<Animator>();
    }

    public void PlayerHitWinTrigger(GameObject player)
    {
        SpeedThresholdBoi sp = player.GetComponent<SpeedThresholdBoi>();

        if (sp.getMaxSpeedChannel() > respawnEnabledChannel) //This condition can be altered as desired when it comes to design
        {
            StartCoroutine(WaitAndLoadWinScene(1.75f));
        }
        else
        {
            fadeCanvasAnim.SetBool("respawn", true);
        }
    }

    IEnumerator WaitAndLoadWinScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ScoreBoi.instance.HideClock();
        LoadingSceneRelay.instance.UnloadLevelScenes();
        SceneManager.LoadScene("WinScreen", LoadSceneMode.Additive);
    }

        private void Update()
    {
        if (resetPlayer)
            PlayerSceneRelay.instance.ResetPlayer(playerRespawnPoint); //lock player controls
    }

    

    public void ToggleResetPlayer()
    {
        if (resetPlayer)
        {
            resetPlayer = false;
            fadeCanvasAnim.SetBool("respawn", false);
            fadeCanvasAnim.Play("Wait", -1);
        }
        else
        {
            resetPlayer = true;
        }
    }
}