using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zanespace;

[RequireComponent(typeof(Generator))]
public class TileManager : MonoBehaviour
{
    [SerializeField] Generator gen;


    [SerializeField] GameObject tileref, wallref;
    [SerializeField] GameObject[] effectTiles;
    [SerializeField] Sprite stairUpSprite, stairDownSprite;

    static Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();

    static List<Vector2> roomTiles = new List<Vector2>();
    static List<Vector2> hallwayTiles = new List<Vector2>();
    static List<Vector2> connectorTiles = new List<Vector2>();
    static List<Vector2> wallTiles = new List<Vector2>();

    public static Tile defaultTile;

    void Start()
    {
        References.tManager = this;
        defaultTile = References.tManager.GetComponent<Wall>();
        NewFloor();
        References.uManager.EndTurn();
    }
    public void NewFloor()
    {
        DeleteFloor();
        SpawnFloor();
        AddEffectTiles();
        References.uManager.NewFloorUnitSetup();
        Camera.main.GetComponent<CameraController>().NewFloor();
    }

    void DeleteFloor()
    {
        tiles.Clear();
        roomTiles.Clear();
        hallwayTiles.Clear();
        connectorTiles.Clear();

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.gameObject != gameObject)
            {
                Destroy(tile.gameObject);
            }
        }
    }

    void SpawnFloor()
    {
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
        up.GetComponent<SpriteRenderer>().sprite = stairUpSprite;
        tiles[entrancePos] = up.GetComponent<Entrance>();

        Vector2 exitPos = entrancePos;
        while (exitPos == entrancePos)
        {
            exitPos = roomTiles[Random.Range(0, roomTiles.Count)];
        }
        GameObject down = tiles[exitPos].gameObject;
        Destroy(down.GetComponent<Tile>());
        down.AddComponent<Exit>();
        down.GetComponent<SpriteRenderer>().sprite = stairDownSprite;
        tiles[exitPos] = down.GetComponent<Exit>();


        SpawnWalls();
    }

    void SpawnWalls()
    {
        List<Vector2> allTiles = new List<Vector2>();
        allTiles.AddRange(roomTiles);
        allTiles.AddRange(hallwayTiles);
        allTiles.AddRange(connectorTiles);
        wallTiles = new List<Vector2>();
        foreach (Vector2 tile in allTiles)
        {
            if (!tiles.ContainsKey(tile + Vector2.up))
            {
                wallTiles.Add(tile + Vector2.up);
                Instantiate(wallref, tile + Vector2.up, Quaternion.identity);
            }
        }
        allTiles.AddRange(wallTiles);
    }

    void AddEffectTiles()
    {
        //Very Rudimentary, need to add a weight system -------------------------------------------------------------------
        foreach (Vector2 tilePos in roomTiles)
        {
            if (Random.Range(0, 50) == 0)
            {
                Destroy(TileAt(tilePos).gameObject);
                tiles.Remove(tilePos);
                GameObject effectTile = effectTiles[Random.Range(0, effectTiles.Length)];
                tiles.Add(tilePos, Instantiate(effectTile, tilePos, Quaternion.identity).GetComponent<Tile>());
            }
        }
    }

    public static bool IsTileOpen(Vector2 pos)
    {
        return tiles.ContainsKey(pos) && tiles[pos].IsOpen();
    }

    public static Tile TileAt(Vector2 pos)
    {
        if (!tiles.ContainsKey(pos))
        {
            return defaultTile;
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
