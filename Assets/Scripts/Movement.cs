using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float snapRadius = 0.01f;
    public float speed = 4;
    public bool isMoving = false;
    protected IEnumerator moveCoroutine;
    protected List<Vector3> path = new List<Vector3>();
    protected Vector3 targetPosition;
    protected bool isNewTargetSet;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void OnMovementEnd()
    {

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
        isMoving = true;
        while (Vector3.Distance(transform.position, position) > snapRadius)
        {
            //Debug.Log("moving");
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = position;
        isMoving = false;
        path.Remove(position);
        //Debug.Log("stopped moving");
        OnMovementEnd();
    }
    public void Move()
    {
        if (path.Count == 0 && !isMoving)
        {
            //Debug.Log("getting path");
            isNewTargetSet = false;
            GetDestination();
            if (isNewTargetSet)
                FindPath(targetPosition);
        }
        else
        {
            CheckForInterupt();
            if (!isMoving)
            {
                //Debug.Log("starting coroutine");
                if (path.Count > 0)
                {
                    moveCoroutine = MoveToPosition(path[0]);
                    StartCoroutine(moveCoroutine);
                }
            }
            //Debug.Log("waiting for interupt");
        }
    }
}
