using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BoardController : MonoBehaviour
{
    public int BoardSize = 3;
    public Symbol StartingSymbol = Symbol.Circle;
    public List<Symbol> AiPlayers;

    private Symbol[,] _boardData;
    private GameSpot[,] _gameSpots;
    private Symbol _currentPlayer;
    private AudioSource _makePlayAudio;
    private bool _AIIsPlaying = false;

    private void Awake()
    {
        GameController.Winner = Symbol.None;

        _boardData = new Symbol[BoardSize, BoardSize];
        _gameSpots = new GameSpot[BoardSize, BoardSize];

        GameSpot[] allGameSpots = GetComponentsInChildren<GameSpot>();
        foreach (GameSpot gameSpot in allGameSpots)
        {
            StartCoroutine(WaitUntilSpotGetStartedAndPopulate(gameSpot));
        }

        _currentPlayer = StartingSymbol;
        _makePlayAudio = GetComponent<AudioSource>();
    }

    public void SpotClicked(GameSpot gameSpot)
    {
        if (!_AIIsPlaying)
        {
            MakePlay(gameSpot.spot.Line, gameSpot.spot.Column);
        }
    }

    public void SetSymbolAt(int line, int column, Symbol symbol)
    {
        _boardData[line, column] = symbol;
        _gameSpots[line, column].CurrentSymbol = symbol;
    }

    public Symbol GetSymbolAt(int line, int column)
    {
        return _boardData[line, column];
    }

    private void MakePlay(int line, int column)
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
                    if (AiMakeRandomPlay(out Spot play))
                    {
                        StartCoroutine(AICoroutine(play.Line, play.Column));
                    }
                    else if (MinMax.DoMinMax(this, _currentPlayer, -200, 200, 0, out var bestPlay))
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

    public List<Spot> GetBoardEmptySpots()
    {
        List<Spot> freeSpots = new List<Spot>();
        for (int column = 0; column < BoardSize; column++)
        {
            for (int line = 0; line < BoardSize; line++)
            {
                if (GetSymbolAt(line, column) == Symbol.None)
                {
                    freeSpots.Add(new Spot(line, column));
                }
            }
        }

        return freeSpots;
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

    private bool AiMakeRandomPlay(out Spot play)
    {
        play = null;
        List<Spot> freeSpots = GetBoardEmptySpots();
        if (freeSpots.Count > 0 &&
            (GameController.GameMode == GameMode.Easy ||
             (GameController.GameMode == GameMode.Medium && Random.value < 0.65f)))
        {
            play = freeSpots[Random.Range(0, freeSpots.Count)];
            return true;
        }

        return false;
    }

    //Coroutine to make AI fake a human think time
    IEnumerator AICoroutine(int line, int column)
    {
        _AIIsPlaying = true;
        yield return new WaitForSeconds(Random.Range(0.3f, 1.2f));

        _AIIsPlaying = false;
        MakePlay(line, column);
    }

    IEnumerator ChangeSceneCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("GameOver");
    }

    IEnumerator WaitUntilSpotGetStartedAndPopulate(GameSpot gameSpot)
    {
        yield return gameSpot.ScriptStarted;
        
        _gameSpots[gameSpot.spot.Line, gameSpot.spot.Column] = gameSpot;
    }
    
}