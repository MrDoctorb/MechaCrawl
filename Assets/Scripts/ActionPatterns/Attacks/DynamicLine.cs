using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class DynamicLine : AttackLogic
{
    protected override Vector2[] Pattern(Vector2 startPos)
    {
        return TilePatterns.Line(startPos, startPos + new Vector2(-3, -3));
    }

}
