using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float snapRadius = 0.01f;
    public float speed = 16;
    public bool isMoving = false;
    protected IEnumerator moveCoroutine;
    public List<Vector3> path = new List<Vector3>();
    public delegate void OnMovement(bool state);
    public OnMovement onMovement;

    public virtual void OnMovementEnd()
    {
        if (onMovement != null)
            onMovement.Invoke(false);
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
        isMoving = true;
        while (Vector3.Distance(transform.position, position) > snapRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = position;       
        path.Remove(position);
        OnMovementEnd();
        isMoving = false;
    }
    public void Move()
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
            Debug.Log(transform.name + "Moving");
            StartCoroutine(moveCoroutine);
        }
        else
        {
            Debug.Log(transform.position + "waiting");
        }
    }
    protected void LockPosition()
    {
        if (path.Count > 0)
        {
            if (MovementManager.Instance.IsObstacle((int)path[0].x, (int)path[0].y))
            {
                path.Clear();
                return;
            }
            //Debug.Log(transform.name + " " + transform.position + " unlock ");
            MovementManager.Instance.SetObstacle((int)transform.position.x, (int)transform.position.y, false);
            //Debug.Log(transform.name + " " + path[0] + " lock ");
            MovementManager.Instance.SetObstacle((int)path[0].x, (int)path[0].y, true);
        }
    }
}
