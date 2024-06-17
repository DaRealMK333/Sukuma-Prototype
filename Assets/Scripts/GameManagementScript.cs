using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManagementScript : MonoBehaviour
{
    private TrackerScript _trackerScript;
    public LayerMask beadLayer;
    public float rayDistance = 3f;
    private List<string> _beadsInRow;
    //list or array to store the red beads everytime its in the spot
    //check if all the beads 

    
    // Start is called before the first frame update
    private DataManagerScript _dataManagerScript;
    private GameObjectSpawnerScript _gameObjectSpawnerScript;
    private void Start()
    {
        //FindObjectOfType<GameObjectSpawnerScript>();
        _trackerScript = FindObjectOfType<TrackerScript>();
        _dataManagerScript = FindObjectOfType<DataManagerScript>();
        _gameObjectSpawnerScript = FindObjectOfType<GameObjectSpawnerScript>();
        
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //rename the beads to what spots they are on
        if ((gameObject.CompareTag("BlueBead")))
        {
            //other.gameObject.name
            var first17Characters = gameObject.name[..Math.Min(gameObject.name.Length, 17)];
            ExtractCoordinates(other.gameObject.name , out var xcord , out var ycord);
            gameObject.name = first17Characters + " " + xcord + "," + ycord ;
            ExtractCoordinates(gameObject.name, out xcord, out ycord);
            GreySpotDetector(true, gameObject.name,   xcord,   ycord);
            RedSpotDetector(other,true,xcord,ycord);
            RedSpotBeadRemover();
            OutOfBoundSpotDectector(gameObject.name , xcord , ycord);
            BlueBeadCoordinateStorer();
            MiddleSpot(gameObject.name, xcord,ycord);
        }
        
        if ((gameObject.CompareTag("PurpleBead")))
        {
            //other.gameObject.name
            var first19Characters = gameObject.name[..Math.Min(gameObject.name.Length, 19)];
            ExtractCoordinates(other.gameObject.name , out var xcord , out var ycord);
            gameObject.name = first19Characters + " " + xcord + "," + ycord ;
            ExtractCoordinates(gameObject.name , out  xcord , out ycord);
            GreySpotDetector(false,gameObject.name,   xcord,   ycord);
            RedSpotDetector(other,false,xcord,ycord);
            RedSpotBeadRemover();
            OutOfBoundSpotDectector(gameObject.name , xcord , ycord);
            PurpleBeadCoordinateStorer();
            MiddleSpot(gameObject.name,xcord,ycord);
        }
        
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(other.gameObject.transform.position);
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //stores this converted mouse position in a Vector2 variable named mousePosition
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

       if (hit.collider != null)
       {
           ExtractCoordinates(hit.collider.gameObject.name, out int xcord, out int ycord);

           if (hitColliders != null && hitColliders.Length >= 3 && xcord == -5)
           {
               // Move the object underneath
               Transform objectTransform = hitColliders[1].transform;
               Vector3 newPosition = objectTransform.position + Vector3.right; // Move 1 unit to 
               objectTransform.position = newPosition;

           }

           if (hitColliders != null && hitColliders.Length >= 3 && ycord == -5)
           {
               // Move the object underneath
               Transform objectTransform = hitColliders[1].transform;
               Vector3 newPosition = objectTransform.position + Vector3.up; // Move 1 unit to 
               objectTransform.position = newPosition;

           }

           if (hitColliders != null && hitColliders.Length >= 3 && xcord == 5)
           {
               // Move the object underneath
               Transform objectTransform = hitColliders[1].transform;
               Vector3 newPosition = objectTransform.position + Vector3.left; // Move 1 unit to 
               objectTransform.position = newPosition;

           }

           if (hitColliders != null && hitColliders.Length >= 3 && ycord == 5)
           {
               // Move the object underneath
               Transform objectTransform = hitColliders[1].transform;
               Vector3 newPosition = objectTransform.position + Vector3.down; // Move 1 unit to 
               objectTransform.position = newPosition;

           }
       }
    }

    private void GreySpotDetector(bool isBlue ,string bead ,  int  x , int y)
    {
        if ((x == -4 && y == 0) || (x == -2 && y == 0) || ( x == 0 && y == 3) || ( x == 0 && y == 1 )|| ( x == 0 && y == -1 ) || (x == 0 && y == -3) || (x == 2 && y == 0) || (x == 4 && y == 0) )
        {
           Destroy(GameObject.Find(bead));
          // Debug.Log("Deleted :" +bead);
           if (isBlue)
           {
               BlueBeadAdder();
               
           }
           else
           {
                PurpleBeadAdder();
     
           }
        }
    }

    private void BlueBeadAdder()
    {
        _trackerScript.Increment(TrackerScript.Score.BlueScore);
        // Find all GameObjects with the tag "bluebeads"
        GameObject[] blueBeads = GameObject.FindGameObjectsWithTag("BlueBead");

        // Iterate through the array and print the name of each GameObject
        foreach (GameObject blueBead in blueBeads)
        {
            blueBead.name = IncrementNumberInString(blueBead.name);
            // Debug.Log("Change names");
        }
    }

    private void PurpleBeadAdder()
    {
        _trackerScript.Increment(TrackerScript.Score.PurpleScore);
        // Find all GameObjects with the tag "purplebeads"
        GameObject[] purpleBeads = GameObject.FindGameObjectsWithTag("PurpleBead");

        // Iterate through the array and print the name of each GameObject
        foreach (GameObject purpleBead in purpleBeads)
        { 
            purpleBead.name = IncrementNumberInString(purpleBead.name);
        }
    }
    private void FindObjectsInRange(float minX, float maxX, float minY, float maxY ,ref GameObject[] objectsInRange)
    {
        List<GameObject> tempObjectsInRange = new List<GameObject>();

        // Get all game objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects )
        {
            Vector3 position = obj.transform.position;

            // Check if the object's position is within the specified ranges
            if (position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY &&(obj.name.StartsWith("BlueBead") ||obj.name.StartsWith("PurpleBead")))
            {
                tempObjectsInRange.Add(obj);
                if (tempObjectsInRange.Count >= 4)
                {
                    break;
                }
            }
        }

     // Copy the found objects to the fixed-size array
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            if (i < tempObjectsInRange.Count)
            {
                objectsInRange[i] = tempObjectsInRange[i];
            }
            else
            {
                objectsInRange[i] = null;
            }
        }
    }

    private bool BlueCheckAllObjectsAreBlueBeads(GameObject[] objectsInRange)
    {
        
        foreach (GameObject obj in objectsInRange)
        {
            if (obj.name != null)
            {
                if (!obj.name.StartsWith("BlueBead") || obj.name == null)
                {
                    // If any object does not start with "BlueBeads", return false
                    return false;
                }
            }
       
        }

        // If all objects start with "BlueBeads", return true
        return true;
    }
    
    private bool PurpleCheckAllObjectsAreBlueBeads(GameObject[] objectsInRange)
    {
        foreach (GameObject obj in objectsInRange)
        {
            if (obj != null)
            {
                if (!obj.name.StartsWith("PurpleBead") || obj.name == null)
                {
                    // If any object does not start with "BlueBeads", return false
                    return false;
                }
            }
      
        }

        // If all objects start with "BlueBeads", return true
        return true;
    }

    void LogObjectsInRange(GameObject[] objectsInRange)
    {
        Debug.Log("Objects within specified range:");
        foreach (GameObject obj in objectsInRange)
        {
            if (obj != null)
            Debug.Log("GameObject: " + obj.name + ", Position: " + obj.transform.position);
        }
    }
    
    private void RedSpotDetector(Collider2D other,bool isBlue, int  x , int y)
    {

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(other.gameObject.transform.position);
        
        //Top Left Square
        if (x == -2 && y == 2 && hitColliders.Length > 1)
        {
            _dataManagerScript.topLeftSpots.Spot1 = true;
        }
        else if (x == -2 && y == 2 && hitColliders.Length < 2)
        {
            _dataManagerScript.topLeftSpots.Spot1 = false;
        }
        
        if (x == -2 && y == 1 && hitColliders.Length > 1)
        {
            _dataManagerScript.topLeftSpots.Spot2 = true;
        }
        else if (x == -2 && y == 1 && hitColliders.Length < 2)
        {
            _dataManagerScript.topLeftSpots.Spot2 = false;
        }

      
        if (x == -1 && y == 2 && hitColliders.Length > 1)
        {
            _dataManagerScript.topLeftSpots.Spot3 = true;
        }
        else if (x == -1 && y == 2 && hitColliders.Length < 2)
        {
            _dataManagerScript.topLeftSpots.Spot3 = false;
        }
        
        if (x == -1 && y == 1 && hitColliders.Length > 1 )
        {
            _dataManagerScript.topLeftSpots.Spot4 = true;
        }
        else if (x == -1 && y == 1 && hitColliders.Length < 2)
        {
            _dataManagerScript.topLeftSpots.Spot4 = false;
        }

        if (_dataManagerScript.topLeftSpots.AllTrue())
        {
            FindObjectsInRange(-2, -1, 1, 2, ref _dataManagerScript.topLeftObjects);
            LogObjectsInRange(_dataManagerScript.topLeftObjects);
            _dataManagerScript.topLeftSpots.IsAllBlue = BlueCheckAllObjectsAreBlueBeads(_dataManagerScript.topLeftObjects);
            _dataManagerScript.topLeftSpots.IsAllPurple =PurpleCheckAllObjectsAreBlueBeads(_dataManagerScript.topLeftObjects);
        }


        //Top Right Square
        if (x == 1 && y == 2 && hitColliders.Length > 1)
        {
            _dataManagerScript.topRightSpots.Spot1 = true;
        }
        else if (x == 1 && y == 2 && hitColliders.Length < 2)
        {
            _dataManagerScript.topRightSpots.Spot1 = false;
        }


        if (x == 1 && y == 1 && hitColliders.Length > 1)
        {
            _dataManagerScript.topRightSpots.Spot2 = true;
        }
        else if (x == 1 && y == 2 && hitColliders.Length < 2)
        {
            _dataManagerScript.topRightSpots.Spot2 = false;
        }

        
        if (x == 2 && y == 2 && hitColliders.Length > 1)
        {
            _dataManagerScript.topRightSpots.Spot3 = true;
        }
        else if (x == 2 && y == 2 && hitColliders.Length < 2)
        {
            _dataManagerScript.topRightSpots.Spot3 = false;
        }


        if (x == 2 && y == 1 && hitColliders.Length > 1)
        {
            _dataManagerScript.topRightSpots.Spot4 = true;
        }
        else if (x == 2 && y == 1 && hitColliders.Length < 2)
        {
            _dataManagerScript.topRightSpots.Spot4 = false;
        }

        if (_dataManagerScript.topRightSpots.AllTrue())
        {
            FindObjectsInRange(1, 2, 1, 2, ref _dataManagerScript.topRightObjects);
            _dataManagerScript.topRightSpots.IsAllBlue = BlueCheckAllObjectsAreBlueBeads(_dataManagerScript.topRightObjects);
            _dataManagerScript.topRightSpots.IsAllPurple = PurpleCheckAllObjectsAreBlueBeads(_dataManagerScript.topRightObjects);
        }

        
        //Bottom Left Square
        if (x == -2 && y == -1 && hitColliders.Length > 1)
        {
            _dataManagerScript.bottomLeftSpots.Spot1 = true;
        }
        else if (x == -2 && y == -1 && hitColliders.Length < 2)
        {
            _dataManagerScript.bottomLeftSpots.Spot1 = false;
        }

        if (x == -2 && y == -2 && hitColliders.Length > 1)
        {
            _dataManagerScript.bottomLeftSpots.Spot2 = true;
        }
        else if(x == -2 && y == -2 && hitColliders.Length < 2)
        {
            _dataManagerScript.bottomLeftSpots.Spot2 = false;
        }

        if (x == -1 && y == -1  && hitColliders.Length > 1)
        {
            _dataManagerScript.bottomLeftSpots.Spot3 = true;
        }
        else if (x == -1 && y == -1  && hitColliders.Length < 2)
        {
            _dataManagerScript.bottomLeftSpots.Spot3 = false;
        }

        if (x == -1 && y == -2 && hitColliders.Length > 1)
        {
            _dataManagerScript.bottomLeftSpots.Spot4 = true;
        }
        else if (x == -1 && y == -2 && hitColliders.Length < 2)
        {
            _dataManagerScript.bottomLeftSpots.Spot4 = false;
        }

        if (_dataManagerScript.bottomLeftSpots.AllTrue())
        {
            FindObjectsInRange(-2, -1, -2, -1, ref _dataManagerScript.bottomLeftObjects);
            _dataManagerScript.bottomLeftSpots.IsAllBlue = BlueCheckAllObjectsAreBlueBeads(_dataManagerScript.bottomLeftObjects);   
            _dataManagerScript.bottomLeftSpots.IsAllPurple = PurpleCheckAllObjectsAreBlueBeads(_dataManagerScript.bottomLeftObjects); 
        }

        
        //Bottom Right Square
        if (x == 1 && y == -1 && hitColliders.Length > 1)
        {
            _dataManagerScript.bottomRightSpots.Spot1 = true;
        }
        else if (x == 1 && y == -1 && hitColliders.Length < 2)
        {
            _dataManagerScript.bottomRightSpots.Spot1 = false;
        }

        if (x == 1 && y == -2 && hitColliders.Length > 1)
        {
            _dataManagerScript.bottomRightSpots.Spot2 = true;
        }
        else if (x == 1 && y == -2 && hitColliders.Length < 2 )
        {
            _dataManagerScript.bottomRightSpots.Spot2 = false;
        }

        if (x == 2 && y == -1 && hitColliders.Length > 1)
        {
            _dataManagerScript.bottomRightSpots.Spot3 = true;
        }
        else if (x == 2 && y == -1 && hitColliders.Length < 2)
        {
            _dataManagerScript.bottomRightSpots.Spot3 = true;
        }
        
        if (x == 2 && y == -2 && hitColliders.Length > 1)
        {
            _dataManagerScript.bottomRightSpots.Spot4 = true;
        }
        else if (x == 2 && y == -2 && hitColliders.Length < 2)
        {
            _dataManagerScript.bottomRightSpots.Spot4 = false;
        }

        if (_dataManagerScript.bottomRightSpots.AllTrue())
        {
            FindObjectsInRange(1, 2, -2, -1,ref _dataManagerScript.bottomRightObjects );
            _dataManagerScript.bottomRightSpots.IsAllBlue = BlueCheckAllObjectsAreBlueBeads(_dataManagerScript.bottomRightObjects);
            _dataManagerScript.bottomRightSpots.IsAllPurple = PurpleCheckAllObjectsAreBlueBeads(_dataManagerScript.bottomRightObjects);
        }
        
    }

    public void DestroyBlueBeadsInArea(float minX, float maxX, float minY, float maxY, bool isCurrentPlayerSpace,
        ref GameObject[] objectsInRange)
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check if the object's name starts with "bluebeads"
            if (obj.name.StartsWith("BlueBead"))
            {
                // Get the object's position
                Vector3 position = obj.transform.position;

                // Check if the object is within the specified x and y coordinates
                if (position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY)
                {
                    // Destroy the object
                    Destroy(obj);
                }

            }
        }

        foreach (GameObject obj in allObjects)
        {
            // Check if the object's name starts with "bluebeads"
            if (obj.name.StartsWith("PurpleBead"))
            {
                Vector3 position = obj.transform.position;
                if (isCurrentPlayerSpace)
                {
                    if (!objectsInRange.Contains(obj) && position.x >= minX && position.x <= maxX &&
                        position.y >= minY && position.y <= maxY)
                    {
                        Destroy(obj);
                        PurpleBeadAdder();
                    }

                }
            }
        }
    }

    public void DestroyPurpleBeadsInArea(float minX, float maxX, float minY, float maxY ,bool isCurrentPlayerSpace , ref GameObject[] objectsInRange)
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check if the object's name starts with "bluebeads"
            if (obj.name.StartsWith("PurpleBead"))
            {
                // Get the object's position
                Vector3 position = obj.transform.position;

                // Check if the object is within the specified x and y coordinates
                if (position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY)
                {
                    // Destroy the object
                    Destroy(obj);
                }

         
            }
        }
        foreach (GameObject obj in allObjects)
        {
            // Check if the object's name starts with "bluebeads"
            if (obj.name.StartsWith("BlueBead"))
            {
                Vector3 position = obj.transform.position;
                if (isCurrentPlayerSpace)
                {
                    if (!objectsInRange.Contains(obj) && position.x >= minX && position.x <= maxX &&
                        position.y >= minY && position.y <= maxY)
                    {
                        Destroy(obj);
                        BlueBeadAdder();
                    }

                }
            }
        }
    }
    private void RedSpotBeadRemover()
    {
        if (_dataManagerScript.topLeftSpots.IsAllBlue)
        {
            DestroyPurpleBeadsInArea(-5,0,0 ,5,false , ref _dataManagerScript.topLeftObjects);
            DestroyPurpleBeadsInArea(-5,0,0 ,5,true , ref _dataManagerScript.topLeftObjects);
        }
        else if (_dataManagerScript.topLeftSpots.IsAllPurple)
        {
            DestroyBlueBeadsInArea(-5,0,0 ,5,false , ref _dataManagerScript.topLeftObjects);
            DestroyBlueBeadsInArea(-5,0,0 ,5,true , ref _dataManagerScript.topLeftObjects);
        }
        
        if (_dataManagerScript.topRightSpots.IsAllBlue)
        {
            DestroyPurpleBeadsInArea(0,5,0 ,5,false , ref _dataManagerScript.topRightObjects);
            DestroyPurpleBeadsInArea(0,5,0 ,5,true , ref _dataManagerScript.topRightObjects);
        }
        else if (_dataManagerScript.topRightSpots.IsAllPurple)
        {
            DestroyBlueBeadsInArea(0,5,0 ,5,false , ref _dataManagerScript.topRightObjects);
            DestroyBlueBeadsInArea(0,5,0 ,5,true , ref _dataManagerScript.topRightObjects);
        }
        
        if (_dataManagerScript.bottomLeftSpots.IsAllBlue)
        {
            DestroyPurpleBeadsInArea(-5,0,-5 ,0,false , ref _dataManagerScript.bottomLeftObjects);
            DestroyPurpleBeadsInArea(-5,0,-5 ,0,true , ref _dataManagerScript.bottomLeftObjects);
        }
        else if (_dataManagerScript.bottomLeftSpots.IsAllPurple)
        {
            DestroyBlueBeadsInArea(-5,0,-5 ,0,false , ref _dataManagerScript.bottomLeftObjects);
            DestroyBlueBeadsInArea(-5,0,-5 ,0,true , ref _dataManagerScript.bottomLeftObjects);
        }
        
        if (_dataManagerScript.bottomRightSpots.IsAllBlue)
        {
            DestroyPurpleBeadsInArea(0,5,-5 ,0,false , ref _dataManagerScript.bottomRightObjects);
            DestroyPurpleBeadsInArea(0,5,-5 ,0,true , ref _dataManagerScript.bottomRightObjects);
        }
        else if (_dataManagerScript.bottomRightSpots.IsAllPurple)
        {
            DestroyBlueBeadsInArea(0,5,-5 ,0, false , ref _dataManagerScript.bottomRightObjects);
            DestroyBlueBeadsInArea(0,5,-5 ,0, true , ref _dataManagerScript.bottomRightObjects);
        }
    }


    private void MiddleSpot(string bead , int x , int y)
    {
        if (x == 0 && y == 0)
        {
            Destroy(GameObject.Find(bead));
        }
    }

    private void OutOfBoundSpotDectector(string bead, int x, int y)
    {
         
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //stores this converted mouse position in a Vector2 variable named mousePosition
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (x == -5 || y == 5 ||  x == 5 || y == -5)
        {
            if (hit.collider != null)
            {
                if (hit.collider.name != bead)
                {
                    Destroy(GameObject.Find(bead)); 
                }
            }
 
        }
    }

    private void PurpleBeadCoordinateStorer()
     {
         if (_dataManagerScript.PurpleBeads != null)
         {
             _dataManagerScript.PurpleBeads.Clear();

             // Add all the bead coordinates to the list
             
             GameObject[] allObjects = FindObjectsOfType<GameObject>();
             //_dataManagerScript.Pi = 0;
             foreach (GameObject obj in allObjects)
             {
                 if (obj.name.StartsWith("PurpleBead"))
                 {
                    // _dataManagerScript.Pi++;
                    // Debug.Log("Object Purple :" + _dataManagerScript.Pi + obj.name);
                     ExtractCoordinates(obj.name, out int x, out int y);
                     //int x = Mathf.RoundToInt(obj.transform.position.x);
                     // int y = Mathf.RoundToInt(obj.transform.position.y);
                     _dataManagerScript.PurpleBeads.Add(new DataManagerScript.Bead(x, y));

                   //  Debug.Log(_dataManagerScript.Pi);
                 }
             }

             // Check for matches
             PurpleCheckForMatches();
             //Debug.Log("List: Purple " + _dataManagerScript.PurpleBeads.Count);
         }
     }

     private void PurpleCheckForMatches()
     {
         for (int i = 0; i < _dataManagerScript.PurpleBeads.Count; i++)
         {
             DataManagerScript.Bead bead = _dataManagerScript.PurpleBeads[i];

             // Check horizontally
             if (PurpleCheckLine(bead.X, bead.Y, 1, 0, out string direction))
             {
                 //DoSomething(bead, direction);
                 //Debug.Log("Purple Direction "+direction);
                 _dataManagerScript.PurpleDirection.Horizontal = true;
                 PurpleBeadMatchesCoordinates(direction , _dataManagerScript.PurpleBeads);
                 break;
             }
             // Check vertically
             if (PurpleCheckLine(bead.X, bead.Y, 0, 1, out direction))
             {
                 //DoSomething(bead, direction);
                 //Debug.Log("purple Direction "+direction);
                 _dataManagerScript.PurpleDirection.Vertical = true;
                 PurpleBeadMatchesCoordinates(direction , _dataManagerScript.PurpleBeads);
                 break;
             }
             // Check diagonally (positive slope)
             if (PurpleCheckLine(bead.X, bead.Y, 1, 1, out direction))
             {
                 //DoSomething(bead, direction);
                 //Debug.Log("Purple Direction "+direction);
                 _dataManagerScript.PurpleDirection.PostiveDiagonal = true;
                 PurpleBeadMatchesCoordinates(direction , _dataManagerScript.PurpleBeads);
                 break;
             }
             // Check diagonally (negative slope)
             if (PurpleCheckLine(bead.X, bead.Y, 1, -1, out direction))
             {
                 //DoSomething(bead, direction);
                // Debug.Log("Purple Direction "+direction);
                _dataManagerScript.PurpleDirection.NegativeDiagonal = true;
                PurpleBeadMatchesCoordinates(direction , _dataManagerScript.PurpleBeads);
                 break;
             }
         }
     }
     private void PurpleBeadMatchesCoordinates(string direction, List<DataManagerScript.Bead> matchCoordinates)
     {
//         Debug.Log(direction + " match found at: ");
         
        _dataManagerScript.purpleyCordList.Clear();
        _dataManagerScript.purplexCordList.Clear();
        _dataManagerScript.PurpleNegativeDiagonalList.Clear(); 
        _dataManagerScript.PurplePositiveDiagonalList.Clear();
        
         if (direction == "horizontal")
         {
             _dataManagerScript.PurpleDirection.Horizontal = true;
             foreach (DataManagerScript.Bead bead in matchCoordinates)
             {
                 //Debug.Log("(" + bead.X + ", " + bead.Y + ")"); 
                 // Add your action here for each matched bead coordinate
                 _dataManagerScript.purpleyCordList.Add(bead.Y);
             }
             
         }
         else if (direction == "vertical")
         {
             _dataManagerScript.BlueDirection.Vertical = true;
             foreach (DataManagerScript.Bead bead in matchCoordinates)
             {
                 //Debug.Log("(" + bead.X + ", " + bead.Y + ")"); 
                 // Add your action here for each matched bead coordinate
                 _dataManagerScript.purplexCordList.Add(bead.X);
             }
         }
         else if (direction == "diagonal_positive")
         {
             _dataManagerScript.BlueDirection.PostiveDiagonal = true;
             foreach (DataManagerScript.Bead bead in matchCoordinates)
             {
                 //Debug.Log("(" + bead.X + ", " + bead.Y + ")"); 
                 // Add your action here for each matched bead coordinate
                 _dataManagerScript.PurpleNegativeDiagonalList.AddRange(matchCoordinates);
             }
         }
         else if (direction == "diagonal_negative")
         {
             _dataManagerScript.BlueDirection.NegativeDiagonal = true;
             foreach (DataManagerScript.Bead bead in matchCoordinates)
             {
                 //Debug.Log("(" + bead.X + ", " + bead.Y + ")"); 
                 // Add your action here for each matched bead coordinate
                 _dataManagerScript.PurplePositiveDiagonalList.AddRange(matchCoordinates);
             }
         }
     }
     private bool PurpleCheckLine(int startX, int startY, int stepX, int stepY, out string direction)
     {
         int matchCount = 1;

         for (int i = 1; i <= 2; i++)
         {
             if (_dataManagerScript.PurpleBeads.Exists(b => b.X == startX + i * stepX && b.Y == startY + i * stepY))
             {
                 matchCount++;
             }
         }

         if (matchCount == 3)
         {
             if (stepX == 1 && stepY == 0)
             {
                 direction = "horizontal";
             }
             else if (stepX == 0 && stepY == 1)
             {
                 direction = "vertical";
             }
             else if (stepX == 1 && stepY == 1)
             {
                 direction = "diagonal_positive";
             }
             else if (stepX == 1 && stepY == -1)
             {
                 direction = "diagonal_negative";
             }
             else
             {
                 direction = "unknown";
                 _dataManagerScript.PurpleDirection =
                     new DataManagerScript.PurpleThreeInARowDircection(false, false, false, false);
             }
            // Debug.Log("Current Purple Direction "+direction);
             return true;
         }

         direction = "none";
        // Debug.Log("Current Purple Direction "+direction);
         return false;
     }

     private void BlueBeadCoordinateStorer()
     {
         if (_dataManagerScript.BlueBeads != null)
         {
             _dataManagerScript.BlueBeads.Clear();
             
             // Add all the bead coordinates to the list
             GameObject[] allObjects = FindObjectsOfType<GameObject>();
             _dataManagerScript.Bi = 0;
             foreach (GameObject obj in allObjects)
             {
                 if (obj.name.StartsWith("BlueBead"))
                 {
                     _dataManagerScript.Bi++;
                     //Debug.Log("Object BlueBead :" + _dataManagerScript.Bi + obj.name);
                     ExtractCoordinates(obj.name, out int x, out int y);
                     //int x = Mathf.RoundToInt(obj.transform.position.x);
                     //int y = Mathf.RoundToInt(obj.transform.position.y);
                     _dataManagerScript.BlueBeads.Add(new DataManagerScript.Bead(x, y));
                     //Debug.Log(_dataManagerScript.Bi);
                 }
             }

             // Check for matches
             BlueCheckForMatches();
             //BlueBeadMatchesCoordinates();
           //  Debug.Log("List: BlueBead " + _dataManagerScript.BlueBeads.Count);
         }
     }


     private void BlueCheckForMatches()
     {
         for (int i = 0; i < _dataManagerScript.BlueBeads.Count; i++)
         {
             DataManagerScript.Bead bead = _dataManagerScript.BlueBeads[i];

             // Check horizontally
             if (BlueCheckLine(bead.X, bead.Y, 1, 0, out string direction))
             {
                 //DoSomething(bead, direction);
                 // Debug.Log("Blue Direction "+ direction);
                 _dataManagerScript.BlueDirection.Horizontal = true;
                 BlueBeadMatchesCoordinates(direction,_dataManagerScript.BlueBeads);
                 break;
             }
             // Check vertically
             if (BlueCheckLine(bead.X, bead.Y, 0, 1, out direction))
             {
                 //DoSomething(bead, direction);
                 //Debug.Log("Blue Direction "+ direction);
                 _dataManagerScript.BlueDirection.Vertical = true;
                 BlueBeadMatchesCoordinates(direction,_dataManagerScript.BlueBeads);
                 break;
             }
             // Check diagonally (positive slope)
             if (BlueCheckLine(bead.X, bead.Y, 1, 1, out direction))
             {
                 //DoSomething(bead, direction);
                 //Debug.Log("Blue Direction "+direction);
                 _dataManagerScript.BlueDirection.PostiveDiagonal = true;
                 BlueBeadMatchesCoordinates(direction,_dataManagerScript.BlueBeads);
                 break;
             }
             // Check diagonally (negative slope)
             if (BlueCheckLine(bead.X, bead.Y, 1, -1, out direction))
             {
                 //DoSomething(bead, direction);
                // Debug.Log("Blue Direction "+direction);
                 _dataManagerScript.BlueDirection.NegativeDiagonal = true;
                 BlueBeadMatchesCoordinates(direction,_dataManagerScript.BlueBeads);
                 break;
             }
         }
         
     }
     
       private void BlueBeadMatchesCoordinates(string direction, List<DataManagerScript.Bead> matchCoordinates)
     {
        // Debug.Log(direction + " match found at: ");
         
        _dataManagerScript.blueyCordList.Clear();
        _dataManagerScript.bluexCordList.Clear();
        _dataManagerScript.BlueNegativeDiagonalList.Clear(); 
        _dataManagerScript.BluePositiveDiagonalList.Clear();
        
         if (direction == "horizontal")
         {
             _dataManagerScript.BlueDirection.Horizontal = true;
             foreach (DataManagerScript.Bead bead in matchCoordinates)
             {
                 //Debug.Log("(" + bead.X + ", " + bead.Y + ")"); 
                 // Add your action here for each matched bead coordinate
                 _dataManagerScript.blueyCordList.Add(bead.Y);
             }
             
         }
         else if (direction == "vertical")
         {
             _dataManagerScript.BlueDirection.Vertical = true;
             foreach (DataManagerScript.Bead bead in matchCoordinates)
             {
                 //Debug.Log("(" + bead.X + ", " + bead.Y + ")"); 
                 // Add your action here for each matched bead coordinate
                 _dataManagerScript.bluexCordList.Add(bead.X);
             }
         }
         else if (direction == "diagonal_positive")
         {
             _dataManagerScript.BlueDirection.PostiveDiagonal = true;
             foreach (DataManagerScript.Bead bead in matchCoordinates)
             {
                 //Debug.Log("(" + bead.X + ", " + bead.Y + ")"); 
                 // Add your action here for each matched bead coordinate
                 _dataManagerScript.BlueNegativeDiagonalList.AddRange(matchCoordinates);
             }
         }
         else if (direction == "diagonal_negative")
         {
             _dataManagerScript.BlueDirection.NegativeDiagonal = true;
             foreach (DataManagerScript.Bead bead in matchCoordinates)
             {
                 //Debug.Log("(" + bead.X + ", " + bead.Y + ")"); 
                 // Add your action here for each matched bead coordinate
                 _dataManagerScript.BluePositiveDiagonalList.AddRange(matchCoordinates);
             }
         }
     }
     

     private bool BlueCheckLine(int startX, int startY, int stepX, int stepY, out string direction)
     {
         int matchCount = 1;

         for (int i = 1; i <= 2; i++)
         {
             if (_dataManagerScript.BlueBeads.Exists(b => b.X == startX + i * stepX && b.Y == startY + i * stepY))
             {
                 matchCount++;
             }
         }

         if (matchCount == 3)
         {
             if (stepX == 1 && stepY == 0)
             {
                 direction = "horizontal";
                
             }
             else if (stepX == 0 && stepY == 1)
             {
                 direction = "vertical";
                 
             }
             else if (stepX == 1 && stepY == 1)
             {
                 direction = "diagonal_positive";
                 
             }
             else if (stepX == 1 && stepY == -1)
             {
                 direction = "diagonal_negative";
                
             }
             else
             {
                 direction = "unknown";
                 _dataManagerScript.BlueDirection =
                     new DataManagerScript.BlueThreeInARowDircection(false, false, false, false);
             }
           //  Debug.Log("Current Blue Direction: " + direction);
             return true;
         }

         direction = "none";
       //  Debug.Log("Current Blue Direction: " + direction);
         return false;
     }

   
    


   /*  bool CheckLine(int startX, int startY, int stepX, int stepY)
     {
         int matchCount = 1;

         for (int i = 1; i <= 2; i++)
         {
             if (_beads.Exists(b => b.X == startX + i * stepX && b.Y == startY + i * stepY))
             {
                 matchCount++;
             }
         }

         return matchCount == 3;
     }
     */
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
    
    static string IncrementNumberInString(string input)
    {
        // Regular expression to find the number in the string
        string pattern = @"\d+";
        Match match = Regex.Match(input, pattern);

        if (match.Success)
        {
            // Parse the extracted number
            int extractedNumber = int.Parse(match.Value);
        
            // Increment the number
            int incrementedNumber = extractedNumber + 1;
        
            // Replace the original number with the incremented number in the string
            string updatedString = Regex.Replace(input, pattern, incrementedNumber.ToString());
        
           // Debug.Log("Updated string: " + updatedString); // For debugging purposes
            return updatedString;
        }
        else
        {
         //   Debug.Log("No number found in the input string.");
            return input; // Return the original string if no number is found
        }
    }
}
