using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public string statusName;
    [SelectType][SerializeReference]Effect[] effects;
    public UnitController target;
    public int duration;

    private void Start()
    {
        
    }

    void Apply()
    {

    }
}

public enum AlertType
{
    TurnStart,
    TurnEnd,
    MoveStart,
    MoveEnd
}