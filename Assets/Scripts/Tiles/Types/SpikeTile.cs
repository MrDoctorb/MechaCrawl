using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;
public class SpikeTile : Tile
{
    public override void EnterEffect()
    {
        if(unit.move.GetMoveType() == MoveType.Fly)
        {
            return;
        }
        if (unit.move.GetMoveType() == MoveType.Jump)
        {
            unit.onEndMoveTotal += Hurt;
        }
        else
        {
            Hurt();
        }
    }

    public override void ExitEffect()
    {
        if(unit.move.GetMoveType() == MoveType.Jump)
        {
            unit.onEndMoveTotal -= Hurt;
        }
    }

    public override void StopEffect()
    {
        if (unit.move.GetMoveType() == MoveType.Jump)
        {
            unit.onEndMoveTotal -= Hurt;
        }
    }

    void Hurt()
    {
        unit.TakeDamage(1);
    }
}
