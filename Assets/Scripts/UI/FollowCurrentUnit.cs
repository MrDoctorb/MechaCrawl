using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class FollowCurrentUnit : MonoBehaviour
{
    UnitManager manager;
    UnitController target;
    SpriteRenderer rend;
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        manager = References.uManager;
        manager.onTurnStart += UpdateTarget;
    }

    void UpdateTarget()
    {
        if(target != null)
        {

            target.onEndMoveSingle -= MoveTo;
            target.onStartTurn -= MoveTo;
        }

        target = manager.GetMostRecentUnit();
        target.onEndMoveSingle += MoveTo;
        target.onStartTurn += MoveTo;

    }

    void MoveTo()
    {
        transform.position = target.transform.position;
        if(target.visible)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
    }
}
