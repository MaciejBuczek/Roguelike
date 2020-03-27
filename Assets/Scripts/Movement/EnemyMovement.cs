using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : Movement
{
    public Transform playerTransform;
    public float sightDistance = 4;
    public LayerMask layerMask;
    public bool isEnemyTurn = false;
    private bool isPlayerTargeted=false;
    // Update is called once per frame
    private void Update()
    {     

    }
    /*public override void GetDestination()
    {
        if (isPlayerTargeted && Vector3.Distance(playerTransform.position, transform.position) > sightDistance)
            isPlayerTargeted = false;
        else if(!isPlayerTargeted && Vector3.Distance(playerTransform.position, transform.position) <= sightDistance)
        {
            if (IsPlayerInSight())
            {
                isPlayerTargeted = true;
            }
        }
        if (isPlayerTargeted)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= 1.5f)
            {
                path.Clear();
                if(!MovementManager.Instance.IsObstacle((int)transform.position.x, (int)playerTransform.position.y))
                    targetPosition = new Vector3(transform.position.x, playerTransform.position.y);
                else if (!MovementManager.Instance.IsObstacle((int)playerTransform.position.x,(int)transform.position.y))
                    targetPosition = new Vector3(playerTransform.position.x, transform.position.y);
            }else
                targetPosition = playerTransform.position;
            isNewTargetSet = true;
        }
        else if (path.Count == 0)
        {
            GetRandomDestination();
            isNewTargetSet = true;
        }
    }*/
    public override void GetDestination()
    {
        if (!isPlayerTargeted && IsPlayerInSight())
            isPlayerTargeted = true;
        else if (isPlayerTargeted && !IsPlayerInSight())
            isPlayerTargeted = false;
        if(isPlayerTargeted)
        {
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
            GetRandomDestination();
        LockPosition();
    }
    void LockPosition()
    {
        if (path.Count > 0)
        {
            //Debug.Log(transform.name + " " + transform.position + " unlock ");
            MovementManager.Instance.SetObstacle((int)transform.position.x, (int)transform.position.y, false);
            //Debug.Log(transform.name + " " + path[0] + " lock ");
            MovementManager.Instance.SetObstacle((int)path[0].x, (int)path[0].y, true);
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
    public override void CheckForInterupt()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) <= 1f)
        {
            path.Clear();
            OnMovementEnd();
        }
    }
}
