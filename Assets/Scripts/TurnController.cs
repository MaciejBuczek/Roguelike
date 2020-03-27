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
    public bool test = true;
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
            enemy.GetDestination();
        }
    }
    IEnumerator MoveEnemies()
    {
        bool areEnemiesMoving = true;
        foreach(EnemyMovement enemy in enemies)
        {
            enemy.Move();
        }
        test = false;
        while (true)
        {
            foreach(EnemyMovement enemy2 in enemies)
            {
                areEnemiesMoving = false;
                if (enemy2.isMoving)
                {
                    areEnemiesMoving = true;
                    break;
                }
            }
            if (!areEnemiesMoving)
            {
                break;
            }
            yield return null;
        }
        test = true;
    }
    /*private IEnumerator StartEnemyTurn(EnemyMovement enemy)
    {    
        enemy.StartMovement();
        while (enemy.isEnemyTurn)
        {
            yield return null;
        }
        isMoving = false;
    }*/
}
