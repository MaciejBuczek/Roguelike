using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    // Start is called before the first frame update
    public GameObject player;
    public Animator animator;
    public LayerMask layerMask;
    public int idleChance = 2;
    public EnemyController controller;

    private bool isFollowing = false;
    private Vector2 lastPlayerPosition;
    private int idleTurns;

    // Update is called once per frame
    public void MoveEnemy(float distanceToPlayer)
    {
        if (distanceToPlayer <= sightDistance)
        {
            if (IsPlayerInSight())
            {
                idleTurns = 0;
                isFollowing = true;
            }
            else
            {
                isFollowing = false;
                if (lastPlayerPosition != null)
                    path = PathFinder.Instance.GeneratePath(transform.position, lastPlayerPosition);
            }
        }
        if (isFollowing)
        {
            lastPlayerPosition = new Vector2(Mathf.Ceil(player.transform.position.x), Mathf.Ceil(player.transform.position.y));
            path = PathFinder.Instance.GeneratePath(transform.position, lastPlayerPosition);
        }
        else
        {
            if (path.Count == 0)
            {
                if (Random.Range(1, 99) <= idleChance)
                {
                    idleTurns = Random.Range(1, 11);
                }
                while (path.Count == 0 && idleTurns == 0)
                {
                    path = PathFinder.Instance.GeneratePath(transform.position, GetRandomPosition());
                }
            }
        }
        if (idleTurns > 0)
            idleTurns--;
        else
        {
            if (path.Count != 0)
                StartCoroutine(MoveToPosition(path[0]));
        }
        if (idleTurns > 0)
            OnMovementEnd();
    }
    private Vector2 GetRandomPosition()
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
        } while (!DungeonGenerator.instance.dungeonMovementLayout[randY, randX] || (new Vector2(randX, randY) == (Vector2)transform.position));
        return new Vector2(randX, randY);
    }

    protected override void Flip(bool isRight)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = isRight;
    }
    protected override void OnMovementEnd()
    {
        if (path.Count == 0 || player.GetComponent<PlayerMovement>().path.Count == 0)
            SetMoveAnimation(false);
        controller.OnTurnEnd();
    }
    protected override void SetMoveAnimation(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }
    private Vector3 GetDirection()
    {
        Vector3 heading = player.transform.position - transform.position;
        float distance = player.transform.position.magnitude;
        Vector3 direction = heading / distance;
        return direction;
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
}