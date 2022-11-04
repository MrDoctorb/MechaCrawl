using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zanespace;

public class TurnOrderVisuals : MonoBehaviour
{
    Animator[] containers = new Animator[6];
    Image[] icons = new Image[6];
    Image[] panels = new Image[6];

    UnitController currentTurn;

    private void Awake()
    {
        References.TurnOrderVisualizer = this;
    }

    private void Start()
    {
        containers = GetComponentsInChildren<Animator>();
        for(int i = 0; i < containers.Length; i++)
        {
            icons[i] = containers[i].transform.GetChild(0).GetComponent<Image>();
            panels[i] = containers[i].GetComponent<Image>();
        }
    }

    public void UpdateVisuals(List<UnitController> units)
    {
        currentTurn = units[0];
        containers[0].SetTrigger("Left");
        for (int i = 1; i < 5; i++)
        {
            containers[i].SetTrigger("Up");
        }
        containers[5].SetTrigger("Right");
        icons[5].sprite = units[4].GetComponent<SpriteRenderer>().sprite;

        if (units[4] is EnemyController)
        {
            panels[5].color = References.enemyOutline;
        }
        else
        {
            panels[5].color = References.allyOutline;
        }

        StartCoroutine(ChangeSprites(units));
    }

    IEnumerator ChangeSprites(List<UnitController> units)
    {
        yield return new WaitForSeconds(.25f);
        for (int i = 0; i < units.Count; i++)
        {
            containers[i].SetTrigger("Reset");
            icons[i].sprite = units[i].GetComponent<SpriteRenderer>().sprite;
            if (units[i] is EnemyController)
            {
                panels[i].color = References.enemyOutline;
            }
            else
            {
                panels[i].color = References.allyOutline;
            }
        }
        containers[5].SetTrigger("Reset");
    }
}
