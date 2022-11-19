using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : Effect
{
    [SelectType] [SerializeReference] Effect[] effects;
    UnitController target;
    [SerializeField] int occurrences;
    [SerializeField] AlertType triggerCondition;
    [SerializeField] AlertType decrementCondition;

    public override void ApplyTo(UnitController unit)
    {
        target = unit;
        SubscribeFunction(ApplyEffects, triggerCondition);
        SubscribeFunction(Decrement, decrementCondition);
    }

    void ApplyEffects()
    {
        foreach (Effect effect in effects)
        {
            effect.ApplyTo(target);
        }
    }

    void Decrement()
    {
        occurrences--;
        if(occurrences <= 0)
        {
            
        }
    }

    void SubscribeFunction(Action method, AlertType alert)
    {
        switch (alert)
        {
            case AlertType.TurnStart:
                target.onStartTurn += method.Invoke;
                break;
            case AlertType.TurnEnd:
                target.onEndTurn += method.Invoke;
                break;
            case AlertType.MoveStart:
                target.onStartMove += method.Invoke;
                break;
            case AlertType.MoveEnd:
                target.onEndMove += method.Invoke;
                break;
        }
    }

    void UnsubscribeFunction(Action method, AlertType alert)
    {
        switch (alert)
        {
            case AlertType.TurnStart:
                target.onStartTurn -= method.Invoke;
                break;
            case AlertType.TurnEnd:
                target.onEndTurn -= method.Invoke;
                break;
            case AlertType.MoveStart:
                target.onStartMove -= method.Invoke;
                break;
            case AlertType.MoveEnd:
                target.onEndMove -= method.Invoke;
                break;
        }
    }


    public override string Description()
    {
        throw new System.NotImplementedException();
    }
}

public enum AlertType
{
    TurnStart,
    TurnEnd,
    MoveStart,
    MoveEnd
}