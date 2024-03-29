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

    public int maxHP;
    public float speed;
    public int hp { private set; get; }
    [NonSerialized] public float actionPoints;
    public bool visible { private set; get; }
    public Direction facing;


    //onMove Alerts called for movement between every tile
    public event Alert onStartMoveSingle;
    public event Alert onEndMoveSingle;
    public event Alert onStartMoveTotal;
    public event Alert onEndMoveTotal;
    public event Alert onStartTurn;
    public event Alert onEndTurn;
    public event Alert onTakeDamage;
    public event Alert onDeath;
    public event Alert onHealthChange;
    public event Alert onVisibilityChange;

    protected SpriteRenderer rend;

    private void OnEnable()
    {
        rend = GetComponent<SpriteRenderer>();
        GetComponent<Animator>().enabled = true;
        if (gameObject.name.Contains("(Clone)"))
        {
            gameObject.name = gameObject.name.Replace("(Clone)", "");
        }
        if(GetComponent<LightSource>())
        {
            SetVisibility(true);
        }


        References.uManager.AddUnit(this);
    }

    private void OnDisable()
    {
        References.uManager.RemoveUnit(this);
    }

    protected virtual void Start()
    {
        SetHealth(maxHP);
        move = GetComponent<MoveLogic>();
        actions = FindActions();

        foreach (ActionLogic logic in GetComponents<ActionLogic>())
        {
            logic.myUnit = this;
        }


        //attack = GetComponent<AttackLogic>(); //Deprecated
        //deathEffects = GetComponents<Effect>();
        //EnterLevel();
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

    public virtual void EnterLevel()
    {
        //This needs to be super optimized for multiple people
        //MoveToTile(FindObjectOfType<Entrance>().transform.position);
        transform.position = FindObjectOfType<Entrance>().transform.position;
        TileManager.TileAt(transform.position).UpdateLighting();
    }

    public virtual void StartTurn()
    {
        CameraController.instance.NewFocus(transform.position);
        onStartTurn?.Invoke();
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

    public IEnumerator MovePath(Vector2[] path)
    {
        onStartMoveTotal?.Invoke();
        foreach (Vector2 pos in path)
        {
            MoveToTile(pos);

            //This also shouldn't be hard coded I don't think
            yield return new WaitForSeconds(References.timeBetweenMove);
        }
        onEndMoveTotal?.Invoke();
    }

    public virtual void MoveToTile(Vector2 pos)
    {
        if (TileManager.IsTileOpen(pos))
        {
            //Only invoke onMove if something is subscribed to it
            onStartMoveSingle?.Invoke();
            TileManager.TileAt(pos).Enter(this);
            onEndMoveSingle?.Invoke();
        }
    }


    //Flashes between red and white quickly
    IEnumerator DamageAnim()
    {
        float totalTime = .75f;
        float loops = 2;
        float timer;
        int countingLoops = 0;
        while (countingLoops < loops)
        {
            timer = 0;
            //Go to Red
            while(timer < (totalTime/loops)/2)
            {
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
                rend.color = Color.Lerp(Color.white, Color.red, timer/(totalTime/loops/2));
            }
            //Go back
            while(timer < totalTime/loops)
            {
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
                rend.color = Color.Lerp(Color.red, Color.white, (timer-(totalTime/loops/2))/(totalTime/loops/2));
            }
            yield return new WaitForEndOfFrame();
            countingLoops++;
        }
        rend.color = Color.white;

    }

    /// <summary>
    /// Causes the target to take an amount of damage.
    /// Effects that trigger when damage is taken trigger after damage is dealt
    /// and after checking if the unit has died.
    /// A dead unit does not trigger damage effects.
    /// </summary>
    /// <param name="dmg">The amount of damage to be dealt</param>
    public void TakeDamage(int dmg)
    {
        if (dmg < 0)
        {
            dmg = 0;
        }
        StartCoroutine(DamageAnim());
        SetHealth(hp - dmg);
        onTakeDamage?.Invoke();
    }

    public void Heal(int amount)
    {
        //Heal always does at least one point
        if (amount < 1)
        {
            amount = 1;
        }
        SetHealth(hp + amount);
    }

    public void SetHealth(int amount)
    {
        hp = amount;
        if (hp > maxHP)
        {
            hp = maxHP;
        }
        if (hp == 0)
        {
            if (this is EnemyController)
            {
                enabled = false;
                GetComponent<Animator>().enabled = false;
            }
            else
            {
                hp = -1;
            }
        }
        if (hp < 0)
        {
            onDeath?.Invoke();
            References.uManager.CheckLossState(this);
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        onHealthChange?.Invoke();
    }

    public void SetVisibility(bool visible)
    {
        this.visible = visible;
        if(rend != null)
        {
            rend.enabled = visible;
        }
        onVisibilityChange?.Invoke();
    }
}
