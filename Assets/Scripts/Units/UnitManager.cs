using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zanespace;

public class UnitManager : MonoBehaviour
{
    UnitController[] allUnits;
    List<UnitController> nextUnits;
    [SerializeField] Text tempTurnOrderDisplay;

    void Start()
    {
        References.uManager = this;
        ReloadUnits();
        CalculateNextUnits();
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
        return allUnits.Length;
    }

    public void ReloadUnits()
    {

        allUnits = FindObjectsOfType<UnitController>();
    }

    void CalculateNextUnits()
    {
        /* nextUnits = new List<UnitController>();
         List<UnitController> tempList = new List<UnitController>(allUnits);

         tempList.Sort();
         if(tempList.Count >= 5)
         {
             nextUnits = tempList.GetRange(0, 5);
         }
         else
         {
             nextUnits = tempList.GetRange(0, tempList.Count);
         }

         foreach (UnitController unit in tempList)
         {
             //print(unit.name + " Turns till Go: " + unit.TurnsTillNextAction());
         }*/

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
            print(nextUnits[0].name);
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
