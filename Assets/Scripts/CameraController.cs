using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float slideSpeed;
    public static CameraController instance;
    Vector2 previousMousePos;


    private void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            previousMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector2 currentPos = Input.mousePosition;
            Vector3 posChange = Camera.main.ScreenToWorldPoint(previousMousePos) - Camera.main.ScreenToWorldPoint(currentPos);
            posChange.z = 0;
            transform.position += posChange;
            previousMousePos = currentPos;

            //Clamp X and Y Movement
        }

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            Vector2 startMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1, 20);

            Vector2 endMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            transform.position += (Vector3)(startMouseWorldPos - endMouseWorldPos);

        }
    }

    public void NewFocus(Vector3 newPos)
    {
        //Vector3 newPos = FindObjectOfType<Entrance>().transform.position;
        newPos.z = -10;
        StartCoroutine(SlideTowards(newPos));
    }

    IEnumerator SlideTowards(Vector3 pos)
    {
        while (Vector3.Distance(transform.position, pos) > .1)
        {
            transform.position = Vector3.Lerp(transform.position, pos, slideSpeed * Time.deltaTime);
            if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
