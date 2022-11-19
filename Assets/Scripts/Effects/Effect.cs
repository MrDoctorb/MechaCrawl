using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class Effect
{
    public abstract void ApplyTo(UnitController unit);
    public abstract string Description();
}
