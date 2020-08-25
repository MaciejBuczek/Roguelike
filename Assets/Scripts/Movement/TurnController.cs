using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnController : MonoBehaviour
{
    public PlayerMovement player;
    public EnemyMovement[] enemies;
    public delegate void OnPlayerTurn();
    public static TurnController Instance;
    public bool areEnemiesMoving = false;
    public Cursor cursor;
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
        StartCoroutine(PlayerTurn());
    }
    private IEnumerator PlayerTurn()
    {
        if (player.path.Count == 0)
        {
            while (!Input.GetKey(KeyCode.Mouse0))
            {
                yield return null;
            }
            GetMouseInput();
        }
        StartCoroutine(player.PlayerMove());
    }
    private void GetMouseInput()
    {
        Vector2Int start, end;
        Vector3 mousePosition;
        GameObject focusedEnemy = null;

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (cursor.PointingAt != null && cursor.PointingAt.CompareTag("Enemy"))
            focusedEnemy = cursor.PointingAt;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        end = new Vector2Int((int)Mathf.Round(mousePosition.x), (int)Mathf.Round(mousePosition.y));
        Input.ResetInputAxes();

        if (DungeonGenerator.instance.IsPositionOutOfBounds(end) || (focusedEnemy == null && MovementManager.Instance.IsObstacle(end)))
            return;

        start = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
        player.path = MovementManager.Instance.GeneratePath(start, end);

        player.MoveCamera();
    }
    void EnemyTurn()
    {
        GetEnemiesDestinations();
        StartCoroutine(MoveEnemies());
        StartCoroutine(PlayerTurn());
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
