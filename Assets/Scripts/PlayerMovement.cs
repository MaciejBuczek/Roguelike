using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Movement
{
    // Update is called once per frame
    void Update()
    {
        GetDestination();
        CheckForInterupt();
    }
    public override void GetDestination()
    {     
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving)
        {
            Vector3 targetPosition = new Vector3();
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.x = Mathf.Round(mousePosition.x);
            if (mousePosition.x < 0 || mousePosition.x >= DungeonGenerator.instance.cols)
                return;
            mousePosition.y = Mathf.Round(mousePosition.y);
            if (mousePosition.y < 0 || mousePosition.y >= DungeonGenerator.instance.rows)
                return;
            targetPosition.Set((int)Mathf.Round(mousePosition.x), (int)Mathf.Round(mousePosition.y), 0);
            FindPath(targetPosition);
            Input.ResetInputAxes();
        }
    }
    private void CheckForInterupt()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isMoving)
            {
                StopCoroutine(coroutine);
                transform.position = currentPosition;
                isMoving = false;
            }
        }
    }
}
