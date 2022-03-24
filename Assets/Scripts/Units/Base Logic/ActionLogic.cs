using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionLogic : MonoBehaviour
{
    public Sprite icon;
    public string actionName;
    public abstract void Perform();
}
