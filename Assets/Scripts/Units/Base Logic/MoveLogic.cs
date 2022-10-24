using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zanespace;

public abstract class MoveLogic : ActionLogic
{
    [SerializeField] protected int minRange, maxRange;
    [SerializeField] protected MoveType moveType;

    //Probably just change this to a color value at some point
    public GameObject spaceSelect;

    public override void Perform()
    {
        foreach(Vector2 pos in ValidTiles())
        {
            Instantiate(spaceSelect, pos, Quaternion.identity).GetComponent<SingleTileSelect>().logic = this;
        }
    }

    public Vector2[] ValidTiles()
    {
        List<Vector2> validTiles = new List<Vector2>();
        foreach (Vector2 pos in Pattern())
        {
            if (PathFinder.Possible(transform.position, pos, moveType, maxRange))
            {
                validTiles.Add(pos);
            }
        }
        return validTiles.ToArray();
    }

    protected abstract Vector2[] Pattern();

    public override void SelectTargets(Vector2[] pos)
    {
        //This is bad implementation but it works for now
        StartCoroutine(MovePath(PathFinder.Path(transform.position, pos[0], moveType)));
    }

    IEnumerator MovePath(Vector2[] path)
    {
        foreach (Vector2 pos in path)
        {
            myUnit.MoveToTile(pos);

            //This also shouldn't be hard coded I don't think
            yield return new WaitForSeconds(References.timeBetweenMove);
        }

        if(myUnit.move == this)
        {
            myUnit.SelectAction();
        }
        else
        {
            myUnit.EndTurn();
        }
    }

    public MoveType GetMoveType()
    {
        return moveType;
    }

    public override string Description()
    {
        string output = "Moves to a space ";

        if(minRange > 0)
        {
            output += "between " + minRange + " and ";
        }
        else
        {
            output += "within ";
        }
        output += maxRange + " spaces";


        return output;
    }

}
