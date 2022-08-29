using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public abstract class Tile : MonoBehaviour
{
    public TileType type;

    public UnitController unit;

    public abstract void EnterEffect();

    public abstract void StopEffect();

    public abstract void ExitEffect();

    public void Enter(UnitController newUnit)
    {
        unit = newUnit;
        unit.transform.position = transform.position;
        unit.onMove += Exit;
        unit.onEndTurn += StopEffect;

        EnterEffect();
    }

    public void Exit()
    {
        ExitEffect();


        unit.onEndTurn -= StopEffect;
        unit.onMove -= Exit;
        unit = null;
    }

    public virtual bool IsOpen()
    {
        return unit == null;
    }

    public bool CanStopHere(MoveType moveType)
    {
        if (!IsOpen() || type == TileType.BlockAll)
        {
            return false;
        }

        switch (moveType)
        {
            case MoveType.Fly:
                return type != TileType.BlockFly;
            case MoveType.Walk:
            case MoveType.Jump:
            default: //If its not one of those three its teleport
                return type != TileType.BlockWalk;
        }
    }

    public bool CanPassThrough(MoveType moveType)
    {
        if(type == TileType.BlockAll && moveType != MoveType.Teleport)
        {
            return false;
        }

        switch (moveType)
        {
            case MoveType.Fly:
                return type != TileType.BlockFly;
            case MoveType.Walk:
                return type != TileType.BlockWalk; //&& IsOpen();
            default://Teleport and Jump can pass through any space (Except jump can't go through walls
                return true;

        }
    }

    void OnDestroy()
    {
        if(unit != null)
        {
            unit.onEndTurn -= StopEffect;
            unit.onMove -= Exit;
        }
    }
}
