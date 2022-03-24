using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zanespace;

public class UnitController : MonoBehaviour, IComparable
{
    public MoveLogic move
    {
        private set
        {
            _move = value;
        }
        get
        {
            return _move;
        }
    }
    [SerializeField] MoveLogic _move;
    protected ActionLogic[] actions;
    //protected AttackLogic attack; //Deprecated 
    //protected DeathLogic deathEffects;

    [SerializeField] int maxHP;
    public float speed;
    int hp;
    [System.NonSerialized] public float actionPoints;
    public Direction facing;

    public event Alert onMove;
    public event Alert onEndTurn;

    void Start()
    {
        hp = maxHP;
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
        print(References.actionSelectMenu.name);
        print(actions.Length);
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

    public void TakeDamage(int dmg)
    {
        print("AH! I'm hit for " + dmg + "-" + name);
        hp -= dmg;
        if (hp == 0)
        {

        }
        else if (hp < 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            References.uManager.ReloadUnits();
        }
    }
}
