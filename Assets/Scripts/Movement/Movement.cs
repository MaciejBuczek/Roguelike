using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float snapRadius = 0.01f;
    public float speed = 16;
    public bool isMoving = false;
    protected IEnumerator moveCoroutine;
    protected List<Vector3> path = new List<Vector3>();
    protected Vector3 targetPosition;
    protected bool isNewTargetSet;
    protected bool skipMove;
    public delegate void OnMovement(bool state);
    public OnMovement onMovement;

    public virtual void OnMovementEnd()
    {
        if (onMovement != null)
            onMovement.Invoke(false);
    }
    public virtual void CheckForInterupt()
    {

    }
    public virtual void GetDestination()
    {
    }
    public void FindPath(Vector3 destination)
    {
        if (!DungeonGenerator.instance.dungeonMovementLayout[(int)destination.y, (int)destination.x])
            return;
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int end = new Vector2Int((int)destination.x, (int)destination.y);

        path = MovementManager.Instance.GeneratePath(start, end);
    }
    protected IEnumerator MoveToPosition(Vector3 position)
    {
        MovementManager.Instance.SetObstacle((int)transform.position.x, (int)transform.position.y, false);
        isMoving = true;
        while (Vector3.Distance(transform.position, position) > snapRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = position;
        isMoving = false;
        path.Remove(position);
        OnMovementEnd();
        MovementManager.Instance.SetObstacle((int)transform.position.x, (int)transform.position.y, true);
    }
    public void Move()
    {
        skipMove = false;
        CheckForInterupt();
        if (!skipMove)
        {
            GetDestination();
            if (isNewTargetSet)
                FindPath(targetPosition);
            if (!isMoving)
            {
                if (path.Count > 0)
                {
                    if (path[0].x < transform.position.x)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                    else if (path[0].x > transform.position.x)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                    }

                    moveCoroutine = MoveToPosition(path[0]);
                    if (onMovement != null)
                        onMovement.Invoke(true);
                    StartCoroutine(moveCoroutine);
                }
            }
        }
    }
}
