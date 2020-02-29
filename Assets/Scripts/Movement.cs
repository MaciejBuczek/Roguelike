using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float snapRadius = 0.01f;
    public float speed = 4;
    protected bool isMoving;
    protected IEnumerator coroutine;
    protected Vector3 currentPosition;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void GetDestination()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            FindPath(new Vector3(9,11,0));
        }
    }
    public void FindPath(Vector3 destination)
    {
        if (!DungeonGenerator.instance.dungeonMovementLayout[(int)destination.y, (int)destination.x])
            return;
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int end = new Vector2Int((int)destination.x, (int)destination.y);

        List<Vector3> path = MovementManager.Instance.GeneratePath(start, end);
        foreach (Vector3 position in path)
        {
            Debug.Log(position);
        }
        coroutine = Move(path);
        StartCoroutine(coroutine);
    }
    protected IEnumerator Move(List<Vector3> path)
    {
        isMoving = true;
        foreach (Vector3 position in path)
        {
            while (Vector3.Distance(transform.position, position) > snapRadius)
            {
                currentPosition = position;
                transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }
            transform.position = position;
            yield return null;
        }
        isMoving = false;
    }
}
