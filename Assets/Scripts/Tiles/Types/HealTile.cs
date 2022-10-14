using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTile : Tile
{
    public override void EnterEffect()
    {
        unit.Heal(1);
    }

    public override void ExitEffect()
    {
    }

    public override void StopEffect()
    {
        unit.Heal(1);
    }
}
