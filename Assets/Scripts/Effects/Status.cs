using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    Effect[] effects;
    UnitController target;
    int occurrences;
    AlertType triggerCondition;
    AlertType decrementCondition;

    public void Setup(Effect[] effects, int occurrences, AlertType trigger, AlertType decrement)
    {
        this.effects = effects;
        this.occurrences = occurrences;
        triggerCondition = trigger;
        decrementCondition = decrement;
        target = GetComponent<UnitController>();
        SubscribeFunction(ApplyEffects, triggerCondition);
        SubscribeFunction(Decrement, decrementCondition);
    }

    private void OnDestroy()
    {
        UnsubscribeFunction(ApplyEffects, triggerCondition);
        UnsubscribeFunction(Decrement, decrementCondition);
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
            Destroy(this);
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
                target.onStartMoveSingle += method.Invoke;
                break;
            case AlertType.MoveEnd:
                target.onEndMoveSingle += method.Invoke;
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
                target.onStartMoveSingle -= method.Invoke;
                break;
            case AlertType.MoveEnd:
                target.onEndMoveSingle -= method.Invoke;
                break;
        }
    }
/*

    public override string Description()
    {
        throw new System.NotImplementedException();
    }*/
}

public enum AlertType
{
    TurnStart,
    TurnEnd,
    MoveStart,
    MoveEnd
}