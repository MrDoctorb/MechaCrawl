using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zanespace;

public class UnitManager : MonoBehaviour
{
    List<UnitController> allUnits = new List<UnitController>();
    List<UnitController> nextUnits;
    [SerializeField] Text tempTurnOrderDisplay;

    private void Awake()
    {
        References.uManager = this;
    }

    void Start()
    {
        PlaceUnits();
        CalculateNextUnits();
    }

    void PlaceUnits()
    {
        foreach(UnitController unit in allUnits)
        {
            unit.EnterLevel();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }

    public int AmountOfUnits()
    {
        return allUnits.Count;
    }

    public void AddUnit(UnitController unit)
    {
        if (allUnits.Contains(unit))
        {
            Debug.LogError(unit.name + " is already in the unit list");
        }
        else
        {
            allUnits.Add(unit);
            print(unit.name + " added");
        }
    }

    public void RemoveUnit(UnitController unit)
    {
        if (allUnits.Contains(unit))
        {
            allUnits.Remove(unit);
        }
        else
        {
            Debug.LogError(unit.name + " was not in the unit list");
        }
    }

    void CalculateNextUnits()
    {
        if(allUnits.Count <= 0)
        {
            Debug.LogError("There are no units to calculate");
            return;
        }

        string testOutput = "";
        nextUnits = new List<UnitController>();
        List<UnitController> tempList = new List<UnitController>(allUnits);
        Dictionary<UnitController, int> occurrenceList = new Dictionary<UnitController, int>();
        int modifier = 0;
        do
        {
            tempList.Sort();
            bool noActions = true;
            foreach (UnitController unit in tempList)
            {
                int occurrences = 0;
                if (occurrenceList.ContainsKey(unit))
                {
                    occurrences = occurrenceList[unit];
                }
                if ((unit.actionPoints - (References.ACTIONTHRESHOLD * occurrences)) +
                   ((unit.speed / tempList.Count) * modifier) > References.ACTIONTHRESHOLD)
                {
                    nextUnits.Add(unit);
                    noActions = false;
                    if (!occurrenceList.ContainsKey(unit))
                    {
                        occurrenceList.Add(unit, 1);
                    }
                    else
                    {
                        occurrenceList[unit] += 1;
                    }

                    testOutput += unit.name + "\n";


                    //testOutput += unit is EnemyController ? " (Enemy)" + "\n" : testOutput += "\n";
                    break;
                }
            }
            if (noActions)
            {
                ++modifier;
            }

        } while (nextUnits.Count < 5);

        tempTurnOrderDisplay.text = testOutput;
    }
    void NextTurn()
    {
        if (nextUnits[0].actionPoints > References.ACTIONTHRESHOLD)
        {
            nextUnits[0].actionPoints -= References.ACTIONTHRESHOLD;
            nextUnits[0].StartTurn();
        }
        else
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        StartCoroutine(BetweenTurns());
    }

    private IEnumerator BetweenTurns()
    {
        yield return new WaitForEndOfFrame();
        foreach (UnitController unit in allUnits)
        {
            unit.actionPoints += unit.speed / AmountOfUnits();
        }
        CalculateNextUnits();
        NextTurn();
    }


}
