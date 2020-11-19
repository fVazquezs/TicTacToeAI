using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class WinnerTextController : MonoBehaviour
{
    private void Start()
    {
        TextMeshProUGUI textWinner = gameObject.GetComponent<TextMeshProUGUI>();
        
        switch(GameController.Winner)
        {
            case Symbol.Cross:
                textWinner.SetText("Last game winner was cross!");
                break;
            case Symbol.Circle:
                textWinner.SetText("Last game winner was Circle!");
                break;
            case Symbol.None:
                textWinner.SetText("Last game was a draw!");
                break;
            default:
                textWinner.SetText("");
                break;
        }
    }
}