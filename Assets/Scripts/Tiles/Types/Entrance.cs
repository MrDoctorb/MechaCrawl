using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : Tile
{
    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public override void EnterEffect()
    {
    }

    public override void ExitEffect()
    {
     
    }

    public override void StopEffect()
    {
        if (unit.GetType() != typeof(EnemyController))
        {
            print("I go UP! ^-^");
        }
    }

}
