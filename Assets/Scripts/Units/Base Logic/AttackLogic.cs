using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public abstract class AttackLogic : ActionLogic 
{
    List<UnitController> targets = new List<UnitController>();
    //TargetType type
    [SerializeField] Effect[] effects;
    UnitController myUnit;


    //Probably just change this to a color value at some point
    public GameObject spaceSelect;

    void Start()
    {
        effects = GetComponents<Effect>(); //This needs to be updated
        myUnit = GetComponent<UnitController>();
    }

    public override void Perform()
    {
        foreach(Vector2[] tileGroup in ValidTiles(transform.position))
        {
            List<AttackTileSelect> tiles = new List<AttackTileSelect>();
            foreach(Vector2 pos in tileGroup)
            {

                AttackTileSelect tile = Instantiate(spaceSelect, pos, Quaternion.identity).GetComponent<AttackTileSelect>();
                tiles.Add(tile);
                tile.logic = this;
            }

            foreach (AttackTileSelect tile in tiles)
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
            foreach(Vector2 possiblePos in Pattern(startPos))
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

    protected abstract Vector2[] Pattern(Vector2 startPos);


    public void SelectAttackTargets(Vector2[] positions)
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
}
