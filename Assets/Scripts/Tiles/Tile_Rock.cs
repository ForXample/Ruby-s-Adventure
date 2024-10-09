using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Rock : TileBase
{
    public override void OnCharacterEnter(GameObject character)
    {
        if (character.CompareTag("Player"))
        {
            character.GetComponent<PlayerController>().Die();
            Debug.Log("I'm not sure how you did this, but attaboy!");
        }
        else
        {
            Debug.Log("Something else entered Water tile.");
        }
    }
    public override void OnTileShifted()
    {

    }
}
