using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zanespace;

public class ActionButtonDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string description;
    [SerializeField] GameObject descriptionBoxSpawn;
    static GameObject descriptionBoxRef;
    RectTransform canvas;
    void Start()
    {
        canvas = References.actionSelectMenu.transform.parent.GetComponent<RectTransform>();
    }

    void OnDisable()
    {
        Destroy(descriptionBoxRef);
        descriptionBoxRef = null;
    }

    private void Update ()
    {
        if(descriptionBoxRef != null)
        {
            descriptionBoxRef.transform.position = Input.mousePosition + new Vector3(3, 3);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //TEMPORARY ASSIGNMENT, CHANGE LATER
        if (descriptionBoxRef !=  null)
        {
            Destroy(descriptionBoxRef);
        }
        descriptionBoxRef = Instantiate(descriptionBoxSpawn, canvas);
        descriptionBoxRef.transform.GetChild(0).GetComponent<Text>().text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDisable();
    }
}
