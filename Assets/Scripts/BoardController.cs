using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
   public void SpotClicked(Spot spot)
   {
       Debug.Log($"Line {spot.line} Column {spot.column}");

       switch(spot.CurrentSymbol)
       {
           case Symbol.None:
                spot.CurrentSymbol = Symbol.Cross;
                break;
           case Symbol.Cross:
                spot.CurrentSymbol = Symbol.Circle;
                break;
           case Symbol.Circle:
                spot.CurrentSymbol = Symbol.None;
                break;
       }
   }
}
