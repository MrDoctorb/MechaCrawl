using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Damage : Effect
{
    [SerializeField] int damage;
    public override void ApplyTo(UnitController unit)
    {
        unit.TakeDamage(damage);
    }

    public override string Description()
    {
        return "deals " + damage + " damage";
    }
}
