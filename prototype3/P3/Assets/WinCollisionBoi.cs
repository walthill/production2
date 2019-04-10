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
    DisplayScore displayScore;
    bool resetPlayer = false;
   

    private void Awake()
    {
        instance = this;
        fadeCanvasAnim = gameObject.GetComponent<Animator>();
        displayScore = GameObject.FindGameObjectWithTag("Finish").GetComponent<DisplayScore>();
    }

    public void PlayerHitWinTrigger(GameObject player)
    {
        SpeedThresholdBoi sp = player.GetComponent<SpeedThresholdBoi>();

        if (sp.getMaxSpeedChannel() > respawnEnabledChannel) //This condition can be altered as desired when it comes to design
            displayScore.PlayerWin();
        else
        {
            fadeCanvasAnim.SetBool("respawn", true);
        }
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