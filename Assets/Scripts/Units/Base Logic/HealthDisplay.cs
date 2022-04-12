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

        unit.onHealthChange += DisplayHealth;
    }

    void OnDisable()
    {
        unit.onHealthChange -= DisplayHealth;
    }

    void DisplayHealth()
    {
        text.text = unit.hp.ToString();
    }

}
