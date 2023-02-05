using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AllosiusDevUtilities.Core;
using AllosiusDevCore;
using AllosiusDevUtilities.Audio;
using UnityEngine.UI;

public class GameCanvasManager : MonoBehaviour
{
    public TextMeshProUGUI creaturePlayerScoreText;
    public TextMeshProUGUI rootPlayerScoreText;
    public TextMeshProUGUI remainingTurnsText;

    public Image creaturePlayerIcon;
    public Image rootPlayerIcon;

    [SerializeField] private SceneData mainMenuSceneData;
    [SerializeField] private SceneData gameSceneData;

    public GameObject endGamePanel;
    public Image victoryPortrait;
    public TextMeshProUGUI creaturePlayerNumberColor;
    public TextMeshProUGUI rootPlayerNumberColor;

    public void SetEndGame(bool rootVictory, bool equality)
    {
        endGamePanel.SetActive(true);

        if(equality)
        {
            victoryPortrait.gameObject.SetActive(false);
            rootPlayerNumberColor.gameObject.SetActive(false);
            creaturePlayerNumberColor.gameObject.SetActive(false);
        }

        if(rootVictory)
        {
            victoryPortrait.sprite = rootPlayerIcon.sprite;
            rootPlayerNumberColor.gameObject.SetActive(true);
            rootPlayerNumberColor.text = GameCore.Instance.player.CurrentColorOwned.ToString();
            creaturePlayerNumberColor.gameObject.SetActive(false);
        }
        else
        {
            victoryPortrait.sprite = rootPlayerIcon.sprite;
            creaturePlayerNumberColor.gameObject.SetActive(true);
            creaturePlayerNumberColor.text = GameCore.Instance.creaturePlayer.CurrentColorOwned.ToString();
            rootPlayerNumberColor.gameObject.SetActive(false);
        }
    }

    public void RetryButton()
    {
        Time.timeScale = 1;

        GameStateManager.gameIsPaused = false;

        AudioController.Instance.StopAllMusics();

        SceneLoader.Instance.ActiveLoadingScreen(gameSceneData, 1.0f);
    }

    public void QuitButton()
    {
        Time.timeScale = 1;

        GameStateManager.gameIsPaused = false;

        AudioController.Instance.StopAllMusics();
        PauseMenu.canPause = false;
        SceneLoader.Instance.ActiveLoadingScreen(mainMenuSceneData, 1.0f);
    }
}
