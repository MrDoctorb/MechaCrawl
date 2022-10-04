using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public abstract class Tile : MonoBehaviour
{
    public TileType type;

    public UnitController unit;

    public Alert onLightChange;
    [System.NonSerialized] public List<LightSource> lightSources;
    protected SpriteRenderer rend;

    public abstract void EnterEffect();

    public abstract void StopEffect();

    public abstract void ExitEffect();

    void OnEnable()
    {
        lightSources = new List<LightSource>();
    }

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        SetVisibility(0);
    }

    public void Enter(UnitController newUnit)
    {
        unit = newUnit;
        unit.transform.position = transform.position;
        unit.onStartMove += Exit;
        unit.onEndTurn += StopEffect;

        EnterEffect();
    }

    public void Exit()
    {
        ExitEffect();


        unit.onEndTurn -= StopEffect;
        unit.onStartMove -= Exit;
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
        if (type == TileType.BlockAll && moveType != MoveType.Teleport)
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

    public bool CanSeeThrough()
    {
        if (type == TileType.BlockAll)
        {
            return false;
        }

        return true;
    }

    void OnDestroy()
    {
        if (unit != null)
        {
            unit.onEndTurn -= StopEffect;
            unit.onStartMove -= Exit;
        }
    }

    public bool IsLit()
    {
        return lightSources.Count > 0;
    }

    public void UpdateLighting()
    {
        //Default value is Explored
        float brightest = .2f;
        foreach (LightSource source in lightSources)
        {
            float newLight = (Mathf.Abs(source.brightness -Functions.GridDistance(source.transform.position, transform.position))
                /(float)source.brightness) + .2f;
            if (newLight > brightest)
            {
                brightest = newLight;
            }
        }
        if (brightest != rend.color.a)
        {
            SetVisibility(brightest);
            onLightChange?.Invoke();
        }
        if (unit is EnemyController)
        {
            if (brightest > .2f)
            {
                unit.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                unit.GetComponent<SpriteRenderer>().enabled = false;
            }

        }

    }

    /// <summary>
    /// Set the visibility of the given tile
    /// </summary>
    /// <param name="percentage">1 is fully visible, 0 is invisible</param>
    protected void SetVisibility(float percentage)
    {
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, percentage);
    }
}
