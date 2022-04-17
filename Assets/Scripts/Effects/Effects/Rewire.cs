using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Make Rewire work for enemies too
public class Rewire : Effect
{
    public override void ApplyTo(UnitController unit)
    {
        if (unit.hp == 0 && unit is EnemyController)
        {
            UnitController newUnit = unit.gameObject.AddComponent<UnitController>();

            //Transfer values
            newUnit.maxHP = unit.maxHP;
            newUnit.Heal(newUnit.maxHP /2);
            newUnit.speed = unit.speed;
            newUnit.facing = unit.facing;

          //  TileManager.TileAt(newUnit.transform.position).Enter(newUnit);


            unit.GetComponentInChildren<HealthDisplay>().enabled = false;
            Destroy(unit);
            newUnit.GetComponentInChildren<HealthDisplay>().enabled = true;
        }
    }
}
