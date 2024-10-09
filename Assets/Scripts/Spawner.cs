using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    public GridManager gridManager;
    public int startingRow;
    public int startingCol;

    void Awake()
    {
        startingRow = gridManager.levelDesign.startingRow;
        startingCol = gridManager.levelDesign.startingCol;
        playerController = player.GetComponent<PlayerController>();
        playerController.currentRow = startingRow;
        playerController.currentColumn = startingCol;
    }

    void Start()
    {
        GridElement targetGrid = gridManager.GetGridElement(startingRow, startingCol);
        GameObject newPlayer = Instantiate(player, targetGrid.transform.position, Quaternion.identity);
        newPlayer.transform.position += new Vector3(0f, 0.5f, 0f);
        newPlayer.transform.SetParent(targetGrid.gameObject.transform);
        gridManager.playerController = newPlayer.GetComponent<PlayerController>();
    }
}
