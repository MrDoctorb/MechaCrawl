using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionLogic : MonoBehaviour
{
    public Sprite icon;
    public string actionName;
    [System.NonSerialized] public UnitController myUnit;
    public abstract void Perform();
    public abstract void SelectTargets(Vector2[] targets);
    public void SelectTarget(Vector2 pos)
    {
        SelectTargets(new Vector2[] { pos });
    }
}
