using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
        [FormerlySerializedAs("CanvaGame")] [Header("Canvas")]
        public GameObject canvaGame;
        [FormerlySerializedAs("CanvaRestart")] public GameObject canvaRestart;

        [FormerlySerializedAs("BlueWinTxt")] [Header("CanvasRestart")]
        public GameObject blueWinTxt;
        [FormerlySerializedAs("PurpleWinTxt")] public GameObject purpleWinTxt;

        [Header("Other")]
        public TrackerScript trackerScript;

        public void ShowRestart(bool blueWin)
        {
            Time.timeScale = 0;

            canvaGame.SetActive(false);
            canvaRestart.SetActive(true);

            if (blueWin)
            {
                blueWinTxt.SetActive(true);
                purpleWinTxt.SetActive(false);
            }
            else
            {
                blueWinTxt.SetActive(false);
                purpleWinTxt.SetActive(true);
            }
        }

        public void RestartGame()
        {
            Time.timeScale = 1;

            canvaGame.SetActive(true);
            canvaRestart.SetActive(false);

            trackerScript.ResetScores();
        }
        
      
    }


    