using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnManager : MonoBehaviour
{
    public bool playerTurn = true;
    public bool enemiesTurn = false;

    //public List<GameObject> enemies;
    public List<AggressiveCreatureController> enemies;
    public PlayerController player;

    public List<Combat> creaturesInCombat;

    public static TurnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player.MakeTurn();
        creaturesInCombat = new List<Combat>();
        List<int> creatures = new List<int>();
    }
    
    public void OnPlayerTurnEnd()
    {
        EnemyTurn();
    } 
    void EnemyTurn()
    {
        StartCoroutine(EnemiesTurn());
        player.MakeTurn();
    }
    IEnumerator EnemiesTurn()
    {
        enemiesTurn = true;
        foreach (AggressiveCreatureController enemy in enemies)
        {
            //enemy.GetComponent<EnemyMovement>().MoveEnemy();
            enemy.MakeTurn();
        }

        while (AreEnemiesActive())
        {
            yield return null;
        }
        enemiesTurn = false;
        playerTurn = true;
    }
    private bool AreEnemiesActive()
    {
        foreach(AggressiveCreatureController enemy in enemies)
        {
            if (enemy.isAvtive)
                return true;
        }
        return false;
    }
    private void GenerateCombatTurnOrder(List<Combat> creatures)
    {
        int maxSpeed, previousSpeed, currentSpeed, currentSpeedCombined, shift;
        creatures.Sort();
        List<Combat> order = new List<Combat>();
        maxSpeed = creatures[creatures.Count - 1].getCreature().attackSpeedMelee;
        order.Add(creatures[creatures.Count - 1]);
        for(int i=creatures.Count - 2; i>=0; i--)
        {
            previousSpeed = creatures[i + 1].getCreature().attackSpeedMelee;
            currentSpeed = creatures[i].getCreature().attackSpeedMelee;
            currentSpeedCombined = 0;
            shift = 0;
            for(int j=0; j<maxSpeed / currentSpeed; j++)
            {
                if (currentSpeedCombined >= previousSpeed)
                {
                    shift = currentSpeedCombined / currentSpeed + 1;
                    previousSpeed = order[shift].getCreature().attackSpeedMelee;
                    currentSpeedCombined = 0;
                }
                order.Insert(shift,creatures[i]);
                currentSpeedCombined += currentSpeed;
            }
        }
    }
}
