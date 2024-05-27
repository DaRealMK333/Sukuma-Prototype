using System;
using UnityEngine;

public class GameObjectSpawnerScript : MonoBehaviour
{
    public GameObject boardColliders;
    public GameObject BlueBead;
    public GameObject PurpleBead;
    public GameObject[] ArrBlueBead = new GameObject[26];
    public GameObject[] ArrPurpleBead = new GameObject[26];
    public GameObject CircleHighlight;

    public string[] ArrBlueBeadName = new string[26];
    public string[] ArrPurpleBeadName = new string[26];

    private bool[,] IsBeadPlaced = new bool[9, 9];
    private bool IsPlayerBlueTurn = true;
    private bool Ishovering;
    
    public GameObjectDestroyer Destroyer;
    public TrackerScript _TrackerScript;
    public bool DirectionBeadPlaced = false;
   
    // Start is called before the first frame update
    void Start()
    {
        _TrackerScript = FindObjectOfType<TrackerScript>();
        
        for (int i = 0; i < ArrBlueBead.Length ; i++)
        {
            ArrBlueBead[i] = BlueBead;
            ArrPurpleBead[i] = PurpleBead;
            
        }
        
        for (int i = 0; i < ArrBlueBead.Length ; i++)
        {
            ArrBlueBeadName[i] = "BlueBead" + (i+1) ;
            ArrPurpleBeadName[i] = "PurpleBead" + (i+1);

        }
        
        Destroyer = FindObjectOfType<GameObjectDestroyer>();
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            { 
                boardColliders.name = "Spot " + (-5f + i) + "," + (5 - j) + " ";
                Instantiate(boardColliders, new Vector3(-5f + i, 5 - j, 0), Quaternion.identity);
                //This creates colliders for each spot on the board
            }
                
        } 
    }

    // Update is called once per frame
    void Update()
    {
        BeadPlacer();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void BeadPlacer()
    {
        bool CanPlace;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //stores this converted mouse position in a Vector2 variable named mousePosition
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        //hit will contain information about the collider that the raycast hits, such as the collider itself 

        if (hit.collider != null) // Check if the ray hits a collider
        {
           
            ExtractCoordinates(hit.collider.name, out int Xcord, out int Ycord);
            Debug.Log(hit.collider.name);
         
            Debug.Log("X: " +  Xcord);
            Debug.Log("Y: " +  Ycord);
            if (Xcord == -5 || Xcord == 5 || Ycord == -5 || Ycord == 5)
            {
                CanPlace = true;
            }
            else
            {
                CanPlace = false;
            }
            

            if (CanPlace)
            {
                if (!(Input.GetMouseButtonDown(0))) // Change this condition if you want to instantiate on hover instead of click
                {
                    Vector3 position = new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.center.y, -1); //stores the position where the beads will be placed
                    if (IsPlayerBlueTurn && Ishovering == false)
                    {
                        BeadNamer(IsPlayerBlueTurn);
                        Instantiate(ArrBlueBead[_TrackerScript.BlueScore - 1], position,
                            Quaternion.identity); //creates blue beads if blues turn
                        Instantiate(CircleHighlight,
                            new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.center.y, -2),
                            Quaternion.identity);
                        Ishovering = true;
                    }
                    else if (IsPlayerBlueTurn == false && Ishovering == false)
                    {
                        BeadNamer(IsPlayerBlueTurn);
                        Instantiate(ArrPurpleBead[_TrackerScript.PurpleScore - 1], position,
                            Quaternion.identity); //creates purple beads if blues turn
                        Instantiate(CircleHighlight,
                            new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.center.y, -2),
                            Quaternion.identity);
                        Ishovering = true;
                    }
                }
                
                if ((Input.GetMouseButtonDown(0))) // Change this condition if you want to instantiate on hover instead of click
                {
                    Vector3 position =
                        new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.center.y,
                            -1); //stores the position where the beads will be placed
           
                    if (IsPlayerBlueTurn && Ishovering)
                    {
                        GameObjectDestroyer.CircleHighlightDestroyer();
                        BeadNamer(IsPlayerBlueTurn);
                        // Instantiate(ArrBlueBead[_TrackerScript.BlueScore-1], position, Quaternion.identity);//creates blue beads if blues turn
                        _TrackerScript.Decrement(TrackerScript.Score.BlueScore);
                
                        Collider2D[] hitColliders = Physics2D.OverlapPointAll(position);
                
                        if (hitColliders != null && hitColliders.Length >= 3 && Xcord == -5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.right; // Move 1 unit to the right
                            objectTransform.position = newPosition;
                            _TrackerScript.LastDirection = Vector3.right;
                            DirectionBeadPlaced = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && Ycord == -5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.up; // Move 1 unit to the up
                            objectTransform.position = newPosition;
                            _TrackerScript.LastDirection = Vector3.up;
                            DirectionBeadPlaced = true; 
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && Xcord == 5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.left; // Move 1 unit to the left
                            objectTransform.position = newPosition;
                            _TrackerScript.LastDirection = Vector3.left;
                            DirectionBeadPlaced = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && Ycord == 5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.down; // Move 1 unit to the down
                            objectTransform.position = newPosition;
                            _TrackerScript.LastDirection = Vector3.down;
                            DirectionBeadPlaced = true;
                        }
                
                        Ishovering = false;
                        IsPlayerBlueTurn = false;
                    }
                    else if (IsPlayerBlueTurn == false && Ishovering)
                    {
                        GameObjectDestroyer.CircleHighlightDestroyer();
                        BeadNamer(IsPlayerBlueTurn);
                        // Instantiate(ArrPurpleBead[_TrackerScript.PurpleScore-1], position, Quaternion.identity); //creates purple beads if blues turn
                        _TrackerScript.Decrement(TrackerScript.Score.PurpleScore);
                
                        Collider2D[] hitColliders = Physics2D.OverlapPointAll(position);
                
                        if (hitColliders != null && hitColliders.Length >= 3 && Xcord == -5 )
                        {
                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.right; // Move 1 unit to the right
                            objectTransform.position = newPosition;
                            _TrackerScript.LastDirection = Vector3.right;
                            DirectionBeadPlaced = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && Ycord == -5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.up; // Move 1 unit to the up
                            objectTransform.position = newPosition;
                            _TrackerScript.LastDirection = Vector3.up;
                            DirectionBeadPlaced = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && Xcord == 5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.left; // Move 1 unit to the left
                            objectTransform.position = newPosition;
                            _TrackerScript.LastDirection = Vector3.left;
                            DirectionBeadPlaced = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && Ycord == 5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.down; // Move 1 unit to the down
                            objectTransform.position = newPosition;
                            _TrackerScript.LastDirection = Vector3.down;
                            DirectionBeadPlaced = true;
                        }
                        Ishovering = false;
                        IsPlayerBlueTurn = true;
                    }
                }
            }
        }
        
        
        
        if (hit.collider == null )
        {
            if (Ishovering)
            {
                GameObjectDestroyer.BlueBeadDestroyer(_TrackerScript.BlueScore);
                GameObjectDestroyer.PurpleBeadDestroyer(_TrackerScript.PurpleScore);
                GameObjectDestroyer.CircleHighlightDestroyer();
                Ishovering = false;
            }
        }
    }

    public int Ycord { get; set; }

    public int Xcord { get; set; }

    public void BeadNamer(bool IsplayerBlue)
    {
        if (IsplayerBlue)
        {
            ArrBlueBead[_TrackerScript.BlueScore - 1].gameObject.name = ArrBlueBeadName[_TrackerScript.BlueScore - 1];
        }
        else
        {
            ArrPurpleBead[_TrackerScript.PurpleScore - 1].gameObject.name = ArrPurpleBeadName[_TrackerScript.PurpleScore - 1];
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
