using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zanespace;

public class UnitManager : MonoBehaviour
{
    List<UnitController> allUnits = new List<UnitController>();
    List<UnitController> nextUnits;
    [SerializeField] List<GameObject> possibleEnemies = new List<GameObject>();
    UnitController mostRecentAlly;
    //I do not currently have code to dynamically generate more containters,
    //so leave this at 5 for now unless you change that
    int unitsToDisplay = 5;

    public event Alert onTurnStart;

    private void Awake()
    {
        References.uManager = this;
    }

    void Start()
    {
        //Set inital ally to the player
        mostRecentAlly = allUnits[0];
        CalculateNextUnits();
    }

    public void NewFloorUnitSetup()
    {
        DeleteEnemies();
        SpawnEnemies();
        PlaceUnits();
    }

    void SpawnEnemies()
    {
        Instantiate(possibleEnemies[Random.Range(0, possibleEnemies.Count)], Vector2.zero, Quaternion.identity);
        Instantiate(possibleEnemies[Random.Range(0, possibleEnemies.Count)], Vector2.zero, Quaternion.identity);
        Instantiate(possibleEnemies[Random.Range(0, possibleEnemies.Count)], Vector2.zero, Quaternion.identity);
    }

    void DeleteEnemies()
    {
        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
        {
            Destroy(enemy.gameObject);
        }
    }

    public void PlaceUnits()
    {
        foreach (UnitController unit in allUnits)
        {
            unit.EnterLevel();
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

    /// <summary>
    /// Calculates the upcoming turn order
    /// </summary>
    void CalculateNextUnits()
    {
        //Stops the process if ther are no units
        if (allUnits.Count <= 0)
        {
            Debug.LogError("There are no units to calculate");
            return;
        }

        //Resets the output list
        nextUnits = new List<UnitController>();

        //Lists for calculating
        List<UnitController> tempList = new List<UnitController>(allUnits);
        Dictionary<UnitController, int> occurrenceList = new Dictionary<UnitController, int>();

        //Modifier increases if there aren't enough units to fill the list,
        //continues to calculate the next theoretical turns
        int modifier = 0;
        do
        { 
            //Assume that no one has taken an action
            bool noActions = true;

            foreach (UnitController unit in tempList)
            {
                //The number of times this unit has appeared in the upcoming list
                int occurrences = 0;
                if (occurrenceList.ContainsKey(unit))
                {
                    occurrences = occurrenceList[unit];
                }

                if ((unit.actionPoints - (References.ACTIONTHRESHOLD * occurrences)) +
                   ((unit.speed / tempList.Count) * modifier) >= References.ACTIONTHRESHOLD)
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
                    break;
                }
            }
            if (noActions)
            {
                ++modifier;
            }

        } while (nextUnits.Count < unitsToDisplay);

        References.TurnOrderVisualizer.UpdateVisuals(nextUnits);
    }
    void NextTurn()
    {
        if (nextUnits[0].actionPoints >= References.ACTIONTHRESHOLD)
        {
            if (!(nextUnits[0] is EnemyController))
            {
                mostRecentAlly = nextUnits[0];
            }

            onTurnStart?.Invoke();
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

        CalculateNextUnits();
    }

    private IEnumerator BetweenTurns()
    {
        // yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.5f);
        foreach (UnitController unit in allUnits)
        {
            unit.actionPoints += unit.speed / AmountOfUnits();
        }
        NextTurn();
    }

    public UnitController GetMostRecentAlly()
    {
        return mostRecentAlly;
    }

    public UnitController GetMostRecentUnit()
    {
        return nextUnits[0];
    }
}
