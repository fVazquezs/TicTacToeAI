public static class MinMax
{
    public struct Play
    {
        public int Line, Column;
        public int Score;
    }

    public static bool DoMinMax(BoardController board, Symbol player, out Play bestPlay)
    {
        Play possiblePlay = new Play();
        possiblePlay.Column = 0;
        possiblePlay.Line = 0;
        possiblePlay.Score = 200;
        bestPlay = possiblePlay;
        return true;
    }
}