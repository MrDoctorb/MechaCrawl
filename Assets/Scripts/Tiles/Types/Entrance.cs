using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : Tile
{ 
    public override void StopEffect()
    {
        if (unit.GetType() != typeof(EnemyController))
        {
            print("I go UP! ^-^");
        }
    }

}
