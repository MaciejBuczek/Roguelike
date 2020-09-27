using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AggressiveCreatureController
{  
    public Transform playerTransform;

    private EnemyMovement enemyMovement;

    protected override void Start()
    {
        enemyMovement = (EnemyMovement)movement;
        base.Start();
    }
    public override void MakeTurn()
    {
        isAvtive = true;
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance == 1)
            isFighting = true;
        else
            isFighting = false;

        if (isFighting)
        {
            Debug.Log("A");
            OnTurnEnd();
        }
        else
            enemyMovement.MoveEnemy(distance);
    }
    public void OnTurnEnd()
    {
        isAvtive = false;
    }
}
