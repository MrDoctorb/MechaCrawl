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
    private float lightLevel = 0;

   [SelectType] [SerializeReference] Effect[] enterEffects = new Effect[0], stopEffects = new Effect[0], exitEffects = new Effect[0];

    public virtual void EnterEffect()
    {
        foreach (Effect effect in enterEffects) 
        {
            print("B");
            effect.ApplyTo(unit);
            print("C");
        }
    }

    public virtual void StopEffect()
    {
        foreach(Effect effect in stopEffects)
        {
            effect.ApplyTo(unit);
        }
    }

    public virtual void ExitEffect()
    {
        foreach(Effect effect in  exitEffects)
        {
            effect.ApplyTo(unit);
        }
    }

    void OnEnable()
    {
        rend = GetComponent<SpriteRenderer>();
        lightSources = new List<LightSource>();
    }

    void Start()
    {
        SetVisibility(0);
        rend.sortingOrder = -(int)transform.position.y;

    }

    public void Enter(UnitController newUnit)
    {
        unit = newUnit;
        unit.transform.position = transform.position;
        unit.onStartMoveSingle += Exit;
        unit.onEndTurn += StopEffect;
        unit.onDeath += ClearUnit;

        if (lightLevel > .2f)
        {
            unit.SetVisibility(true);
        }
        else
        {
            unit.SetVisibility(false);
        }

        EnterEffect();
    }

    public void Exit()
    {
        ExitEffect();


        unit.onEndTurn -= StopEffect;
        unit.onStartMoveSingle -= Exit;
        unit.onDeath -= ClearUnit;
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
            unit.onStartMoveSingle -= Exit;
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
            if(source != null && transform != null)
            {
                float newLight = (Mathf.Abs(source.brightness -Functions.GridDistance(source.transform.position, transform.position))
                    /(float)source.brightness) + .2f;
                if (newLight > brightest)
                {
                    brightest = newLight;
                }
            }
            else
            {
                Debug.LogWarning("Hey you have a null Light source happening");
            }
        }
        if (brightest != lightLevel)
        {
            SetVisibility(brightest);
            onLightChange?.Invoke();
        }
        if (unit is EnemyController)
        {
            if (brightest > .2f)
            {
                unit.SetVisibility(true);
            }
            else
            {
                unit.SetVisibility(false);
            }

        }

    }

    /// <summary>
    /// Set the visibility of the given tile
    /// </summary>
    /// <param name="percentage">1 is fully visible, 0 is invisible</param>
    protected void SetVisibility(float percentage)
    {
        if (percentage > 1)
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1);
        }
        else
        {
            try
            {

                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, percentage);
            }
            catch
            {
                //print("huh");
            }
        }

        lightLevel = percentage;
    }

    void ClearUnit()
    {
        unit = null;
    }
}
