using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Linq;
using UnityEngine;
using Zanespace;

public class AttackLogic : ActionLogic
{
    List<UnitController> targets = new List<UnitController>();
    //TargetType type
    [SelectType] [SerializeReference] PatternLogic pattern;
    //public Effect[] effects;
    [SelectType] [SerializeReference] public List<Effect> effects = new List<Effect>();

    //Probably just change this to a color value at some point
    public GameObject spaceSelect;

    private void Start()
    {
    }

    public override void Perform()
    {
        foreach (Vector2[] tileGroup in ValidTiles(transform.position))
        {
            List<MultiTileSelect> tiles = new List<MultiTileSelect>();
            foreach (Vector2 pos in tileGroup)
            {
                MultiTileSelect tile = Instantiate(spaceSelect, pos, Quaternion.identity).GetComponent<MultiTileSelect>();
                tiles.Add(tile);
                tile.logic = this;
            }

            foreach (MultiTileSelect tile in tiles)
            {
                tile.tilesInSet = tiles.ToArray();
            }
        }
    }

    public Vector2[][] ValidTiles(Vector2 startPos)
    {
        List<Vector2[]> validTiles = new List<Vector2[]>();
        foreach (Vector2[] group in pattern.Pattern(startPos))
        {
            List<Vector2> tiles = new List<Vector2>();
            foreach (Vector2 pos in group)
            {
                if (TileManager.TileAt(pos).type == TileType.BlockAll)
                {
                    continue;
                }
                bool good = true;
                foreach (Vector2 lineOfSightPos in TilePatterns.Line(startPos, pos))
                {
                    if (TileManager.TileAt(lineOfSightPos).type == TileType.BlockAll)
                    {
                        good = false;
                        break;
                    }
                }
                if (good)
                {
                    tiles.Add(pos);
                }
            }
            validTiles.Add(tiles.ToArray());
        }
        return validTiles.ToArray();
    }

   /* Vector2[][] PossibleTiles(Vector2 startPos)
    {
        List<Vector2[]> fourDirections = new List<Vector2[]>();
        for (int i = 0; i < 4; ++i)
        {
            Vector2 offset = Functions.DirectionToVector((Direction)i);
            List<Vector2> tiles = new List<Vector2>();
            foreach (Vector2 possiblePos in pattern.Pattern(startPos))
            {
                Vector2 pos = Functions.RotatePointAroundPoint(startPos, possiblePos, (Direction)i);
                if (TileManager.TileAt(pos).type == TileType.BlockAll)
                {
                    print(TileManager.TileAt(pos).transform.position + " " + pos);
                    break;
                }
                tiles.Add(pos);
            }
            fourDirections.Add(tiles.ToArray());
        }
        return fourDirections.ToArray();
    }*/

    public override void SelectTargets(Vector2[] positions)
    {
        foreach (Vector2 pos in positions)
        {
            UnitController possibleUnit = TileManager.TileAt(pos).unit;
            if (possibleUnit != null)
            {
                targets.Add(possibleUnit);
            }
        }
        foreach (UnitController target in targets)
        {
            foreach (Effect effect in effects)
            {
                effect.ApplyTo(target);
            }
        }

        targets.Clear();
        myUnit.EndTurn();
    }

    public override string Description()
    {
        string output = "";
        for (int i = 0; i < effects.Count; i++)
        {
            output += effects[i].Description();
            if (i + 1 < effects.Count)
            {
                output += " and";
            }
            output += ", ";
        }
        output += pattern.Description();
        return char.ToUpper(output[0]) + output.Substring(1);
    }


}
