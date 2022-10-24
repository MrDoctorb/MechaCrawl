using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternLogic : MonoBehaviour
{
    public abstract Vector2[] Pattern(Vector2 startPos);
}
