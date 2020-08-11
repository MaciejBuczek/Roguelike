﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Movement
{
    public delegate void OnPlayerTurnEnd();
    public OnPlayerTurnEnd onPlayerTurnEnd;
    public CameraController cameraController;
    private GameObject focusedEnemy;
    private bool isCheckingForInterrupt = false;
    Vector2Int start, end;
    // Update is called once per frame
    private void Awake()
    {
        TurnController.Instance.onPlayerTurn += OnPlayerTurn;
    }
    private void GetMouseInput()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        CheckIfEnemyIsFocused();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        end = new Vector2Int((int)Mathf.Round(mousePosition.x), (int)Mathf.Round(mousePosition.y));
        Input.ResetInputAxes();

        if (DungeonGenerator.instance.IsPositionOutOfBounds(end) || (focusedEnemy == null && MovementManager.Instance.IsObstacle(end)))
            return;
        start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        path = MovementManager.Instance.GeneratePath(start, end);
        MoveCamera();
    }
    private void CheckIfEnemyIsFocused()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hitInfo.collider != null && hitInfo.collider.CompareTag("Enemy"))
        {
            Debug.Log(hitInfo.collider.name);
            focusedEnemy = hitInfo.collider.gameObject;
        }
    }
    private bool CheckIfEnemyIsReached() {
        if (Vector3.Distance(transform.position, focusedEnemy.transform.position) < 2)
            return true;
        return false;
    }
    private void SetPathToEnemyPosition()
    {
        start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        end = new Vector2Int((int)focusedEnemy.transform.position.x, (int)focusedEnemy.transform.position.y);
        path = MovementManager.Instance.GeneratePath(start, end);
        path.RemoveAt(path.Count - 1);
    }
    private void MoveCamera()
    {
        Transform cameraTransform = transform.GetChild(0).transform;
        if (cameraTransform.position.x != transform.position.x || cameraTransform.position.y != transform.position.y)
            cameraController.lerpToPosition(transform.position, Time.time, 0.15f);
    }
    public override void OnMovementEnd()
    {
        if (onPlayerTurnEnd != null)
        {
            onPlayerTurnEnd.Invoke();
        }
    }
    IEnumerator CheckForInterupt()
    {
        isCheckingForInterrupt = true;
        while (path.Count>0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                focusedEnemy = null;
                path.Clear();
                break;              
            }
            yield return null;
        }
        isCheckingForInterrupt = false;
    }
    void OnPlayerTurn()
    {
        StartCoroutine(PlayerMove());
    }
    public  IEnumerator PlayerMove()
    {
        if (path.Count == 0)
        {
            while (!Input.GetKey(KeyCode.Mouse0))
            {
                yield return null;
            }
            GetMouseInput();
        }
        if (path.Count > 0)
        {
            if (focusedEnemy != null && CheckIfEnemyIsReached())
            {
                focusedEnemy = null;
                path.Clear();
                yield return new WaitUntil(() => TurnController.Instance.areEnemiesMoving == true);
                OnMovementEnd();
            }
            else
            {
                if(focusedEnemy != null)
                    SetPathToEnemyPosition();
                LockPosition();
                Move();
                yield return new WaitUntil(() => TurnController.Instance.areEnemiesMoving == true);
                if (!isCheckingForInterrupt)
                    StartCoroutine(CheckForInterupt());
            }
        }
        else
        {
            yield return new WaitUntil(() => TurnController.Instance.areEnemiesMoving == true);
            OnMovementEnd();
        }
    }
}
