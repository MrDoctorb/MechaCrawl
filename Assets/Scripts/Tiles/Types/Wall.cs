using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;
public class Wall : Tile
{
    public override bool IsOpen()
    {
        return false;
    }
}
