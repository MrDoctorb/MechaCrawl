using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zanespace;


public class TileManager : MonoBehaviour
{
    [SerializeField] DungeonGenerator gen;

    public GameObject tileref;

    static Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();

    static List<Vector2> roomTiles = new List<Vector2>();
    static List<Vector2> hallwayTiles = new List<Vector2>();
    static List<Vector2> connectorTiles = new List<Vector2>();
    void Start()
    {
        References.tManager = this;

        foreach (DungeonTile tile in gen.Generate())
        {
            if (tile != null)
            {
                tiles.Add(tile.pos, Instantiate(tileref, (Vector2)tile.pos, Quaternion.identity).GetComponent<Tile>());
                switch (tile.type)
                {
                    case DungeonTileType.Room:
                        roomTiles.Add(tile.pos);
                        break;
                    case DungeonTileType.Hallway:
                        hallwayTiles.Add(tile.pos);
                        break;
                    case DungeonTileType.Connector:
                        connectorTiles.Add(tile.pos);
                        break;

                }
            }
        }
        List<Vector2> doorways = new List<Vector2>();
        foreach (Vector2 connect in connectorTiles)
        {
            foreach (Vector2 adjacent in new List<Vector2>(TilePatterns.Range(connect, 1, 1)))
            {
                if (roomTiles.Contains(adjacent))
                {
                    doorways.Add(connect);
                }
                else
                {

                    hallwayTiles.Add(connect);
                }
            }
        }
        connectorTiles = doorways;

        Vector2 entrancePos = (roomTiles[Random.Range(0, roomTiles.Count)]);
        GameObject up = tiles[entrancePos].gameObject;
        Destroy(up.GetComponent<Tile>());
        up.AddComponent<Entrance>();
        up.GetComponent<SpriteRenderer>().color = Color.green;
        tiles[entrancePos] = up.GetComponent<Entrance>();

        Vector2 exitPos = entrancePos;
        while(exitPos == entrancePos)
        {
            exitPos = (roomTiles[Random.Range(0, roomTiles.Count)]);
        }
        GameObject down = tiles[exitPos].gameObject;
        Destroy(down.GetComponent<Tile>());
        down.AddComponent<Exit>();
        down.GetComponent<SpriteRenderer>().color = Color.red;
        tiles[exitPos] = down.GetComponent<Exit>();

        /* foreach (Vector2 pos in TilePatterns.Range(Vector2.zero, 60))
         {
             tiles.Add(pos, Instantiate(tileref, pos, Quaternion.identity).GetComponent<Tile>());
             if (Random.Range(0, 10) == 0)
             {
                 tiles[pos].type = TileType.BlockAll;
                 tiles[pos].GetComponent<SpriteRenderer>().color = Color.black;
             }
         }*/
    }

    public static bool IsTileOpen(Vector2 pos)
    {
        return tiles.ContainsKey(pos) && tiles[pos].IsOpen();
    }

    public static Tile TileAt(Vector2 pos)
    {
        if (!tiles.ContainsKey(pos))
        {
            return References.tManager.GetComponent<Wall>();
        }
        return tiles[pos];
    }

    public static Tile RandomTile()
    {
        return tiles[tiles.Keys.ElementAt(Random.Range(0, tiles.Count))];
    }

    public void ColorCodeTiles()
    {

        foreach (Vector2 room in roomTiles)
        {
            TileAt(room).GetComponent<SpriteRenderer>().color = Color.red;
        }
        foreach (Vector2 hallway in hallwayTiles)
        {
            TileAt(hallway).GetComponent<SpriteRenderer>().color = Color.green;
        }
        foreach (Vector2 door in connectorTiles)
        {
            TileAt(door).GetComponent<SpriteRenderer>().color = Color.blue;
        }

    }
}