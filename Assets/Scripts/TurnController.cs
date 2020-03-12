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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void EnemyTurn()
    {
        Debug.Log("enemy trun");
        foreach(EnemyMovement enemy in enemies)
        {
            Debug.Log(enemy.name + "turn");
            enemy.StartMovement();
        }
        if (onPlayerTurn != null)
        {
            onPlayerTurn.Invoke();
        }
    }
}
