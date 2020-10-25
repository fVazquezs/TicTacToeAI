using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Symbol
{
    None,
    Cross,
    Circle
}

public class Spot : MonoBehaviour
{

    public int line;
    public int column;

    public GameObject CrossObjectRoot;
    public GameObject CircleObjectRoot;

    private Symbol _currentSymbol;

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

    private void Start()
    {
        CurrentSymbol = Symbol.None;
    }
    private void OnMouseDown()
    {
        GetComponentInParent<BoardController>().SpotClicked(this);
    }
}
