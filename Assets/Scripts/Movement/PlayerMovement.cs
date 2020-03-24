using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Movement
{
    public delegate void OnPlayerTurnEnd();
    public OnPlayerTurnEnd onPlayerTurnEnd;
    public CameraController cameraController;
    public bool isPlayerTurn = true;
    private GameObject focusedEnemy;
    private bool isFollowingEnemy = false;
    // Update is called once per frame
    private void Awake()
    {
        TurnController.Instance.onPlayerTurn += OnPlayerTurn;
    }
    private void Update()
    {
        if (isPlayerTurn)
        {
            Move();
        }
    }
    public override void GetDestination()
    {
        isNewTargetSet = false;
        if (isFollowingEnemy)
        {
            path.Clear();
            targetPosition = focusedEnemy.transform.position;
            isNewTargetSet = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Enemy"))
            {
                Debug.Log(hitInfo.collider.name);
                focusedEnemy = hitInfo.collider.gameObject;
                if (Vector3.Distance(transform.position, focusedEnemy.transform.position) <= 1.5f)
                    skipMove = true;
                else
                {
                    isFollowingEnemy = true;
                    targetPosition = focusedEnemy.transform.position;
                    isNewTargetSet = true;
                }
            }
            else
            {
                GetMouseInput();
            }         
        }
    }
    private void GetMouseInput()
    {
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
        Transform cameraTransform = transform.GetChild(0).transform;
        if (cameraTransform.position.x != transform.position.x || cameraTransform.position.y != transform.position.y)
            cameraController.lerpToPosition(transform.position, Time.time, 0.15f);
        Input.ResetInputAxes();
        isNewTargetSet = true;
    }
    public override void CheckForInterupt()
    {
        if (isFollowingEnemy)
        {
            if (Vector3.Distance(transform.position, focusedEnemy.transform.position) <= 2f)
            {
                path.Clear();
                skipMove = true;
                isFollowingEnemy = false;
            }
        }
        if (isMoving && Input.GetKeyDown(KeyCode.Mouse0))
        {
            path.Clear();
            isNewTargetSet = false;
        }
    }
    public override void OnMovementEnd()
    {
        isPlayerTurn = false;
        if (onPlayerTurnEnd != null)
        {
            onPlayerTurnEnd.Invoke();
        }
    }
    void OnPlayerTurn()
    {
        isPlayerTurn = true;
    }
}
