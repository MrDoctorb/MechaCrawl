using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class Exit : Tile
{
    public override void StopEffect()
    {
        if(unit.GetType() != typeof(EnemyController))
        {
            print("I'm going Down Town “w”");
            References.tManager.NewFloor();
        }
    }

}
