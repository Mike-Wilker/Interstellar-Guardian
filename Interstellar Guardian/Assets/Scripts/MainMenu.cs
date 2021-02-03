using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void showCredits()
    {
        Instantiate(Resources.Load<Transform>(@"Menus\Credits"), transform);
    }
    public void exit()
    {
        Application.Quit();
    }
}
