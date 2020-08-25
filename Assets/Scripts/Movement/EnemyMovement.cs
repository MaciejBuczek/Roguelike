using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : Movement
{
    public Transform playerTransform;
    public PlayerMovement playerMovement;
    public float sightDistance = 4;
    public LayerMask layerMask;
    public bool isEnemyTurn = false;
    private bool isPlayerTargeted=false;
    public bool isIdle = false;
    private Vector3 lastPlayerPosition;
    private Animator animator;
    public int idleTurns = 0;
    public int idleChance = 4;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    protected override void SetAnimationDirection(bool isRight)
    {
        animator.SetBool("isRight", isRight);
    }
    protected override void SetAnimationMoving(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }
     
    protected override void OnMovementEnd()
    {
        if (!playerMovement.isMovingContinuously)
        {
            SetAnimationMoving(false);
        }
    }

    public override void GetDestination()
    {
        if (isIdle && IsPlayerInSight())
        {
            isIdle = false;
            idleTurns = 0;
        }
        if (!isIdle)
        {
            if (!isPlayerTargeted && IsPlayerInSight())
            {
                isPlayerTargeted = true;
                lastPlayerPosition = playerTransform.position;
            }
            else if (isPlayerTargeted && !IsPlayerInSight())
            {
                isPlayerTargeted = false;
                if (path.Count == 0)
                {
                    Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
                    Vector2Int end = new Vector2Int((int)lastPlayerPosition.x, (int)lastPlayerPosition.y);
                    path = MovementManager.Instance.GeneratePath(start, end);
                }
            }
            if (isPlayerTargeted)
            {
                lastPlayerPosition = playerTransform.position;
                path.Clear();
                if (Vector3.Distance(playerTransform.position, transform.position) < 1.5f)
                {
                    if (!MovementManager.Instance.IsObstacle((int)transform.position.x, (int)playerTransform.position.y))
                        path.Add(new Vector3(transform.position.x, playerTransform.position.y));
                    else if (!MovementManager.Instance.IsObstacle((int)playerTransform.position.x, (int)transform.position.y))
                        path.Add(new Vector3(playerTransform.position.x, transform.position.y));
                }
                else
                {
                    Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
                    Vector2Int end = new Vector2Int((int)playerTransform.position.x, (int)playerTransform.position.y);
                    path = MovementManager.Instance.GeneratePath(start, end);
                }
            }
            else if (path.Count == 0)
            {
                if (Random.Range(0, 10) < idleChance)
                {
                    idleTurns = Random.Range(1, 8);
                    isIdle = true;
                }
                else
                    GetRandomDestination();
            }
            if (!isIdle)
                LockPosition();
        }
    }
    private bool IsPlayerInSight()
    {
        Vector3 direction = GetDirection();
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, sightDistance, layerMask);
        if (raycast.collider != null && raycast.collider.CompareTag("Player"))
            return true;
        else
            return false;
    }
    private Vector3 GetDirection()
    {       
        Vector3 heading = playerTransform.position - transform.position;
        float distance = playerTransform.position.magnitude;
        Vector3 direction = heading / distance;
        return direction;
    }
    private void GetRandomDestination()
    {
        int randX, randY;
        do
        {
            randX = (int)(transform.position.x - sightDistance);
            randY = (int)(transform.position.y - sightDistance);
            randX += Random.Range(0, (int)sightDistance * 2 + 1);
            randY += Random.Range(0, (int)sightDistance * 2 + 1);
            randX = Mathf.Clamp(randX, 0, DungeonGenerator.instance.cols - 1);
            randY = Mathf.Clamp(randY, 0, DungeonGenerator.instance.rows - 1);
        } while (!DungeonGenerator.instance.dungeonMovementLayout[randY, randX]);
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int end = new Vector2Int(randX, randY);
        path = MovementManager.Instance.GeneratePath(start, end);
    }
}
