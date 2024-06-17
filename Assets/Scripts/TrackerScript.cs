using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TrackerScript : MonoBehaviour
{
    [FormerlySerializedAs("BlueScoretxt")] public Text blueScoretxt;
    [FormerlySerializedAs("PurpleScoretxt")] public Text purpleScoretxt;
    private int _blueScore, _purpleScore;
    [FormerlySerializedAs("MinScore")] public int minScore = 0;
    public UIManager uiManager;
    [FormerlySerializedAs("Up")] public bool up;
    [FormerlySerializedAs("Down")] public bool down;
    [FormerlySerializedAs("Left")] public bool left;
    [FormerlySerializedAs("Right")] public bool right;

    private void Start()
    {
        BlueScore = PurpleScore = 26;
        up = down = left = right = false;
    }
    
    private void Update()
    {
        if (up)
        {
            down = left = right = false;
        }
        else if(down)
        {
            up = right = left = false;
        }
        else if (right)
        {
            up = down = left = false;
        }
        else if (left)
        {
            up = down = right = false;
        }
    }

    public enum Score
    {
        BlueScore, PurpleScore
    }
    
    
    public int BlueScore
    {
        get => _blueScore;
        private set
        {
            _blueScore = value;
            if (value == minScore)          
                uiManager.ShowRestart(false);         
        }
    }
    public int PurpleScore
    {
        get => _purpleScore;
        private set
        {
            _purpleScore = value;
            if (value == minScore)
                uiManager.ShowRestart(true);
        }
    }


    public void Decrement(Score whichScore)
    {
        if (whichScore == Score.BlueScore)
        {
            blueScoretxt.text = (--BlueScore).ToString();
        }
        else
        {
            purpleScoretxt.text = (--PurpleScore).ToString();
        }
    }
    
    public void Increment(Score whichScore)
    {
        if (whichScore == Score.BlueScore)
        {
            blueScoretxt.text = (++BlueScore).ToString();
        }
        else
        {
            purpleScoretxt.text = (++PurpleScore).ToString();
        }
    }

    public void ResetScores()
    {
        BlueScore = PurpleScore = 26;
        blueScoretxt.text = purpleScoretxt.text = "26";
    }

}

