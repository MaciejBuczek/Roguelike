using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Movement
{
    public delegate void OnPlayerTurnEnd();
    public OnPlayerTurnEnd onPlayerTurnEnd;
    public CameraController cameraController;
    private bool isCheckingForInterrupt = false;
    // Update is called once per frame
    private void Awake()
    {
        TurnController.Instance.onPlayerTurn += OnPlayerTurn;
    }
    private void GetMouseInput()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Input.ResetInputAxes();
        mousePosition.x = Mathf.Round(mousePosition.x);
        if (mousePosition.x < 0 || mousePosition.x >= DungeonGenerator.instance.cols)
            return;
        mousePosition.y = Mathf.Round(mousePosition.y);
        if (mousePosition.y < 0 || mousePosition.y >= DungeonGenerator.instance.rows)
            return;
        if (MovementManager.Instance.IsObstacle((int)mousePosition.x, (int)mousePosition.y))
            return;
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int end = new Vector2Int((int)Mathf.Round(mousePosition.x), (int)Mathf.Round(mousePosition.y));
        path = MovementManager.Instance.GeneratePath(start, end);
        MoveCamera();
    }
    void MoveCamera()
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
            LockPosition();
            Move();
            if(!isCheckingForInterrupt)
                StartCoroutine(CheckForInterupt());
        }
        else
        {
            yield return new WaitUntil(() => TurnController.Instance.test == true);
            OnMovementEnd();
        }
    }
    void LockPosition()
    {
        if (path.Count > 0)
        {
            MovementManager.Instance.SetObstacle((int)transform.position.x, (int)transform.position.y, false);
            MovementManager.Instance.SetObstacle((int)path[0].x, (int)path[0].y, true);
        }
    }
}
