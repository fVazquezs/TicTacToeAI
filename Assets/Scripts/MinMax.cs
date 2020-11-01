using UnityEngine;

public static class MinMax
{
    public struct Play
    {
        public int Line, Column;
        public int Score;
    }

    public static bool DoMinMax(BoardController board, Symbol player, out Play bestPlay)
    {
        bool isMin = (player == Symbol.Circle);
        bestPlay.Line = bestPlay.Column = -1;
        bestPlay.Score = (isMin ? 200 : -200);

        Symbol winner = board.GetWinner();
        if (winner != Symbol.None)
        {
            switch (winner)
            {
                case Symbol.Circle:
                    bestPlay.Score = -100;
                    break;
                case Symbol.Cross:
                    bestPlay.Score = 100;
                    break;
            }

            return false;
        }

        bool possiblePlayExists = false;
        
        //try play for min
        if(isMin)
        {
            for (int column = 0; column < board.BoardSize; column++)
            {
                for (int line = 0; line < board.BoardSize; line++)
                {
                    if (board.GetSymbolAt(line, column) != Symbol.None)
                    {
                        continue;
                    }

                    possiblePlayExists = true;
                    board.SetSymbolAt(line, column, player);
                    DoMinMax(board, player.GetOther(), out Play roundPlay);
                    if (roundPlay.Score < bestPlay.Score)
                    {
                        bestPlay.Score = roundPlay.Score;
                        bestPlay.Line = line;
                        bestPlay.Column = column;
                    }
                    board.SetSymbolAt(line, column, Symbol.None);
                }
            }
        }
        //try play for max
        else
        {
            for (int column = 0; column < board.BoardSize; column++)
            {
                for (int line = 0; line < board.BoardSize; line++)
                {
                    if (board.GetSymbolAt(line, column) != Symbol.None)
                    {
                        continue;
                    } 
                    possiblePlayExists = true;
                    board.SetSymbolAt(line, column, player);
                    DoMinMax(board, player.GetOther(), out Play roundPlay);
                    if (roundPlay.Score > bestPlay.Score)
                    {
                        bestPlay.Score = roundPlay.Score;
                        bestPlay.Line = line;
                        bestPlay.Column = column;
                    }
                    board.SetSymbolAt(line, column, Symbol.None);
                }
            }
        }

        if (!possiblePlayExists)
        {
            bestPlay.Score = 0;
            Debug.Log("No possible play");
        }
        
        return possiblePlayExists;
    }
}