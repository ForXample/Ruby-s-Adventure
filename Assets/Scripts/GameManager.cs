using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager uiManager;
    public int turnTotal = 0;

    void Awake()
    {
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
    }

    public void GameOver()
    {
        uiManager.OpenGameOverMenu();
        Debug.Log("Game over!");
    }

    public void ChangeTurnTotal(int amt)
    {
        turnTotal += amt;
        uiManager.UpdateTurnTotalDisplay(turnTotal);
        EventManager.Instance.TriggerTurnTotalChanged(turnTotal); // Trigger event
    }
}
