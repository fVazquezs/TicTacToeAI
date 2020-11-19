using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpot : MonoBehaviour
{

    public int line;
    public int column;
    
    public Spot spot;

    public GameObject CrossObjectRoot;
    public GameObject CircleObjectRoot;

    private Symbol _currentSymbol;
    public bool ScriptStarted { get; set; }

    public Symbol CurrentSymbol
    {
        set
        {
            _currentSymbol = value;
            CrossObjectRoot.SetActive(_currentSymbol == Symbol.Cross);
            CircleObjectRoot.SetActive(_currentSymbol == Symbol.Circle);

        }
        get { return _currentSymbol; }
    }

    private void Awake()
    {
        spot = new Spot(line, column);
        ScriptStarted = true;
    }

    private void Start()
    {
        CurrentSymbol = Symbol.None;
    }
    private void OnMouseDown()
    {
        GetComponentInParent<BoardController>().SpotClicked(this);
    }
}
