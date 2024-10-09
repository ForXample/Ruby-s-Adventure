using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Grass : TileBase
{
    public override void OnCharacterEnter(GameObject character)
    {
        if (character.CompareTag("Player"))
        {
            Debug.Log("Player entered Grass tile.");
        }
        else
        {
            Debug.Log("Something else entered Grass tile.");
        }
    }

    public override void OnTileShifted()
    {

    }
}
