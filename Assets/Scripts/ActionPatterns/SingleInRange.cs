using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class SingleInRange : PatternLogic
{
    [SerializeField] int minRange;
    [SerializeField] int maxRange;
    public override Vector2[][] Pattern(Vector2 startPos)
    {
        List<Vector2[]> individualTiles = new List<Vector2[]>();
        foreach (Vector2 pos in TilePatterns.Range(startPos, maxRange, minRange))
        {
            individualTiles.Add(new Vector2[] { pos });
        }

        return individualTiles.ToArray();
    }

    public override string Description()
    {
        string output = "targets a single unit ";
        if (minRange <= 1)
        {
            return output + $"within {maxRange} tiles ";
        }

        if (minRange != maxRange)
        {
            output += $"between {minRange} and ";
        }

        output += $"{maxRange} tiles ";


        return output;
    }

}
