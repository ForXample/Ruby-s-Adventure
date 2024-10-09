using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain_River : MonoBehaviour
{
    private List<GameObject> tiles;
    public TileGroup tileGroup;

    void Start()
    {
        tiles = tileGroup.gameObjects;
    }

    void Update()
    {
        if (transform.childCount == 0)
        {
            //Debug.Log("Destroying.");
            gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].GetComponent<Tile_Water>().MoveMovableObjects(tileGroup);
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].transform.childCount; j++)
                {
                    tiles[i].transform.GetChild(j).gameObject.GetComponent<ObjectController>().canPass = true;
                }
            }
        }
    }

    public void GetChildren()
    {
        tiles = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            tiles.Add(child);
        }
    }
}
