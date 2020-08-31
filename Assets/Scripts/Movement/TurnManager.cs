using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public bool playerTurn = true;
    public bool enemiesTurn = false;

    public List<GameObject> enemies;
    public GameObject player;

    public static TurnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerTurn();
    }
    // Update is called once per frame
    /*void Update()
    {
        if (playerTurn || enemiesTurn)
            return;
        Debug.Log("a");
        StartCoroutine(MoveEnemies());
    }*/
    
    public void OnPlayerTurnEnd()
    {
        EnemyTurn();
    } 
    void EnemyTurn()
    {
        StartCoroutine(MoveEnemies());
        PlayerTurn();
    }
    void PlayerTurn()
    {
        StartCoroutine(player.GetComponent<PlayerMovement>().StartMovement());
    }
    IEnumerator MoveEnemies()
    {
        enemiesTurn = true;
        //Debug.Log("enemies turn start");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyMovement>().MoveEnemy();
        }

        while (AreEnemiesMoving())
        {
            yield return null;
        }
        enemiesTurn = false;
        playerTurn = true;
        //Debug.Log("enemies turn end");
    }
    private bool AreEnemiesMoving()
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyMovement>().isMoving)
                return true;
        }
        return false;
    }
}
