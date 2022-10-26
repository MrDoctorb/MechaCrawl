using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Make Rewire work for enemies too
[System.Serializable]
public class Rewire : Effect
{
    public override void ApplyTo(UnitController unit)
    {
        if (unit.hp == 0 && unit is EnemyController)
        {
            UnitController newUnit = unit.gameObject.AddComponent<UnitController>();

            //Transfer values
            newUnit.maxHP = unit.maxHP;
            newUnit.speed = unit.speed;
            newUnit.facing = unit.facing;

            TileManager.TileAt(newUnit.transform.position).Enter(newUnit);

            unit.GetComponentInChildren<HealthDisplay>().enabled = false;

            Object.Destroy(unit);

            //You can stay, for now
            newUnit.StartCoroutine(StupidFunction(newUnit));

        }
    }

    //Need to do this after other stuff initializes itself. 
    //I feel like there's a better way to do this but this will have to do for now

    //Can't I just make a reset function that has the functionality of the turn off turn on? That seems easier, will get to it soonish
    //Well, It seems that the lighting only works on a following frame
    IEnumerator StupidFunction(UnitController newUnit)
    {
        yield return new WaitForEndOfFrame();
        newUnit.GetComponentInChildren<HealthDisplay>().enabled = true;
        newUnit.SetHealth(newUnit.maxHP /2);


        //VERY TEMPORARY, GET RID OF THIS LATER
        //newUnit.transform.gameObject.AddComponent<LightSource>().brightness = 3;
    }

    public override string Description()
    {
        return "revives mechanical units at 0 hp as an ally and restores them to half hp";
    }
}
