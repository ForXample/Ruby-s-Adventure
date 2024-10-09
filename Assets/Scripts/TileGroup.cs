using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowDirection { Right, Down, Left, Up }

[System.Serializable]
public class TileGroup
{
    public List<GameObject> gameObjects;
    public FlowDirection flowDirection; // New property

    public TileGroup(GameObject gameObject1, GameObject gameObject2, GameObject gameObject3)
    {
        gameObjects = new List<GameObject> { gameObject1, gameObject2, gameObject3 };
        flowDirection = FlowDirection.Right; // Default for row
    }
}


