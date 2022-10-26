using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTile : Tile
{
    bool active = true;
    [SerializeField] Sprite usedSprite;
    public override void EnterEffect()
    {
        if(active)
        {
            unit.Heal(1);
        }
    }

    public override void ExitEffect()
    {
        if(active)
        {
            active = false;
            GetComponent<SpriteRenderer>().sprite = usedSprite;
        }
    }

    public override void StopEffect()
    {
        if(active)
        {
            unit.Heal(1);
        }
    }
}
