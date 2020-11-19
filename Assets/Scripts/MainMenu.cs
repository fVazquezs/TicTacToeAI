using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
      SceneManager.LoadScene("Game");
   }
}
