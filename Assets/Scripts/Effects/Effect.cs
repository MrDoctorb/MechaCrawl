using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public virtual void ApplyTo(UnitController unit)
    {
        throw new System.Exception("AA");
    }
    public virtual string Description()
    {
        throw new System.Exception("AA");
    }
}
