using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class DynamicLine : PatternLogic
{
    public override Vector2[] Pattern(Vector2 startPos)
    {
        return TilePatterns.Line(startPos, startPos + new Vector2(-3, -3));
    }

    public override string Description()
    {
        return " to each enemy in a curved line";
    }

}
