using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class Line : AttackLogic
{
    [SerializeField] int range;
    protected override Vector2[] Pattern(Vector2 startPos)
    {
        List<Vector2> possiblePos = new List<Vector2>();
        for(int i = 0; i < range; ++i)
        {
            possiblePos.Add(startPos + new Vector2(0, i + 1));
        }
        return possiblePos.ToArray();
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
