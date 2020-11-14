using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   private void Awake()
   {
      Debug.Log(GameController.Winner);
   }

   public void PlayEasyMode()
   {
      GameController.GameMode = GameMode.Easy;
      LoadScene();
   }
   
   public void PlayMediumMode()
   {
      GameController.GameMode = GameMode.Medium;
      LoadScene();
   }
   
   public void PlayHardMode()
   {
      GameController.GameMode = GameMode.Hard;
      LoadScene();
   }

   public void PlayPVP()
   {
      GameController.GameMode = GameMode.Pvp;
      LoadScene();
   }


   private void LoadScene()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
   }
}
