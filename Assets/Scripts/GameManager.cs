using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    void Start()
    {
        GameIsOver = false;
    }
    void Update()
    {
        if (GameIsOver)
            return;

        if (Input.GetKeyDown("e"))
        {            
            EndGame();
        }


        if (PlayerHealth.currentHealth <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        Cursor.lockState = CursorLockMode.None;      
        GameIsOver = true;
        gameWinUI.SetActive(true);
    }
}
