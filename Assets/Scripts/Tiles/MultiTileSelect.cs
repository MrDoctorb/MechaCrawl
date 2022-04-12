using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTileSelect : MonoBehaviour
{
    public ActionLogic logic;
    public MultiTileSelect[] tilesInSet;

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

}
