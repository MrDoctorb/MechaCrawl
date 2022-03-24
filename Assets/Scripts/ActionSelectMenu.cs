using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zanespace;

public class ActionSelectMenu : MonoBehaviour
{
    void Awake()
    {
        if (References.actionSelectMenu == null)
        {
            References.actionSelectMenu = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Display(ActionLogic[] actions, Action end)
    {
        for (int i = 0; i < actions.Length; ++i)
        {
            ActionLogic action = actions[i];
            GameObject button = transform.GetChild(i).gameObject;
            SetButtonDelegates(button, actions[i].Perform);

            button.GetComponentInChildren<Image>().sprite = action.icon;
            button.GetComponentInChildren<Text>().text = action.actionName;
        }
        SetButtonDelegates(transform.GetChild(3).gameObject, end);
    }

    void SetButtonDelegates(GameObject buttonObj, Action action)
    {
        Button button = buttonObj.GetComponent<Button>();
        button.gameObject.SetActive(true);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action.Invoke);
        button.onClick.AddListener(Hide);
    }

    public void Hide()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
