using UnityEngine;

public enum Symbol
{
    None,
    Cross,
    Circle
}

public static class SymbolExtensions
{
    public static Symbol GetOther(this Symbol symbol)
    {
        switch (symbol)
        {
            case Symbol.Circle: return Symbol.Cross;
            case Symbol.Cross: return Symbol.Circle;
            default: return Symbol.None;
        }
    }
}