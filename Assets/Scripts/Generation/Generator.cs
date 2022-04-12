using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    public abstract DungeonTile[,] Generate();
}
