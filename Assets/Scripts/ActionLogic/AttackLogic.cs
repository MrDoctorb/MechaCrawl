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
    [SerializeField] PatternLogic pattern;
    //public Effect[] effects;
    [SerializeReference]public List<Effect> effects = new List<Effect>();
    public Damage test;

    //Probably just change this to a color value at some point
    public GameObject spaceSelect;

    private void Start()
    {
        //effectTypes = FindEffectSubClasses().ToArray();

        foreach (Type type in FindEffectSubClasses())
        {
            effects.Add((Effect)Activator.CreateInstance(type));
        }
    }

    public IEnumerable<Type> FindEffectSubClasses()
    {
        var baseType = typeof(Effect);
        var assembly = baseType.Assembly;

        return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
    }
    public override void Perform()
    {
        foreach(Vector2[] tileGroup in ValidTiles(transform.position))
        {
            List<MultiTileSelect> tiles = new List<MultiTileSelect>();
            foreach(Vector2 pos in tileGroup)
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
        //These will be split later, right now possible is doing too much and is bad. But works with the current attack
        return PossibleTiles(startPos);
    }

    Vector2[][] PossibleTiles(Vector2 startPos)
    {
        List<Vector2[]> fourDirections = new List<Vector2[]>();
        for (int i = 0; i < 4; ++i)
        {
            Vector2 offset = Functions.DirectionToVector((Direction)i);
            List<Vector2> tiles = new List<Vector2>();
            foreach(Vector2 possiblePos in pattern.Pattern(startPos))
            {
                Vector2 pos = Functions.RotatePointAroundPoint(startPos, possiblePos, (Direction)i);
                if (TileManager.TileAt(pos).type == TileType.BlockAll)
                {
                    break;
                }
                tiles.Add(pos);
            }
            fourDirections.Add(tiles.ToArray());
        }
        return fourDirections.ToArray();
    }

    public override void SelectTargets(Vector2[] positions)
    {
        foreach(Vector2 pos in positions)
        {
            UnitController possibleUnit = TileManager.TileAt(pos).unit;
            if (possibleUnit != null)
            {
                targets.Add(possibleUnit);
            }
        }
        foreach(UnitController target in targets)
        {
            foreach(Effect effect in effects)
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
        for(int i = 0; i < effects.Count; i++)
        {
            output += effects[i].Description();
            if(i + 1 < effects.Count)
            {
                output += " and";
            }
            output += ", ";
        }
        output += pattern.Description();
        return char.ToUpper(output[0]) + output.Substring(1); 
    }


}
