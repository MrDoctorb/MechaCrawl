using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTileSelect : MonoBehaviour
{
    public ActionLogic logic;

    public Color color;
    SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        color.a = .5f;
        rend.color = color;
    }

    private void OnMouseDown()
    {
        logic.SelectTarget(transform.position);
        foreach (SingleTileSelect tileSelect in FindObjectsOfType<SingleTileSelect>())
        {
            Destroy(tileSelect.gameObject);
        }
    }
    private void OnMouseEnter()
    {
        rend.color = color + new Color(.25f, .25f, .25f, 0);

    }

    private void OnMouseExit()
    {
        rend.color = color;

    }


}
