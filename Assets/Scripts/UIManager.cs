using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject gameOverMenu;

    public TMP_Text turnTotalText;

    public void OpenGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void UpdateTurnTotalDisplay(int turnTotal)
    {
        turnTotalText.text = turnTotal.ToString();
    }
}
