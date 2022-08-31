using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class LightSource : MonoBehaviour
{
    //Distance of the light
    public int brightness;


    //TEMPORARY START FUNCTION
    private void Start()
    {
        GetComponent<UnitController>().onMove += UpdateLighting;
    }

    void UpdateLighting()
    {
        foreach(Tile tile in TilesInSight())
        {
            //tile.SetVisibility(Mathf.Abs((brightness + 1 - Functions.GridDistance(transform.position, tile.transform.position))/(float)brightness));

            //tile.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    /// <summary>
    /// Returns an array of each tile the given lightsource can illuminate
    /// Currently requires optimization
    /// </summary>
    public Tile[] TilesInSight()
    {
        List<Tile> tiles = new List<Tile>();

        for(int i = brightness; i > 0; i--)
        {
            foreach (Vector2 pos in TilePatterns.Range(transform.position, i, i))
            {
                Tile nextTile = TileManager.TileAt(pos);
                
                if (nextTile != TileManager.defaultTile && !tiles.Contains(nextTile))
                {
                    //nextTile.GetComponent<SpriteRenderer>().color = Color.red;
                    foreach (Vector2 sightPos in TilePatterns.Line(transform.position, nextTile.transform.position))
                    {
                        Tile sightTile = TileManager.TileAt(sightPos);
                        
                 
                        sightTile.GetComponent<SpriteRenderer>().color = Color.red;
                        
                        
                        if (sightTile == TileManager.defaultTile)
                        {
                            break;
                        }
                        if (tiles.Contains(sightTile))
                        {
                            continue;
                        }
                        else
                        {
                        }    
                       // tiles.Add(sightTile);
                    }
                }
            }
        }
        
        return tiles.ToArray();

    }

}
