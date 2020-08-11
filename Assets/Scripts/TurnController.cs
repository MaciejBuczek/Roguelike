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
    //public bool enemyTurnEnd = true;
    public bool areEnemiesMoving = true;
    // Start is called before the first frame update
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
            if(!enemy.isMoving)
                enemy.Move();
        }
        areEnemiesMoving = false;
        while (AreEnemiesMoving())
        {
                yield return null;
        }
        areEnemiesMoving = true;
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
