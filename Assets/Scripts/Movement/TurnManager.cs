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
        if (playerMovement.path.Count == 0 && playerMovement.focuesedEnemy == null) 
        {
            do
            {
                while (InventoryUI.Instance.isInventoryOpen || !Input.GetKey(KeyCode.Mouse0))
                {
                    yield return null;
                }
                GetPlayerInput();
                yield return null;
            } while (!GetPlayerInput());
        }
        if (playerMovement.path.Count > 0 || playerMovement.focuesedEnemy != null)
            StartCoroutine(playerMovement.StartMovement());
        else
            EnemyTurn();
    }
    IEnumerator MoveEnemies()
    {
        enemiesTurn = true;
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
    bool GetPlayerInput()
    {
        Input.ResetInputAxes();
        GameObject focusedEnemy = null;
        Vector2 end;

        if (EventSystem.current.IsPointerOverGameObject())
            return false;
        if (Cursor.Instance.PointingAt != null && Cursor.Instance.PointingAt.CompareTag("Enemy"))
        {
            focusedEnemy = Cursor.Instance.PointingAt;
            playerMovement.focuesedEnemy = focusedEnemy;
            return true;
        }

        end = Cursor.Instance.position;

        if (end == (Vector2)playerMovement.transform.position)
            return true;

        if (DungeonGenerator.instance.IsPositionOutOfBounds(end) || (focusedEnemy == null && PathFinder.Instance.IsObstacle((int)end.x, (int)end.y)))
            return false;

        playerMovement.SetPath(PathFinder.Instance.GeneratePath(player.transform.position, end));
        return true;
    }
}
