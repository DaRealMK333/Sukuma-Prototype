using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
        [Header("Canvas")]
        public GameObject CanvaGame;
        public GameObject CanvaRestart;

        [Header("CanvasRestart")]
        public GameObject BlueWinTxt;
        public GameObject PurpleWinTxt;

        [Header("Other")]
        public TrackerScript trackerScript;

        public void showRestart(bool BlueWin)
        {
            Time.timeScale = 0;

            CanvaGame.SetActive(false);
            CanvaRestart.SetActive(true);

            if (BlueWin)
            {
                BlueWinTxt.SetActive(true);
                PurpleWinTxt.SetActive(false);
            }
            else
            {
                BlueWinTxt.SetActive(false);
                PurpleWinTxt.SetActive(true);
            }
        }

        public void RestartGame()
        {
            Time.timeScale = 1;

            CanvaGame.SetActive(true);
            CanvaRestart.SetActive(false);

            trackerScript.ResetScores();
        }
    }
