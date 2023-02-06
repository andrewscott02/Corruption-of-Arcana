using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Authored & Written by <Jack Drage>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class MobileSplashScreen : MonoBehaviour
    {
        public E_Scenes initialScene;

        public void Play()
        {
            //Reset loadsettings/progress
            SceneManager.LoadScene(initialScene.ToString());
            //load game
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}