using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public PlayerController playerController;

    public TerrainFormationManager terraimManager;

    public GameManager gameManager;

    public LevelDesign_SO levelDesign; // Reference to ScriptableObject containing level data

    public GridElement[,] _grid;

    public Transform gridMap;

    private void Awake()
    {
        // Initialize the grid size based on the LevelDesign ScriptableObject
        _grid = new GridElement[levelDesign.columns, levelDesign.rows];

        PopulateGrid(); // Call to populate the grid based on ScriptableObject data

        RefreshIndices();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OutputGridElements();
        }
    }

    private void PopulateGrid()
    {
        for (var row = 0; row < levelDesign.rows; row++)
        {
            for (var column = 0; column < levelDesign.columns; column++)
            {
                // Instantiate tile based on the ScriptableObject data
                var tilePrefab = levelDesign.GetTile(row, column);
                var tileInstance = Instantiate(tilePrefab, new Vector3(column, 0, row), Quaternion.identity);
                tileInstance.transform.SetParent(gridMap);

                // Assuming the prefab has a GridElement component
                var gridElement = tileInstance.GetComponent<GridElement>();
                _grid[column, row] = gridElement;

                // Initialize the grid element
                gridElement.Initialize(this);
            }
        }
    }

    // Outputs the grid elements from (0,0) to (2,2)
    private void OutputGridElements()
    {
        int maxRows = levelDesign.rows;
        int maxColumns = levelDesign.columns;

        Debug.Log("Outputting grid elements from (0,0) to (2,2):");

        for (int row = 0; row < maxRows; row++)
        {
            for (int column = 0; column < maxColumns; column++)
            {
                var gridElement = GetGridElement(column, row);
                if (gridElement != null)
                {
                    Debug.Log($"Grid Element at ({column}, {row}): {gridElement.name}");
                }
                else
                {
                    Debug.Log($"Grid Element at ({column}, {row}): Empty or null");
                }
            }
        }
    }

    // Shifts the given column one step up with wrap around
    public void ShiftColumnUp(int column)
    {
        TileGroup riverGroup = terraimManager.FindRiverAtColumn(column); // Implement this method

        if (riverGroup != null)
        {
            riverGroup.flowDirection = FlowDirection.Up;
            Debug.Log("River direction changed to: " + riverGroup.flowDirection);
            // return; // Exit without shifting tiles
        }

        var temp = _grid[column, levelDesign.rows - 1];
        for (var row = levelDesign.rows - 1; row > 0; row--)
        {
            _grid[column, row] = _grid[column, row - 1];
        }
        _grid[column, 0] = temp;

        // Update player position if they are in the shifted column
        if (playerController.currentColumn == column)
        {
            if (playerController.currentRow < 2)
            {
                playerController.currentRow += 1;
            }
            else
            {
                playerController.currentRow = 0;
            }
        }
        RefreshIndices();
        terraimManager.RefreshTileGroups();
        terraimManager.CheckForRivers();

        gameManager.ChangeTurnTotal(1);

    }

    // Shifts the given column one step down with wrap around
    public void ShiftColumnDown(int column)
    {
        TileGroup riverGroup = terraimManager.FindRiverAtColumn(column); // Implement this method

        if (riverGroup != null)
        {
            riverGroup.flowDirection = FlowDirection.Down;
            Debug.Log("River direction changed to: " + riverGroup.flowDirection);
            // return; // Exit without shifting tiles
        }

        var temp = _grid[column, 0];
        for (var row = 0; row < levelDesign.rows - 1; row++)
        {
            _grid[column, row] = _grid[column, row + 1];
        }
        _grid[column, levelDesign.rows - 1] = temp;

        if (playerController.currentColumn == column)
        {
            if (playerController.currentRow == 0)
            {
                playerController.currentRow = 2;
            }
            else
            {
                playerController.currentRow -= 1;
            }
        }
        RefreshIndices();
        terraimManager.RefreshTileGroups();
        terraimManager.CheckForRivers();

        gameManager.ChangeTurnTotal(1);

    }

    // Shifts the given row one step right with wrap around
    public void ShiftRowRight(int row)
    {

        TileGroup riverGroup = terraimManager.FindRiverAtRow(row); // Implement this method

        if (riverGroup != null)
        {
            riverGroup.flowDirection = FlowDirection.Right;
            Debug.Log("River direction changed to: " + riverGroup.flowDirection);
            // return; // Exit without shifting tiles
        }


        var temp = _grid[levelDesign.columns - 1, row];
        for (var column = levelDesign.columns - 1; column > 0; column--)
        {
            _grid[column, row] = _grid[column - 1, row];
        }
        _grid[0, row] = temp;

        // Update player position if they are in the shifted row
        if (playerController.currentRow == row)
        {
            if (playerController.currentColumn < 2)
            {
                playerController.currentColumn += 1;
            }
            else
            {
                playerController.currentColumn = 0; 
            }
        }
        RefreshIndices();
        terraimManager.RefreshTileGroups();
        terraimManager.CheckForRivers();

        gameManager.ChangeTurnTotal(1);

    }

    // Shifts the given row one step left with wrap around
    public void ShiftRowLeft(int row)
    {

        TileGroup riverGroup = terraimManager.FindRiverAtRow(row);

        if (riverGroup != null)
        {
            riverGroup.flowDirection = FlowDirection.Left;
            Debug.Log("River direction changed to: " + riverGroup.flowDirection);
            //return; // Exit without shifting tiles
        }


        var temp = _grid[0, row];
        for (var column = 0; column < levelDesign.columns - 1; column++)
        {
            _grid[column, row] = _grid[column + 1, row];
        }
        _grid[levelDesign.columns - 1, row] = temp;

        // Update player position if they are in the shifted row
        if (playerController.currentRow == row)
        {
            if (playerController.currentColumn == 0)
            {
                playerController.currentColumn = 2;
            }
            else
            {
                playerController.currentColumn -= 1;
            }
        }
        RefreshIndices();
        terraimManager.RefreshTileGroups();
        terraimManager.CheckForRivers();

        gameManager.ChangeTurnTotal(1);

    }



    public void RefreshIndices()
    {
        for (var column = 0; column < levelDesign.columns; column++)
        {
            for (var row = 0; row < levelDesign.rows; row++)
            {
                _grid[column, row].UpdateIndices(row, column);
                _grid[column, row].transform.position = new Vector3(column - 1, 0, row);
            }
        }
        
    }

    public GridElement GetGridElement(int column, int row)
    {
        return _grid[column, row];
    }

    // Moves the entire row according to the given delta (+/- 1)
    public void MoveRow(int targetRow, float delta)
    {
        for (var column = 0; column < levelDesign.columns; column++)
        {
            _grid[column, targetRow].transform.position = new Vector3(column - 1 + delta, 0, targetRow);
        }
    }

    // Moves the entire column according to the given delta (+/- 1)
    public void MoveColumn(int targetColumn, float delta)
    {
        for (var row = 0; row < levelDesign.rows; row++)
        {
            _grid[targetColumn, row].transform.position = new Vector3(targetColumn - 1, 0, row + delta);
        }
    }
    
}
