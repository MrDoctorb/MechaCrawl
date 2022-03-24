using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTileSelect : MonoBehaviour
{
    public AttackLogic logic;
    public AttackTileSelect[] tilesInSet;

    void OnMouseDown()
    {
        Vector2[] tiles = new Vector2[tilesInSet.Length];
        for (int i = 0; i < tilesInSet.Length; ++i)
        {
            tiles[i] = tilesInSet[i].transform.position;
        }

        logic.SelectAttackTargets(tiles);

        foreach (AttackTileSelect tileSelect in FindObjectsOfType<AttackTileSelect>())
        {
            Destroy(tileSelect.gameObject);
        }
    }

}
