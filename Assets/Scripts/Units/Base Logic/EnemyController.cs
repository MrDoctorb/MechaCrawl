using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyController : UnitController
{
    TurnOption option;
    AttackLogic attack;

    protected override void EnterLevel()
    {
        MoveToTile(TileManager.RandomTile().transform.position);
        attack = (AttackLogic)actions[0];
    }

    public override void StartTurn()
    {
        //Combine attack and move to determine all possible targets
        List<TurnOption> possibleTargets = new List<TurnOption>();

        foreach (Vector2 movePos in move.ValidTiles())
        {
            foreach (Vector2[] attackGroup in attack.ValidTiles(movePos))
            {
                foreach (Vector2 attackPos in attackGroup)
                {
                    Tile potentialTile = TileManager.TileAt(attackPos);
                    if (potentialTile.unit != null && potentialTile.unit.GetType() != typeof(EnemyController))
                    {
                        possibleTargets.Add(new TurnOption(movePos, attackGroup));
                    }
                }
            }
        }

        if (possibleTargets.Count > 0)
        {
            //Choose target (Could be nearest, could be lowest health)
            option = possibleTargets[Random.Range(0, possibleTargets.Count)];

        }
        else
        {
            Vector2[] validTiles = move.ValidTiles();
            option = new TurnOption(validTiles[Random.Range(0, validTiles.Length)], new Vector2[0]);
        }

        //Move to the required space
        move.SelectTarget(option.movePos);

    }

    public override void SelectAction()
    {
        //only has attack to select
        attack.SelectTargets(option.attackGroup);
    }

}
class TurnOption
{
    public Vector2 movePos;
    public Vector2[] attackGroup;

    public TurnOption(Vector2 movePos, Vector2[] attackGroup)
    {
        this.movePos = movePos;
        this.attackGroup = attackGroup;
    }
}