using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyStatus : Effect
{
    [SelectType] [SerializeReference] Effect[] effects;
    [SerializeField] int occurrences;
    [SerializeField] AlertType triggerCondition;
    [SerializeField] AlertType decrementCondition;

    public override void ApplyTo(UnitController unit)
    {
        Status status = unit.gameObject.AddComponent<Status>();
        status.Setup(effects, occurrences, triggerCondition, decrementCondition);
    }

    string TriggerText()
    {
        switch (triggerCondition)
        {
            case AlertType.TurnStart:
                return "at the start of their turn";
            case AlertType.TurnEnd:
                return "at the end of their turn ";
            case AlertType.MoveStart:
                return "before moving to a tile ";
            case AlertType.MoveEnd:
                return "after moving to a tile ";
            default:
                return "how did you manage to do this";
        }
    }

    string DecrementText()
    {
        switch (triggerCondition)
        {
            case AlertType.TurnStart:
            case AlertType.TurnEnd:
                return $"for the next {occurrences} turns";
            case AlertType.MoveStart:
            case AlertType.MoveEnd:
                return $"for the next {occurrences} tiles moved";
            default:
                return "how did you manage to do this";
        }
    }

    public override string Description()
    {
        string output = "";

        output += DecrementText();
        output += " ";
        output += TriggerText();

        for (int i = 0; i < effects.Length; i++)
        {
            output += $"{effects[i].Description()}";

            if (i < effects.Length -1)
            {
                output += "and ";
            }
        }

        return output;
    }
}
