using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagementScript : MonoBehaviour
{
    public MovementScript _MovementScript;

    private GameObjectSpawnerScript _gameObjectSpawnerScript;

    private TrackerScript _trackerScript;
    // Start is called before the first frame update
    void Start()
    {
        _MovementScript = FindObjectOfType<MovementScript>();
        _gameObjectSpawnerScript = FindObjectOfType<GameObjectSpawnerScript>();
        _trackerScript = FindObjectOfType<TrackerScript>();
    }

    // Update is called once per frame
    //Make a ref to your mousePosition
    //Make a reference to a specific collider. 
    //GetComponent<>();
    // if(collider.overlapPoint(mousePos))
    //{
    //
    //}
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //rename the beads to what spots they are on
        if ((gameObject.CompareTag("BlueBead")))
        {
            //other.gameObject.name
            string first17Characters = gameObject.name.Substring(0, Math.Min(gameObject.name.Length, 17));
            int Ycord;
            int Xcord;
            ExtractCoordinates(other.gameObject.name , out  Xcord , out Ycord);
            gameObject.name = first17Characters + " " + Xcord + "," + Ycord ;
            GreySpotDetector(true, gameObject.name,   Xcord,   Ycord);
            Debug.Log("Blue");
        }
        
        if ((gameObject.CompareTag("PurpleBead")))
        {
            //other.gameObject.name
            int Ycord;
            int Xcord;
            string first19Characters = gameObject.name.Substring(0, Math.Min(gameObject.name.Length, 19));
            ExtractCoordinates(other.gameObject.name , out Xcord , out Ycord);
            gameObject.name = first19Characters + " " + Xcord + "," + Ycord ;
            GreySpotDetector(false,gameObject.name,   Xcord,   Ycord);
        }
        
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(other.gameObject.transform.position);
  
        if (hitColliders != null && hitColliders.Length >= 3 )
        {

            // Move the object underneath
            Transform objectTransform = hitColliders[1].transform;
            Vector3 newPosition = objectTransform.position + _trackerScript.LastDirection; // Move 1 unit to 
            objectTransform.position = newPosition;
            
        }
    }

    private void GreySpotDetector(bool IsBlue ,string Bead ,  int  x , int y)
    {
      
     
        if ((x == -4 && y == 0) || (x == -2 && y == 0) || ( x == 0 && y == 3) || ( x == 0 && y == -1 ) || (x == 0 && y == -3) || (x == 2 && y == 0) || (x == 4 && y == 0) )
        {
           Destroy(GameObject.Find(Bead));
           
           if (IsBlue)
           {
               _trackerScript.Increment(TrackerScript.Score.BlueScore);
           }
           else
           {
               _trackerScript.Increment(TrackerScript.Score.PurpleScore);
           }
           
        }
        
    }

    private void RedSpotDetector()
    {
        
    }

    private void GreenSpotDetector()
    {
        
    }

    private void MiddleSpot(bool IsBlue ,GameObject Bead ,out int x , out int y)
    {
        x = 6;
        y = 6;
        if (x == 0 && y == 0)
        {
            
        }
    }
    static void ExtractCoordinates(string input, out int x, out int y)
    {
        
        // Split the string by space and comma
        string[] parts = input.Split(' ', ',');

        // Initialize variables
        x = 6;
        y = 6;

        // Try parsing the first number
        if (parts.Length > 1 && !int.TryParse(parts[1], out x))
        {
            x = 6; // default to 0 if parsing fails
        }

        // Try parsing the second number
        if (parts.Length > 2 && !int.TryParse(parts[2], out y))
        {
            y = 6; // default to 0 if parsing fails
        }
    }
}
