using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GridManager gridManager;
    private GameManager gameManager;
    public Spawner spawner;

    // Current position of the player in grid indices
    public int currentRow;
    public int currentColumn;

    void Awake()
    {
        gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        // Check for arrow key inputs
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(1, 0); // Move up (increase row index)
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(-1, 0); // Move down (decrease row index)
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(0, -1); // Move left (decrease column index)
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(0, 1); // Move right (increase column index)
        }
    }

    private void Move(int deltaX, int deltaY)
    {
        // Calculate new indices
        int newRow = currentRow + deltaX;
        int newColumn = currentColumn + deltaY;

        // Check if new indices are within bounds
        if (newRow >= 0 && newRow < gridManager.levelDesign.rows && newColumn >= 0 && newColumn < gridManager.levelDesign.columns)
        {

            // Update the player's position and indices
            currentRow = newRow;
            currentColumn = newColumn;

            GameObject targetTile = gridManager.GetGridElement(currentColumn, currentRow).gameObject;

            // Set the player's parent to the new tile and update its position
            transform.SetParent(targetTile.transform);
            transform.localPosition = Vector3.zero; // Center the player on the tile
            transform.position += new Vector3(0f, 0.5f, 0f);

            TileBase tile = targetTile.GetComponent<TileBase>();
            if (tile != null)
            {
                tile.OnCharacterEnter(gameObject);
            }


            gameManager.ChangeTurnTotal(1);


        }
        else
        {
            Debug.Log("Can't move outside bounds!");
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        gameManager.GameOver();
    }
}
