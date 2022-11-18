using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class Line : PatternLogic
{
    [SerializeField] int range;
    public override Vector2[][] Pattern(Vector2 startPos)
    {
        List<Vector2[]> pattern = new List<Vector2[]>();
        for (int i = 0; i < 4; ++i)
        {
            Vector2 offset = Functions.DirectionToVector((Direction)i);
            List<Vector2> tiles = new List<Vector2>();
            for (int j = 0; j < range; ++j)
            {
                Vector2 possiblePos = startPos + (offset * (j + 1));
                tiles.Add(possiblePos);
            }
            pattern.Add(tiles.ToArray());
        }
        return pattern.ToArray();
    }

    public override string Description()
    {
        string output;
        if (range > 1)
        {
            output = "targets each enemy in a line " + range + " long ";
        }
        else
        {
            output = "targets an adjacent enemy ";
        }

        return output;
    }

    /*public override void DisplayAttack()
    {
        for (int i = 0; i < 4; ++i)
        {
            Vector2 offset = Functions.DirectionToVector((Direction)i);
            List<AttackTileSelect> tiles = new List<AttackTileSelect>();
            for (int j = 0; j < range; ++j)
            {
                Vector2 possiblePos = (Vector2)transform.position + (offset * (j + 1));
                if (TileManager.TileAt(possiblePos).type == TileType.BlockAll)
                {
                    break;
                }
                AttackTileSelect tile = Instantiate(spaceSelect, possiblePos, Quaternion.identity).GetComponent<AttackTileSelect>();
                tiles.Add(tile);
                tile.logic = this;
            }
            foreach (AttackTileSelect tile in tiles)
            {
                tile.tilesInSet = tiles.ToArray();
            }
        }
    }*/

}
