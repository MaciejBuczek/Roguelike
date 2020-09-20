using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{

    public float speed = 5.0f;
    public int sightDistance;
    [HideInInspector] public List<Vector3> path;
    [HideInInspector] public bool isMoving = false;
    // Start is called before the first frame update
    protected void Start()
    {
        path = new List<Vector3>();
        PathFinder.Instance.SetObstacle((int)transform.position.x, (int)transform.position.y, true);
    }

    protected IEnumerator MoveToPosition(Vector2 end)
    {
        float distance;

        path.Remove(path[0]);
        if (!LockPosition(end))
        {
            path.Clear();
            yield break;
        }
        SetMoveAnimation(true);
        if (end.x > transform.position.x)
            Flip(false);
        else if (end.x < transform.position.x)
            Flip(true);
        isMoving = true;
        distance = ((Vector2)(transform.position) - end).sqrMagnitude;
        while(distance > float.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, end, speed * Time.deltaTime);
            distance = ((Vector2)(transform.position) - end).sqrMagnitude;
            if (CheckForInterupt())
            {
                path.Clear();
            }
            yield return null;
        }
        isMoving = false;
        OnMovementEnd();
    }
    protected virtual bool CheckForInterupt()
    {
        return false;
    }
    protected bool LockPosition(Vector2 position)
    {
        if (PathFinder.Instance.IsObstacle((int)position.x, (int)position.y))
        {
            path.Clear();
            return false;
        }
        PathFinder.Instance.SetObstacle((int)transform.position.x, (int)transform.position.y, false);
        PathFinder.Instance.SetObstacle((int)position.x, (int)position.y, true);
        return true;
    }
    protected abstract void Flip(bool isRight);
    protected abstract void SetMoveAnimation(bool isMoving);
    protected abstract void OnMovementEnd();
}
