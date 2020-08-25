using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public PlayerMovement player;
    public EnemyMovement[] enemies;
    public delegate void OnPlayerTurn();
    public OnPlayerTurn onPlayerTurn;
    public static TurnController Instance;
    public bool areEnemiesMoving = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More then one instance of turn controller found");
        }
        else
            Instance = this;
        player.onPlayerTurnEnd += EnemyTurn;
    }
    void Start()
    {
        StartCoroutine(player.PlayerMove());
    }
    void EnemyTurn()
    {
        GetEnemiesDestinations();
        StartCoroutine(MoveEnemies());
        StartCoroutine(player.PlayerMove());
    }
    void GetEnemiesDestinations()
    {
        foreach(EnemyMovement enemy in enemies)
        {
            if(!enemy.isMoving)
                enemy.GetDestination();
        }
    }
    IEnumerator MoveEnemies()
    {
        foreach(EnemyMovement enemy in enemies)
        {
            if (enemy.isIdle)
            {
                enemy.idleTurns--;
                if (enemy.idleTurns == 0)
                    enemy.isIdle = false;
            }
            if (!enemy.isMoving && !enemy.isIdle)
                enemy.Move();
        }
        areEnemiesMoving = true;
        while (AreEnemiesMoving())
        {
                yield return null;
        }
        areEnemiesMoving = false;
    }
    public bool AreEnemiesMoving()
    {
        foreach(EnemyMovement enemy in enemies)
        {
            if (enemy.isMoving)
                return true;
        }
        return false;
    }
}
