using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;


public class LightSource : MonoBehaviour
{
    //Distance of the light
    public int brightness;
    UnitController unit;

    private void OnEnable()
    {
        unit = GetComponent<UnitController>();
        unit.onEndMove += UpdateLighting;
        unit.onStartTurn += UpdateLighting;
    }

    private void OnDisable()
    {
        unit.onEndMove -= UpdateLighting;
        unit.onStartTurn += UpdateLighting;

    }

    void UpdateLighting()
    {
        foreach (Tile tile in TilesInSight())
        {
            tile.SetVisibility(Mathf.Abs((brightness + 2 - Functions.GridDistance(transform.position, tile.transform.position))/(float)brightness));
        }
    }

    /// <summary>
    /// Returns an array of each tile the given lightsource can illuminate
    /// Currently requires optimization
    /// </summary>
    public Tile[] TilesInSight()
    {
        List<Tile> tiles = new List<Tile>();

        for (int i = brightness; i > 0; i--)
        {
            foreach (Vector2 pos in TilePatterns.Range(transform.position, i, i))
            {
                Tile nextTile = TileManager.TileAt(pos);

                if (nextTile != TileManager.defaultTile && !tiles.Contains(nextTile))
                {
                    foreach (Vector2 sightPos in TilePatterns.Line(transform.position, nextTile.transform.position))
                    {
                        Tile sightTile = TileManager.TileAt(sightPos);

                        //If this tile is a wall, add it and stop looking
                        if (sightTile == TileManager.defaultTile || sightTile is Wall)
                        {
                            tiles.Add(sightTile);
                            break;
                        }
                        //If the tiles we have seen so far already have this tile, skip it
                        if (tiles.Contains(sightTile))
                        {
                            continue;
                        }
                        //Ensure this isn't a diagonal
                        Tile xSide = TileManager.TileAt(sightPos + new Vector2(transform.position.x - sightPos.x, 0).normalized);
                        Tile ySide = TileManager.TileAt(sightPos + new Vector2(0, transform.position.y - sightPos.y).normalized);   
                        if ((xSide == TileManager.defaultTile || xSide is Wall) &&(ySide == TileManager.defaultTile || ySide is Wall))
                        {
                            tiles.Add(xSide);
                            tiles.Add(ySide);
                            break;
                        }
                        //Add the valid tile to tiles we can see
                        tiles.Add(sightTile);
                    }
                }
            }
        }

        return tiles.ToArray();

    }

}
