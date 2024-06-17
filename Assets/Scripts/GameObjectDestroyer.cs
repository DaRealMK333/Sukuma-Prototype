using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDestroyer : MonoBehaviour
{
    // ReSharper disable Unity.PerformanceAnalysis
    public static void BlueBeadDestroyer(int beadNumber)
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Iterate through all objects and find those whose names start with "bluebead"
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("BlueBead" + (beadNumber).ToString() + "(Clone)"))
            {
               Destroy(obj);
            }
        }
        
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static void PurpleBeadDestroyer(int beadNumber )
    { 
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Iterate through all objects and find those whose names start with "bluebead"
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("PurpleBead" + (beadNumber).ToString() + "(Clone)"))
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
