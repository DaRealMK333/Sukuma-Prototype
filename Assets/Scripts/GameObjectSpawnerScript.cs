using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameObjectSpawnerScript : MonoBehaviour
{
    public GameObject boardColliders;
    [FormerlySerializedAs("BlueBead")] public GameObject blueBead;
    [FormerlySerializedAs("PurpleBead")] public GameObject purpleBead;
    [FormerlySerializedAs("ArrBlueBead")] public GameObject[] arrBlueBead = new GameObject[26];
    [FormerlySerializedAs("ArrPurpleBead")] public GameObject[] arrPurpleBead = new GameObject[26];
    [FormerlySerializedAs("CircleHighlight")] public GameObject circleHighlight;

    [FormerlySerializedAs("ArrBlueBeadName")] public string[] arrBlueBeadName = new string[26];
    [FormerlySerializedAs("ArrPurpleBeadName")] public string[] arrPurpleBeadName = new string[26];

    private bool _isPlayerBlueTurn = true;
    private bool _ishovering;
    
    [FormerlySerializedAs("Destroyer")] public GameObjectDestroyer destroyer;
    [FormerlySerializedAs("_TrackerScript")] public TrackerScript trackerScript;

    private DataManagerScript _dataManagerScript;
    
    public GameObjectSpawnerScript instance;
    public bool threeRow = false;
    // Start is called before the first frame update
    void Start()
    {
        trackerScript = FindObjectOfType<TrackerScript>();
        _dataManagerScript = FindObjectOfType<DataManagerScript>();
        
        for (int i = 0; i < arrBlueBead.Length ; i++)
        {
            arrBlueBead[i] = blueBead;
            arrPurpleBead[i] = purpleBead;
            
        }
        
        for (int i = 0; i < arrBlueBead.Length ; i++)
        {
            arrBlueBeadName[i] = "BlueBead" + (i+1) ;
            arrPurpleBeadName[i] = "PurpleBead" + (i+1);

        }
        
        destroyer = FindObjectOfType<GameObjectDestroyer>();
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
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnBead(GameObject beadPrefab, Vector3 position)
    {
        if (!threeRow)
        {
            Instantiate(beadPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Cannot place bead due to three in a row restriction.");
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void BeadPlacer()
    {
        bool canPlace;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //stores this converted mouse position in a Vector2 variable named mousePosition
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        //hit will contain information about the collider that the raycast hits, such as the collider itself 

        if (hit.collider != null) // Check if the ray hits a collider
        {
           
            ExtractCoordinates(hit.collider.name, out int xcord, out int ycord);
            
            if (xcord == -5 || xcord == 5 || ycord == -5 || ycord == 5)
            {
                canPlace = true;
            }
            else
            {
                canPlace = false;
            }
            
                            //Debug.Log(ycord);
                if (_dataManagerScript.BlueDirection.Horizontal)
                {
                    if (_dataManagerScript.blueyCordList.Contains(ycord) && !_isPlayerBlueTurn)
                    {
                       // canPlace = false;
                    }
                }
                 if (_dataManagerScript.BlueDirection.Vertical)
                {
                    if (_dataManagerScript.bluexCordList.Contains(xcord) && !_isPlayerBlueTurn)
                    {
                      // canPlace = false;
                    }
                }
        
                if ( _dataManagerScript.PurpleDirection.Horizontal ) 
                {
                    if (_dataManagerScript.purpleyCordList.Contains(ycord)  && _isPlayerBlueTurn)
                    {
                      // canPlace = false;
                    }
                }
                
                if (_dataManagerScript.PurpleDirection.Vertical)
                {
                    if (_dataManagerScript.purplexCordList.Contains(xcord)  && _isPlayerBlueTurn)
                    {
                      // canPlace = false;
                    }
                }

                if (_dataManagerScript.topLeftSpots.IsAllBlue || _dataManagerScript.topLeftSpots.IsAllPurple)
                {
                    if (xcord >= -5 && xcord <= 0 && ycord >= 0 && ycord <= 5)
                    {
                        canPlace = false;
                    }
                }
                if (_dataManagerScript.topRightSpots.IsAllBlue || _dataManagerScript.topRightSpots.IsAllPurple)
                {
                    if (xcord >= 0 && xcord <= 5 && ycord >= 0 && ycord <= 5)
                    {
                        canPlace = false;
                    }
                }
                if (_dataManagerScript.bottomLeftSpots.IsAllBlue || _dataManagerScript.bottomLeftSpots.IsAllPurple)
                {
                    if (xcord >= -5 && xcord <= 0 && ycord >= -5 && ycord <= 0)
                    {
                        canPlace = false;
                    }
                }
                if (_dataManagerScript.bottomRightSpots.IsAllBlue || _dataManagerScript.bottomRightSpots.IsAllPurple)
                {
                    if (xcord >= 0 && xcord <= 5 && ycord >= -5 && ycord <= 0)
                    {
                        canPlace = false;
                    }
                }
                

            if (canPlace)
            {
                if (!(Input.GetMouseButtonDown(0))) // Change this condition if you want to instantiate on hover instead of click
                {
                    Vector3 position = new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.center.y, -1); //stores the position where the beads will be placed
                    if (_isPlayerBlueTurn && _ishovering == false)
                    {
                        BeadNamer(_isPlayerBlueTurn);
                        Instantiate(arrBlueBead[trackerScript.BlueScore - 1], position,
                            Quaternion.identity); //creates blue beads if blues turn
                        Instantiate(circleHighlight,
                            new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.center.y, -2),
                            Quaternion.identity);
                        _ishovering = true;
                    }
                    else if (_isPlayerBlueTurn == false && _ishovering == false)
                    {
                        BeadNamer(_isPlayerBlueTurn);
                        Instantiate(arrPurpleBead[trackerScript.PurpleScore - 1], position,
                            Quaternion.identity); //creates purple beads if blues turn
                        Instantiate(circleHighlight,
                            new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.center.y, -2),
                            Quaternion.identity);
                        _ishovering = true;
                    }
                }

                

                if ((Input.GetMouseButtonDown(0))) // Change this condition if you want to instantiate on hover instead of click
                {
                    Vector3 position =
                        new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.center.y,
                            -1); //stores the position where the beads will be placed
           
                    if (_isPlayerBlueTurn && _ishovering)
                    {
                        GameObjectDestroyer.CircleHighlightDestroyer();
                        BeadNamer(_isPlayerBlueTurn);
                        // Instantiate(ArrBlueBead[_TrackerScript.BlueScore-1], position, Quaternion.identity);//creates blue beads if blues turn
                        trackerScript.Decrement(TrackerScript.Score.BlueScore);
                
                        Collider2D[] hitColliders = Physics2D.OverlapPointAll(position);
                
                        if (hitColliders != null && hitColliders.Length >= 3 && xcord == -5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.right; // Move 1 unit to the right
                            objectTransform.position = newPosition;
                            trackerScript.up = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && ycord == -5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.up; // Move 1 unit to the up
                            objectTransform.position = newPosition;
                            trackerScript.down = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && xcord == 5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.left; // Move 1 unit to the left
                            objectTransform.position = newPosition;
                            trackerScript.left = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && ycord == 5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.down; // Move 1 unit to the down
                            objectTransform.position = newPosition;
                            trackerScript.right = true;
                        }
                
                        _ishovering = false;
                        _isPlayerBlueTurn = false;
                    }
                    else if (_isPlayerBlueTurn == false && _ishovering)
                    {
                        GameObjectDestroyer.CircleHighlightDestroyer();
                        BeadNamer(_isPlayerBlueTurn);
                        // Instantiate(ArrPurpleBead[_TrackerScript.PurpleScore-1], position, Quaternion.identity); //creates purple beads if blues turn
                        trackerScript.Decrement(TrackerScript.Score.PurpleScore);
                
                        Collider2D[] hitColliders = Physics2D.OverlapPointAll(position);
                
                        if (hitColliders != null && hitColliders.Length >= 3 && xcord == -5 )
                        {
                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.right; // Move 1 unit to the right
                            objectTransform.position = newPosition;
                            trackerScript.up = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && ycord == -5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.up; // Move 1 unit to the up
                            objectTransform.position = newPosition;
                            trackerScript.down = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && xcord == 5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.left; // Move 1 unit to the left
                            objectTransform.position = newPosition;
                            trackerScript.left = true;
                        }
                        if (hitColliders != null && hitColliders.Length >= 3 && ycord == 5 )
                        {

                            // Move the object underneath
                            Transform objectTransform = hitColliders[1].transform;
                            Vector3 newPosition = objectTransform.position + Vector3.down; // Move 1 unit to the down
                            objectTransform.position = newPosition;
                            trackerScript.right = true;
                        }
                        _ishovering = false;
                        _isPlayerBlueTurn = true;
                    }
                }
            }
        }
        
        if (hit.collider == null )
        {
            if (_ishovering)
            {
                GameObjectDestroyer.BlueBeadDestroyer(trackerScript.BlueScore);
                GameObjectDestroyer.PurpleBeadDestroyer(trackerScript.PurpleScore);
                GameObjectDestroyer.CircleHighlightDestroyer();
                _ishovering = false;
            }
        }
    }

    private void BeadNamer(bool isplayerBlue)
    {
        if (isplayerBlue)
        {
            arrBlueBead[trackerScript.BlueScore - 1].gameObject.name = arrBlueBeadName[trackerScript.BlueScore - 1];
        }
        else
        {
            arrPurpleBead[trackerScript.PurpleScore - 1].gameObject.name = arrPurpleBeadName[trackerScript.PurpleScore - 1];
        }
    }


    private static void ExtractCoordinates(string input, out int x, out int y)
    {
        // Split the string by space and comma
        string[] parts = input.Split(' ', ',');

        // Initialize variables out of range
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

public class GameObjectSpawner
{
}
