using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class MoveBasicRange : MoveLogic
{
    protected override Vector2[] Pattern()
    {
        return TilePatterns.Range(transform.position, maxRange, minRange);
    }
}
