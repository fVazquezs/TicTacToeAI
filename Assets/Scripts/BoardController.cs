using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BoardController : MonoBehaviour
{
    public int BoardSize = 3;
    public Symbol StartingSymbol = Symbol.Circle;
    public List<Symbol> AiPlayers;

    private Symbol[,] _boardData;
    private Spot[,] _spots;
    private Symbol _currentPlayer;
    public AudioSource _makePlayAudio;
    private bool _AIIsPlaying = false;

    private void Awake()
    {
        GameController.Winner = Symbol.None;

        Debug.Log(GameController.GameMode);
        _boardData = new Symbol[BoardSize, BoardSize];
        _spots = new Spot[BoardSize, BoardSize];

        Spot[] allSpots = GetComponentsInChildren<Spot>();
        foreach (Spot spot in allSpots)
        {
            _spots[spot.line, spot.column] = spot;
        }

        _currentPlayer = StartingSymbol;
        _makePlayAudio = GetComponent<AudioSource>();
    }

    public void SpotClicked(Spot spot)
    {
        if (!_AIIsPlaying)
        {
            MakePlay(spot.line, spot.column);
        }
    }

    public void SetSymbolAt(int line, int column, Symbol symbol)
    {
        _boardData[line, column] = symbol;
        _spots[line, column].CurrentSymbol = symbol;
    }

    public Symbol GetSymbolAt(int line, int column)
    {
        return _boardData[line, column];
    }

    public void MakePlay(int line, int column)
    {
        if (GetSymbolAt(line, column) == Symbol.None)
        {
            SetSymbolAt(line, column, _currentPlayer);
            _makePlayAudio.Play();
            _currentPlayer = _currentPlayer.GetOther();
            Symbol winner = GetWinner();
            if (winner == Symbol.Cross || winner == Symbol.Circle)
            {
                GameController.Winner = winner;
                StartCoroutine(ChangeSceneCoroutine());
            }

            if (winner == Symbol.None)
            {
                if (GameController.GameMode != GameMode.Pvp && AiPlayers.Contains(_currentPlayer))
                {
                    if (MinMax.DoMinMax(this, _currentPlayer, -200, 200, out var bestPlay))
                    {
                        StartCoroutine(AICoroutine(bestPlay.Line, bestPlay.Column));
                    }
                    else
                    {
                        GameController.Winner = winner;
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                }
            }
        }
    }

    public bool IsCalcCorrect()
    {
        float random = Random.Range(0, 1000);
        switch (GameController.GameMode)
        {
            case GameMode.Easy:
                return random > 500;
            case GameMode.Medium:
                return random > 750;
            case GameMode.Hard:
                return true;
        }

        return true;
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

            if (_boardData[index, BoardSize - index - 1] == _boardData[0, BoardSize - 1] &&
                _boardData[0, BoardSize - 1] != Symbol.None)
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
    
    //Coroutine to make AI fake a human think time
    IEnumerator AICoroutine(int line, int column)
    {
        _AIIsPlaying = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

        _AIIsPlaying = false;
        MakePlay(line, column);
    }

    IEnumerator ChangeSceneCoroutine()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}