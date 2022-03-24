using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileSelect : MonoBehaviour
{
    public MoveLogic logic;

    private void OnMouseDown()
    {
        logic.SelectMoveTarget(transform.position);
        foreach(MoveTileSelect tileSelect in FindObjectsOfType<MoveTileSelect>())
        {
            Destroy(tileSelect.gameObject);
        }
    }
}
