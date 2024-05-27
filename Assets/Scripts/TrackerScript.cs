using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackerScript : MonoBehaviour
{
    private void Start()
    {
        BlueScore = PurpleScore = 26;
    }

    public enum Score
    {
        BlueScore, PurpleScore
    }

    public Text BlueScoretxt, PurpleScoretxt;
    private int blueScore, purpleScore;
    public int MinScore = 0;
    public UIManager uiManager;
    public Vector3 LastDirection;
    public int BlueScore
    {
        get { return blueScore; }
        set
        {
            blueScore = value;
            if (value == MinScore)          
                uiManager.showRestart(false);         
        }
    }
    public int PurpleScore
    {
        get { return purpleScore; }
        set
        {
            purpleScore = value;
            if (value == MinScore)
                uiManager.showRestart(true);
        }
    }


    public void Decrement(Score whichScore)
    {
        if (whichScore == Score.BlueScore)
        {
            BlueScoretxt.text = (--BlueScore).ToString();
        }
        else
        {
            PurpleScoretxt.text = (--PurpleScore).ToString();
        }
    }
    
    public void Increment(Score whichScore)
    {
        if (whichScore == Score.BlueScore)
        {
            BlueScoretxt.text = (++BlueScore).ToString();
        }
        else
        {
            PurpleScoretxt.text = (++PurpleScore).ToString();
        }
    }

    public void ResetScores()
    {
        BlueScore = PurpleScore = 26;
        BlueScoretxt.text = PurpleScoretxt.text = "26";
    }

}

