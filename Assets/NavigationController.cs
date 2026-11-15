using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    public void onClickStart()
    {
        PlayerStats.score = 0;
        SceneManager.LoadScene(1); // Level1
    }

    public void onClickQuit()
    {
        Application.Quit();
    }
}