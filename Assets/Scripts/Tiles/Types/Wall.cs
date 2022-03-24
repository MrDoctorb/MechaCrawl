using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;
public class Wall : Tile
{
    private void Start()
    {
        type = TileType.BlockAll;
    }


    public override void EnterEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void StopEffect()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsOpen()
    {
        return false;
    }
}
