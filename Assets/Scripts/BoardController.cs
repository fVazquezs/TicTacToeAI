using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    public int BoardSize = 3;
    public Symbol StartingSymbol = Symbol.Circle;
    public List<Symbol> AiPlayers;

    private Symbol[,] _boardData;
    private Spot[,] _spots;
    private Symbol _currentPlayer;

    private void Awake()
    {
        _boardData = new Symbol[BoardSize, BoardSize];
        _spots = new Spot[BoardSize, BoardSize];

        var allSpots = GetComponentsInChildren<Spot>();
        foreach (var spot in allSpots)
        {
            _spots[spot.line, spot.column] = spot;
        }

        _currentPlayer = StartingSymbol;
    }

    public void SpotClicked(Spot spot)
    {
        Debug.Log($"Line {spot.line} Column {spot.column}");

        MakePlay(spot.line, spot.column);
    }

    public void SetSymbolAt(int line, int column, Symbol symbol)
    {
        try
        {
            _boardData[line, column] = symbol;
            _spots[line, column].CurrentSymbol = symbol;
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogWarning($"{line} {column}");
            throw;
        }

    }

    public Symbol GetSymbolAt(int line, int column)
    {
        return _boardData[line, column];
    }

    public void MakePlay(int line, int column)
    {
        SetSymbolAt(line, column, _currentPlayer);
        _currentPlayer = _currentPlayer.GetOther();
        Symbol winner = GetWinner();
        if (winner == Symbol.Cross)
        {
            Debug.Log("Winner is cross");
        }
        else if (winner == Symbol.Circle)
        {
            Debug.Log("Winner is circle");
        }
        if (winner == Symbol.None)
        {
            if (AiPlayers.Contains(_currentPlayer))
            {
                if (MinMax.DoMinMax(this, _currentPlayer, out var bestPlay))
                {
                    MakePlay(bestPlay.Line, bestPlay.Column);
                }
                else
                {
                    Debug.Log("No available play");
                }
            }
        }
    }

    public Symbol GetWinner()
    {
        //horizontal
        for (int line = 0; line < BoardSize; line++)
        {
            if (_boardData[line, 0] == Symbol.None)
            {
                continue;
            }
            int symbolCount = 0;
            for (int c = 0; c < BoardSize; c++)
            {
                if (_boardData[line, c] == _boardData[line, 0])
                {
                    symbolCount++;
                }
            }

            if (symbolCount == BoardSize)
            {
                return _boardData[line, 0];
            }
        }

        //vertical
        for (int column = 0; column < BoardSize; column++)
        {
            if (_boardData[0, column] == Symbol.None)
            {
                continue;
            }
            int symbolCount = 0;
            for (int l = 0; l < BoardSize; l++)
            {
                if (_boardData[l, column] == _boardData[0, column])
                {
                    symbolCount++;
                }
            }

            if (symbolCount == BoardSize)
            {
                return _boardData[0, column];
            }
        }

        //diagonal
        int diagonalCount1 = 0;
        int diagonalCount2 = 0;
        for (int index = 0; index < BoardSize; index++)
        {
            if (_boardData[index, index] == _boardData[0, 0] && _boardData[0, 0] != Symbol.None)
            {
                diagonalCount1++;
            }
            if (_boardData[index, BoardSize - index - 1] == _boardData[0, BoardSize - 1] && _boardData[0, BoardSize - 1] != Symbol.None)
            {
                diagonalCount2++;
            }
        }
        if (diagonalCount1 == BoardSize)
        {
            return _boardData[0, 0];
        }
        else if (diagonalCount2 == BoardSize)
        {
            return _boardData[0, BoardSize - 1];
        }

        return Symbol.None;
    }
}
