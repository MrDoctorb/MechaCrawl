using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;


public class LightSource : MonoBehaviour
{
    //Distance of the light
    public int brightness;
    UnitController unit;
    List<Tile> litTiles;

    private void OnEnable()
    {
        unit = GetComponent<UnitController>();
        unit.onEndMoveSingle += UpdateLighting;
        unit.onStartTurn += UpdateLighting;
        litTiles = new List<Tile>();
        if (unit is EnemyController)
        {
            enabled = false;
            return;
        }
        UpdateLighting();

    }

    private void Start()
    {
        References.tManager.onLevelLoadStart += ClearLighting;
    }

    private void OnDisable()
    {
        
        unit.onEndMoveSingle -= UpdateLighting;
        unit.onStartTurn -= UpdateLighting;
        References.tManager.onLevelLoadStart -= ClearLighting;

        foreach (Tile tile in litTiles)
        { 

            tile.onLightChange -= UpdateLighting;
            tile.lightSources.Remove(this);
            tile.UpdateLighting();
        }
        litTiles = new List<Tile>();
    }

    void UpdateLighting()
    {

        List<Tile> newTiles = new List<Tile>(TilesInSight());
        List<Tile> oldTiles = new List<Tile>(litTiles);

        //Only Unsubscribe for tiles that are leaving the zone
        foreach (Tile tile in oldTiles)
        {
            //Tiles that aren't in both of the sets
            if (!newTiles.Contains(tile) && tile != null)
            {
                //Unsubscribe Tile
                tile.onLightChange -= UpdateLighting;
                tile.lightSources.Remove(this);
                //Set tile to be "explored" darkness
                tile.UpdateLighting();
                //Remove tile from litTiles
                litTiles.Remove(tile);
            }
        }

        //Only add tiles that aren't already in litTiles
        foreach (Tile tile in newTiles)
        {
            if (!litTiles.Contains(tile))
            {
                //Subscribe Tile
                tile.onLightChange += UpdateLighting;
                tile.lightSources.Add(this);
                //Add tile to lit Tiles
                litTiles.Add(tile);
            }
        }

        foreach (Tile tile in litTiles)
        {
            //Set the light level 
            tile.UpdateLighting();
        }

    }

    public float Brightness(int distance)
    {
        return Mathf.Abs((brightness + 2 - distance)/(float)brightness);
    }

    /// <summary>
    /// Returns an array of each tile the given lightsource can illuminate
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
                            if (sightPos.y >= transform.position.y)
                            {
                                tiles.Add(sightTile);
                            }
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

                            if (xSide.transform.position.y >= transform.position.y)
                            {
                                tiles.Add(xSide);
                            }
                            if (ySide.transform.position.y >= transform.position.y)
                            {
                                tiles.Add(ySide);
                            }
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

    void ClearLighting()
    {
        litTiles = new List<Tile>();
    }
}
