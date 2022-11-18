using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternLogic
{
    public abstract Vector2[][] Pattern(Vector2 startPos);

    public abstract string Description();
}
