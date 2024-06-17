using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManagerScript : MonoBehaviour
{
    public class Bead
    {
        public int X;
        public int Y;
        
        public Bead(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public List<Bead> BlueBeads = new List<Bead>();
    public List<Bead> PurpleBeads = new List<Bead>();
    
    public int Pi = 0;
    public int Bi = 0;
    
    public struct BlueThreeInARowDircection
    {
        public bool Horizontal, Vertical , NegativeDiagonal , PostiveDiagonal;

        public BlueThreeInARowDircection(bool horizontal, bool vertical, bool negativeDiagonal, bool postiveDiagonal)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            NegativeDiagonal = negativeDiagonal;
            PostiveDiagonal = postiveDiagonal;
        }
    }
   
    
    public struct PurpleThreeInARowDircection
    {
        public bool Horizontal, Vertical , NegativeDiagonal , PostiveDiagonal;

        public PurpleThreeInARowDircection(bool horizontal, bool vertical, bool negativeDiagonal, bool postiveDiagonal)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            NegativeDiagonal = negativeDiagonal;
            PostiveDiagonal = postiveDiagonal;
        }
    }


     public  BlueThreeInARowDircection BlueDirection = new (false, false, false, false);
     public  PurpleThreeInARowDircection PurpleDirection = new(false, false, false, false);
    
     public List<int> bluexCordList = new List<int>();
     public List<int> blueyCordList = new List<int>();
     public List<Bead> BluePositiveDiagonalList = new List<Bead>();
     public  List<Bead> BlueNegativeDiagonalList = new List<Bead>();
     
     public List<int> purplexCordList = new List<int>();
     public List<int> purpleyCordList = new List<int>();
     public List<Bead> PurplePositiveDiagonalList = new List<Bead>();
     public  List<Bead> PurpleNegativeDiagonalList = new List<Bead>();

         public struct TopLeftSpots
         {
             public bool Spot1, Spot2, Spot3, Spot4 ,IsAllBlue ,IsAllPurple ;
             
             public TopLeftSpots(bool spot1, bool spot2, bool spot3, bool spot4 , bool isAllBlue , bool isAllPurple )
             {
                 Spot1 = spot1;
                 Spot2 = spot2;
                 Spot3 = spot3;
                 Spot4 = spot4;
                 IsAllBlue = isAllBlue;
                 IsAllPurple = isAllPurple;
             }
             
             public bool AllTrue()
             {
                 return Spot1 && Spot2 && Spot3 && Spot4;
             }
             
         }

         public struct BottomLeftSpots
         {
             public bool Spot1, Spot2, Spot3, Spot4 ,IsAllBlue ,IsAllPurple ;

             public BottomLeftSpots(bool spot1, bool spot2, bool spot3, bool spot4 , bool isAllBlue , bool isAllPurple )
             {
                 Spot1 = spot1;
                 Spot2 = spot2;
                 Spot3 = spot3;
                 Spot4 = spot4;
                 IsAllBlue = isAllBlue;
                 IsAllPurple = isAllPurple;
             }
             
             public bool AllTrue()
             {
                 return Spot1 && Spot2 && Spot3 && Spot4;
             }
         }

         public struct TopRightSpots
         {
             public bool Spot1, Spot2, Spot3, Spot4 ,IsAllBlue ,IsAllPurple ;
             
             public TopRightSpots(bool spot1, bool spot2, bool spot3, bool spot4 , bool isAllBlue , bool isAllPurple )
             {
                 Spot1 = spot1;
                 Spot2 = spot2;
                 Spot3 = spot3;
                 Spot4 = spot4;
                 IsAllBlue = isAllBlue;
                 IsAllPurple = isAllPurple;
             }
             
             public bool AllTrue()
             {
                 return Spot1 && Spot2 && Spot3 && Spot4;
             }
         }

         public struct BottomRightSpots
         {
             public bool Spot1, Spot2, Spot3, Spot4 ,IsAllBlue ,IsAllPurple ;
     
             public BottomRightSpots(bool spot1, bool spot2, bool spot3, bool spot4 , bool isAllBlue , bool isAllPurple )
             {
                 Spot1 = spot1;
                 Spot2 = spot2;
                 Spot3 = spot3;
                 Spot4 = spot4;
                 IsAllBlue = isAllBlue;
                 IsAllPurple = isAllPurple;
             }
             public bool AllTrue()
             {
                 return Spot1 && Spot2 && Spot3 && Spot4;
             }
         }

         public TopLeftSpots topLeftSpots = new TopLeftSpots(false, false, false, false, false, false);
         public TopRightSpots topRightSpots = new TopRightSpots(false, false, false, false, false, false);
         public BottomLeftSpots bottomLeftSpots = new BottomLeftSpots(false, false, false, false, false, false);
         public BottomRightSpots bottomRightSpots = new BottomRightSpots(false, false, false, false, false, false);

         public GameObject[] topLeftObjects, topRightObjects, bottomLeftObjects, bottomRightObjects;

         void Awake()
         {
             // Initialize the arrays with size 4
             topLeftObjects = new GameObject[4];
             topRightObjects = new GameObject[4];
             bottomLeftObjects = new GameObject[4];
             bottomRightObjects = new GameObject[4];
         }

}
