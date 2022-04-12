using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTileSelect : MonoBehaviour
{
    public ActionLogic logic;

    private void OnMouseDown()
    {
        logic.SelectTarget(transform.position);
        foreach(SingleTileSelect tileSelect in FindObjectsOfType<SingleTileSelect>())
        {
            Destroy(tileSelect.gameObject);
        }
    }
}
