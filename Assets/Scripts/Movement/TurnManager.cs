using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnManager : MonoBehaviour
{
    public bool playerTurn = true;
    public bool enemiesTurn = false;

    public List<GameObject> enemies;
    public GameObject player;
    private PlayerMovement playerMovement;

    public static TurnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        StartCoroutine(PlayerTurn());
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
        StartCoroutine(PlayerTurn());
    }
    IEnumerator PlayerTurn()
    {
        while(playerMovement.path.Count == 0)
        {
            while (!Input.GetKey(KeyCode.Mouse0))
            {
                yield return null;
            }
            GetPlayerInput();
            yield return null;
        }
        StartCoroutine(playerMovement.StartMovement());
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
    private void GetPlayerInput()
    {
        Input.ResetInputAxes();
        GameObject focusedEnemy = null;
        Vector2 end;

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (Cursor.Instance.PointingAt != null && Cursor.Instance.PointingAt.CompareTag("Enemy"))
            focusedEnemy = Cursor.Instance.PointingAt;

        end = Cursor.Instance.position;

        if (DungeonGenerator.instance.IsPositionOutOfBounds(end) || (focusedEnemy == null && PathFinder.Instance.IsObstacle((int)end.x, (int)end.y)))
            return;

        playerMovement.SetPath(PathFinder.Instance.GeneratePath(player.transform.position, end));

        //player.MoveCamera();
    }
}
