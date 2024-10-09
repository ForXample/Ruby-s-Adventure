using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelDesign", menuName = "Level Design")]
public class LevelDesign_SO : ScriptableObject
{
    public int rows;
    public int columns;
    public int startingRow;
    public int startingCol;

    // Store tile prefabs directly (like GrassTile, WaterTile)
    public GameObject[] tiles; // Array of GameObjects representing the grid

    // Initialize the tile array based on rows and columns
    public void InitializeTiles()
    {
        tiles = new GameObject[rows * columns];
    }

    // Helper method to get the GameObject for a specific tile at row, column
    public GameObject GetTile(int row, int column)
    {
        return tiles[row * columns + column];
    }
}


