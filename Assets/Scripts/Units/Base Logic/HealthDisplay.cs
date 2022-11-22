using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthDisplay : MonoBehaviour
{
    // Text text;
    Slider slider;
    UnitController unit;

    Transform segmentContainer;
    GameObject segment;

    void OnEnable()
    {
        if (unit == null)
        {
            unit = GetComponentInParent<UnitController>();
        }
        if (slider == null)
        {
            slider = GetComponent<Slider>();
            slider.maxValue = unit.maxHP;
        }
        if(segmentContainer == null)
        {
            SegmentSetup();
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
        slider.value = unit.hp;
    }

    void SetVisibility()
    {
        if (unit.visible)
        {
            slider.gameObject.SetActive(true);
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }

    void SegmentSetup()
    {
        segmentContainer = transform.GetComponentInChildren<HorizontalLayoutGroup>().transform;
        segment = segmentContainer.GetChild(0).gameObject;
        for(int i = 1; i < unit.maxHP; i++)
        {
            Instantiate(segment, segmentContainer);
        }

        if (unit.maxHP > 10)
        {
            segmentContainer.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
        }
    }
}
