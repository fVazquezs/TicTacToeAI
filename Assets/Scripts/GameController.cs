using UnityEngine;


public enum GameMode
{
    Hard,
    Medium,
    Easy,
    Pvp
}
public class GameController : MonoBehaviour
{
    public static GameMode GameMode { set; get; }
    public static Symbol Winner { set; get; }
}
