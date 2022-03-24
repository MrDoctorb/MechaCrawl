using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Tile
{
    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public override void EnterEffect()
    {
    }

    public override void ExitEffect()
    {
    }

    public override void StopEffect()
    {
        if(unit.GetType() != typeof(EnemyController))
        {
            print("I'm going Down Town “w”");
        }
    }

}
