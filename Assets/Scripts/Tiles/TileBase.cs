using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : MonoBehaviour
{
    public abstract void OnCharacterEnter(GameObject character);
    public abstract void OnTileShifted();
}

