using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MinMax
{
    public struct Play
    {
        public int Line, Column;
        public int Score;
    }

    public static bool DoMinMax(BoardController board, Symbol player, int alpha, int beta, int depth, out Play bestPlay)
    {
        bool isMin = (player == Symbol.Circle);
        bestPlay.Line = bestPlay.Column = -1;
        bestPlay.Score = (isMin ? 200 : -200);

        Symbol winner = board.GetWinner();
        if (winner != Symbol.None)
        {
            bestPlay.Score = winner == Symbol.Circle ? -100 : 100;
            return false;
        }

        bool possiblePlayExists = false;

        if (!OpenBoard(board, player, alpha, beta, depth, ref bestPlay, isMin, ref possiblePlayExists)) return false;

        if (!possiblePlayExists)
        {
            bestPlay.Score = 0;
        }

        return possiblePlayExists;
    }

    private static bool OpenBoard(BoardController board, Symbol player, int alpha, int beta, int depth,
        ref Play bestPlay,
        bool isMin, ref bool possiblePlayExists)
    {
        foreach (Spot spot in board.GetBoardEmptySpots())
        {
            possiblePlayExists = true;
            board.SetSymbolAt(spot.Line, spot.Column, player);
            DoMinMax(board, player.GetOther(), alpha, beta, depth + 1, out Play roundPlay);
            if ((isMin && roundPlay.Score < bestPlay.Score) || (!isMin && roundPlay.Score > bestPlay.Score))
            {
                bestPlay.Score = roundPlay.Score;
                bestPlay.Line = spot.Line;
                bestPlay.Column = spot.Column;
            }

            if (isMin)
            {
                beta = Math.Min(beta, bestPlay.Score);
            }
            else
            {
                alpha = Math.Max(alpha, bestPlay.Score);
            }

            board.SetSymbolAt(spot.Line, spot.Column, Symbol.None);
            if (beta <= alpha)
            {
                return false;
            }
        }

        return true;
    }
}