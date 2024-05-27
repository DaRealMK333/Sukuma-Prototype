using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void BlueBeadDestroyer(int BeadNumber)
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Iterate through all objects and find those whose names start with "bluebead"
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("BlueBead" + (BeadNumber).ToString() + "(Clone)"))
            {
               Destroy(obj);
            }
        }
        
        
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static void PurpleBeadDestroyer(int BeadNumber )
    { 
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Iterate through all objects and find those whose names start with "bluebead"
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("PurpleBead" + (BeadNumber).ToString() + "(Clone)"))
            {
                Destroy(obj);
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void CircleHighlightDestroyer()
    {
        Destroy(GameObject.Find("Circle Highlight(Clone)"));
    }
}
