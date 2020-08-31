using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    // Start is called before the first frame update
    public int sightDistance = 4;
    // Update is called once per frame
    public void MoveEnemy()
    {
        while(path.Count == 0)
        {
            path = PathFinder.Instance.GeneratePath(transform.position, GetRandomPosition());
        }
        StartCoroutine(MoveToPosition(path[0]));
        //path.Remove(path[0]);
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
}
