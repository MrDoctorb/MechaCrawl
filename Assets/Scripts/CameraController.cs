using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 previousMousePos;
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

        if(Input.mouseScrollDelta != Vector2.zero)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1, 20);
        }
    }
}
