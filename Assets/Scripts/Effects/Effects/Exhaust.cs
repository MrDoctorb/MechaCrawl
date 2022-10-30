using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Exhaust : Effect
{
    [SerializeField] float amount;
    public override void ApplyTo(UnitController unit)
    {
        unit.actionPoints -= amount;
    }

    public override string Description()
    {
        return "reduced turn order by " + amount + " points";
    }
}
