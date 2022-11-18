using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Effect
{
    [SerializeField] int healAmount;
    public override void ApplyTo(UnitController unit)
    {
        unit.Heal(healAmount);
    }

    public override string Description()
    {
        return "heals for " + healAmount;
    }
}
