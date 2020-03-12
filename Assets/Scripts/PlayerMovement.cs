using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Movement
{
    public delegate void OnPlayerTurnEnd();
    public OnPlayerTurnEnd onPlayerTurnEnd;
    public CameraController cameraController;
    private bool isPlayerTurn = true;
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving)
        {
            targetPosition = new Vector3();
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
            {              
                cameraController.lerpToPosition(transform.position, Time.time, 0.15f);
            }
            Input.ResetInputAxes();
            isNewTargetSet = true;
        }
    }
    public override void CheckForInterupt()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            path.Clear();
            Debug.Log(path.Count);
            Debug.Log("movement interupted");
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
        Debug.Log("player turn");
        isPlayerTurn = true;
    }
}
