using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public abstract void ApplyTo(UnitController unit);
    public abstract string Description();
}
