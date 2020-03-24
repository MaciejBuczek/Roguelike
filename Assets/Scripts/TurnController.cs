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
    private bool isMoving;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More then one instance of turn controller found");
        }
        else
            Instance = this;
        player.onPlayerTurnEnd += OnEnemiesTurnStart;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private IEnumerator StartEnemyTurn(EnemyMovement enemy)
    {    
        enemy.StartMovement();
        while (enemy.isEnemyTurn)
        {
            yield return null;
        }
        isMoving = false;
    }
    private IEnumerator MoveEnemies()
    {
        foreach(EnemyMovement enemy in enemies)
        {
            StartCoroutine(StartEnemyTurn(enemy));
            isMoving = true;
            while (isMoving)
            {
                yield return null;
            }
        }
        if (onPlayerTurn != null)
        {
            onPlayerTurn.Invoke();
        }
    }
    void OnEnemiesTurnStart()
    {
        StartCoroutine(MoveEnemies());
    }
}
