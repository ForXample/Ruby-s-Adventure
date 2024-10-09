using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Water : TileBase
{
    private GridManager gridManager;
    private TerrainFormationManager terrainManager;

    public bool isRiver = false;

    void Awake()
    {
        gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
        terrainManager = GameObject.Find("Terrain Manager").GetComponent<TerrainFormationManager>();
    }

    public override void OnCharacterEnter(GameObject character)
    {
        if (character.CompareTag("Player"))
        {
            character.GetComponent<PlayerController>().Die();
            Debug.Log("Player entered Water tile.");
        }
        else
        {
            Debug.Log("Something else entered Water tile.");
        }
    }

    public override void OnTileShifted()
    {
        // Handle tile shift logic if necessary
    }

    /*
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isRiver)
            {
                TileGroup tileGroup = terrainManager.FindRiverAtColumn(GetComponent<GridElement>()._currentColumn);
                if (tileGroup != null)
                {
                    MoveMovableObjects(tileGroup);
                }
                else
                {
                    tileGroup = terrainManager.FindRiverAtRow(GetComponent<GridElement>()._currentRow);
                    MoveMovableObjects(tileGroup);
                }
            }
        }
    }
    */

    public void MoveMovableObjects(TileGroup tileGroup)
    {
        if (tileGroup == null)
        {
            Debug.LogError("TileGroup is null!");
            return; // Early exit
        }

        if (transform.childCount == 0)
        {
            Debug.Log($"{gameObject.name}: Has no child.");
            return; // Early exit
        }
        else
        {
            
            Debug.Log($"{gameObject.name}: Moving child.");

            foreach (Transform child in transform)
            {
                if (child.GetComponent<ObjectController>().canPass)
                {

                    int currentColumn = GetComponent<GridElement>()._currentColumn;
                    int currentRow = GetComponent<GridElement>()._currentRow;
                    Transform nextTile = GetNextWaterTile(tileGroup.flowDirection, currentColumn, currentRow);

                    if (nextTile != null)
                    {
                        Debug.Log($"{gameObject.name}: Moving child to {nextTile.name}");
                        child.SetParent(nextTile);
                        child.localPosition = Vector3.zero;
                    }
                    child.GetComponent<ObjectController>().canPass = false;
                }
                else
                {
                    Debug.Log("This object has been moved already!");
                }
                //child.GetComponent<ObjectController>().canPass = true;
            }

        }

    }

    Transform GetNextWaterTile(FlowDirection flowDirection, int currentColumn, int currentRow)
    {
        Transform target = null; // Declare target here

        switch (flowDirection)
        {
            case FlowDirection.Up:
                if (currentRow < 2) // Adjusted for 0-indexing (assuming 3 rows: 0, 1, 2)
                {
                    target = gridManager.GetGridElement(currentColumn, currentRow + 1).gameObject.transform;
                }
                else
                {
                    target = gridManager.GetGridElement(currentColumn, 0).gameObject.transform; // Wrap to the first row
                }
                break;

            case FlowDirection.Down:
                if (currentRow > 0)
                {
                    Debug.Log($"{gameObject.name}: Move to next row.");
                    target = gridManager.GetGridElement(currentColumn, currentRow - 1).gameObject.transform;
                }
                else
                {
                    Debug.Log($"{gameObject.name}: Wrap to last row.");
                    target = gridManager.GetGridElement(currentColumn, 2).gameObject.transform; // Wrap to the last row
                }
                break;

            case FlowDirection.Left:
                if (currentColumn > 0)
                {
                    target = gridManager.GetGridElement(currentColumn - 1, currentRow).gameObject.transform;
                }
                else
                {
                    target = gridManager.GetGridElement(2, currentRow).gameObject.transform; // Wrap to the last column
                }
                break;

            case FlowDirection.Right:
                if (currentColumn < 2) // Adjusted for 0-indexing
                {
                    target = gridManager.GetGridElement(currentColumn + 1, currentRow).gameObject.transform;
                }
                else
                {
                    target = gridManager.GetGridElement(0, currentRow).gameObject.transform; // Wrap to the first column
                }
                break;
        }

        return target; // Return the target after the switch statement
    }
}
