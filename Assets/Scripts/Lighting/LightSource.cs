using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;


public class LightSource : MonoBehaviour
{
    //Distance of the light
    public int brightness;

    //TEMPORARY START FUNCTION
    private void OnEnable()
    {
        GetComponent<UnitController>().onEndMove += UpdateLighting;
    }

    private void OnDisable()
    {
        GetComponent<UnitController>().onEndMove -= UpdateLighting;
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

                        if (sightTile == TileManager.defaultTile || sightTile is Wall)
                        {
                            print(sightTile.name + " " + sightPos);
                            tiles.Add(sightTile);
                            break;
                        }
                        if (tiles.Contains(sightTile))
                        {
                            continue;
                        }
                        tiles.Add(sightTile);
                    }
                }
            }
        }

        return tiles.ToArray();

    }

}
