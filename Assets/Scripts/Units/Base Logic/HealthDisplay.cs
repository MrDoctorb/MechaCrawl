using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthDisplay : MonoBehaviour
{
    Text text;
    UnitController unit;

    void OnEnable()
    {
        if(unit == null)
        {
            unit = GetComponentInParent<UnitController>();
        }
        if(text == null)
        {
            text = GetComponent<Text>();
        }

        unit.onVisibilityChange += SetVisibility;
        unit.onHealthChange += DisplayHealth;
        DisplayHealth();
    }

    void OnDisable()
    {
        unit.onHealthChange -= DisplayHealth;
    }

    void DisplayHealth()
    {
        text.text = unit.hp.ToString();
    }

    void SetVisibility()
    {
        if(unit.visible)
        {
            text.color = Color.white;
        }
        else
        {
            text.color  = Color.clear;
        }
    }
}
