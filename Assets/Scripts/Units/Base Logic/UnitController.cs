using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

[RequireComponent(typeof(MoveLogic))]
public class UnitController : MonoBehaviour, IComparable
{
    public MoveLogic move { private set; get; }
    protected ActionLogic[] actions;
    //protected AttackLogic attack; //Deprecated 
    //protected DeathLogic deathEffects;

    [SerializeField] int maxHP;
    public float speed;
    public int hp { private set; get; }
    [System.NonSerialized] public float actionPoints;
    public Direction facing;

    public event Alert onMove;
    public event Alert onEndTurn;
    public event Alert onTakeDamage;
    public event Alert onHealthChange;

    private void OnEnable()
    {
        References.uManager.AddUnit(this);
    }

    private void OnDisable()
    {
        References.uManager.RemoveUnit(this);
    }

    void Start()
    {
        SetHealth(maxHP);
        move = GetComponent<MoveLogic>();
        actions = FindActions();


        //attack = GetComponent<AttackLogic>(); //Deprecated
        //deathEffects = GetComponents<Effect>();
        EnterLevel();
    }

    ActionLogic[] FindActions()
    {
        List<ActionLogic> tempActions = new List<ActionLogic>();
        foreach (ActionLogic potentialAction in GetComponents<ActionLogic>())
        {
            if (potentialAction != move)
            {
                tempActions.Add(potentialAction);
            }
        }
        return tempActions.ToArray();
    }

    protected virtual void EnterLevel()
    {
        //This needs to be super optimized for multiple people
        //MoveToTile(FindObjectOfType<Entrance>().transform.position);
        transform.position = FindObjectOfType<Entrance>().transform.position;
    }

    public virtual void StartTurn()
    {
        move.Perform();
    }

    public virtual void SelectAction()
    {
        References.actionSelectMenu.Display(actions, EndTurn);
        //attack.Perform();
    }

    public void EndTurn()
    {
        onEndTurn?.Invoke();
        References.uManager.EndTurn();
    }

    public int TurnsTillNextAction()
    {
        return (int)((References.ACTIONTHRESHOLD - actionPoints) * References.uManager.AmountOfUnits() / speed);
    }

    public int CompareTo(object obj)
    {
        try
        {
            UnitController tempUnit = (UnitController)obj;
            return TurnsTillNextAction().CompareTo(tempUnit.TurnsTillNextAction());
        }
        catch
        {
            throw new Exception("That's not a Unit >[  It's " + obj);
        }
    }

    public void MoveToTile(Vector2 pos)
    {
        if (TileManager.IsTileOpen(pos))
        {
            //Only invoke onMove if something is subscribed to it
            onMove?.Invoke();
            TileManager.TileAt(pos).Enter(this);
        }
    }

    /// <summary>
    /// Causes the target to take an amount of damage.
    /// Effects that trigger when damage is taken trigger after damage is dealt
    /// and after checking if the unit has died.
    /// A dead unit does not trigger damage effects.
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(int dmg)
    {
        if (dmg < 0)
        {
            dmg = 0;
        }

        print("AH! I'm hit for " + dmg + "-" + name);
        SetHealth(hp - dmg);

        if (hp == 0)
        {

            return;
        }
        else if (hp < 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        onTakeDamage?.Invoke();
    }

    void Heal(int amount)
    {
        SetHealth(hp + amount);
    }

    void SetHealth(int amount)
    {

        hp = amount;
        if (hp > maxHP)
        {
            hp = maxHP;
        }
        onHealthChange?.Invoke();
    }
}
