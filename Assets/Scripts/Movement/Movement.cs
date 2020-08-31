using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{

    public float speed = 5.0f;
    public List<Vector3> path;
    public bool isMoving = false;
    // Start is called before the first frame update
    protected void Start()
    {
        path = new List<Vector3>();
    }

    protected IEnumerator MoveToPosition(Vector2 end)
    {
        if (!LockPosition())
        {
            path.Remove(path[0]);
            yield break;
        }
        Debug.Log(transform.name + " start");
        isMoving = true;
        float distance = ((Vector2)(transform.position) - end).sqrMagnitude;
        while(distance > float.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, end, speed * Time.deltaTime);
            distance = ((Vector2)(transform.position) - end).sqrMagnitude;
            yield return null;
        }
        isMoving = false;
        path.Remove(path[0]);
        Debug.Log(transform.name + " end");
    }
    protected bool LockPosition()
    {
        if (PathFinder.Instance.IsObstacle((int)path[0].x, (int)path[0].y))
        {
            path.Clear();
            return false;
        }
        PathFinder.Instance.SetObstacle((int)transform.position.x, (int)transform.position.y, false);
        PathFinder.Instance.SetObstacle((int)path[0].x, (int)path[0].y, true);
        return true;
    }

}
