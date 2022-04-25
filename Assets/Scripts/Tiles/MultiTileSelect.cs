using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTileSelect : MonoBehaviour
{
    public ActionLogic logic;
    public MultiTileSelect[] tilesInSet;
    public Color color;
    public SpriteRenderer rend { private set; get; }

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        color.a = .5f;
        rend.color = color;
    }


    void OnMouseDown()
    {
        Vector2[] tiles = new Vector2[tilesInSet.Length];
        for (int i = 0; i < tilesInSet.Length; ++i)
        {
            tiles[i] = tilesInSet[i].transform.position;
        }

        logic.SelectTargets(tiles);

        foreach (MultiTileSelect tileSelect in FindObjectsOfType<MultiTileSelect>())
        {
            Destroy(tileSelect.gameObject);
        }
    }

    private void OnMouseEnter()
    {
        foreach(MultiTileSelect tile in tilesInSet)
        {
            tile.rend.color = color + new Color(.25f, .25f, .25f, 0);
        }
    }

    private void OnMouseExit()
    {
        foreach (MultiTileSelect tile in tilesInSet)
        {
            tile.rend.color = color;
        }
    }

}
