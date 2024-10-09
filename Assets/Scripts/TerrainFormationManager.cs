using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainFormationManager : MonoBehaviour
{
    private GridManager _gridManager;
    private LevelDesign_SO levelDesign;

    public List<TileGroup> tileGroups = new List<TileGroup>();

    public GameObject river;

    void Start()
    {
        _gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
        levelDesign = _gridManager.levelDesign;
        CheckForRivers();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CheckForRivers();
        }

    }

    void AddNewGameObjectGroup(GameObject gameObject1, GameObject gameObject2, GameObject gameObject3)
    {
        TileGroup newGroup = new TileGroup(gameObject1, gameObject2, gameObject3);
        GameObject riverParent = Instantiate(river);
        riverParent.transform.SetParent(_gridManager.gridMap);
        riverParent.GetComponent<Terrain_River>().tileGroup = newGroup;
        foreach (GameObject child in newGroup.gameObjects)
        {
            child.GetComponent<Tile_Water>().isRiver = true;
            child.transform.SetParent(riverParent.transform);
        }
        riverParent.GetComponent<Terrain_River>().GetChildren();



        // Create a list of the new group game objects
        var newGameObjects = new List<GameObject> { gameObject1, gameObject2, gameObject3 };

        // Use HashSet for comparison, as it disregards the order of elements
        var newGroupSet = new HashSet<GameObject>(newGameObjects);

        foreach (TileGroup group in tileGroups)
        {
            var existingGroupSet = new HashSet<GameObject>(group.gameObjects);

            if (existingGroupSet.SetEquals(newGroupSet)) // Check if they are equal disregarding order
            {
                return; // Exit if a duplicate is found
            }
        }

        // Determine the flow direction
        var gridElement1 = gameObject1.GetComponent<GridElement>();
        var gridElement2 = gameObject2.GetComponent<GridElement>();
        var gridElement3 = gameObject3.GetComponent<GridElement>();

        if (gridElement1._currentRow == gridElement2._currentRow && gridElement2._currentRow == gridElement3._currentRow)
        {
            newGroup.flowDirection = FlowDirection.Right; // Default flow direction for rows
        }
        else if (gridElement1._currentColumn == gridElement2._currentColumn && gridElement2._currentColumn == gridElement3._currentColumn)
        {
            newGroup.flowDirection = FlowDirection.Down; // Default flow direction for columns
        }

        tileGroups.Add(newGroup);
    }




    bool CheckForRiversInRow(int row)
    {
        if (row < 0 || row >= _gridManager.levelDesign.rows)
        {
            return false;
        }


        for (int col = 0; col < 3; col++)
        {
            if (col >= _gridManager.levelDesign.columns)
            {
                return false;
            }

            if (_gridManager._grid[col, row] == null || !_gridManager._grid[col, row].gameObject.CompareTag("Water"))
            {
                return false;
            }
        }
        return true;
    }

    bool CheckForRiversInColumn(int column)
    {
        if (column < 0 || column >= _gridManager.levelDesign.columns)
        {
            return false;
        }


        for (int row = 0; row < 3; row++)
        {
            if (row >= _gridManager.levelDesign.rows)
            {
                return false;
            }

            if (_gridManager._grid[column, row] == null || !_gridManager._grid[column, row].gameObject.CompareTag("Water"))
            {
                return false;
            }
        }

        return true;
    }


    public void CheckForRivers()
    {
        for (int row = 0; row < 3; row++)
        {
            if (CheckForRiversInRow(row))
            {
                AddRiverTilesToGroup(row, true);
            }
        }

        for (int col = 0; col < 3; col++)
        {
            if (CheckForRiversInColumn(col))
            {
                AddRiverTilesToGroup(col, false);
            }
        }
    }

    bool CheckIfFormLine(TileGroup tileGroup)
    {
        var gameObjects = tileGroup.gameObjects;
        if (gameObjects[0].GetComponent<GridElement>()._currentRow == gameObjects[1].GetComponent<GridElement>()._currentRow &&
            gameObjects[1].GetComponent<GridElement>()._currentRow == gameObjects[2].GetComponent<GridElement>()._currentRow)
        {
            return true;
        }
        if (gameObjects[0].GetComponent<GridElement>()._currentColumn == gameObjects[1].GetComponent<GridElement>()._currentColumn &&
            gameObjects[1].GetComponent<GridElement>()._currentColumn == gameObjects[2].GetComponent<GridElement>()._currentColumn)
        {
            return true;
        }
        return false;
    }

    public void RefreshTileGroups()
    {
        for (int i = tileGroups.Count - 1; i >= 0; i--)
        {
            TileGroup group = tileGroups[i];
            if (!CheckIfFormLine(group))
            {
                // Set isRiver to false for each Tile_Water component
                foreach (GameObject tile in group.gameObjects)
                {
                    tile.GetComponent<Tile_Water>().isRiver = false;
                    tile.transform.SetParent(_gridManager.gridMap);
                }
                tileGroups.RemoveAt(i);
            }
        }
    }




    void AddRiverTilesToGroup(int index, bool isRow)
    {
        GameObject tile1, tile2, tile3;

        if (isRow)
        {
            tile1 = _gridManager._grid[0, index].gameObject; // First column
            tile2 = _gridManager._grid[1, index].gameObject; // Second column
            tile3 = _gridManager._grid[2, index].gameObject; // Third column
        }
        else
        {
            tile1 = _gridManager._grid[index, 0].gameObject; // First row
            tile2 = _gridManager._grid[index, 1].gameObject; // Second row
            tile3 = _gridManager._grid[index, 2].gameObject; // Third row
        }

        // Check if tiles are not null before adding
        if (tile1 != null && tile2 != null && tile3 != null)
        {
            AddNewGameObjectGroup(tile1, tile2, tile3);
        }
    }




    public TileGroup FindRiverAtRow(int row)
    {
        foreach (TileGroup group in tileGroups)
        {
            if (group.gameObjects.All(tile => tile.gameObject.GetComponent<GridElement>()._currentRow == row)) // Assuming z is the row position
            {
                return group;
            }
        }
        return null; // No river found
    }

    public TileGroup FindRiverAtColumn(int column)
    {
        foreach (TileGroup group in tileGroups)
        {
            if (group.gameObjects.All(tile => tile.gameObject.GetComponent<GridElement>()._currentColumn == column)) // Assuming x is the column position
            {
                return group;
            }
        }
        return null; // No river found
    }
}
