using UnityEngine;
public class EnemyMovement : Movement
{
    public Transform playerTransform;
    public CircleCollider2D enemyCollider;
    public float sightDistance = 4;
    public LayerMask layerMask;
    private Vector3 destination;
    private bool isEnemyTurn = false;
    private bool isMovementEnd = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemyTurn)
        {
            Move();
        }
    }
    public override void GetDestination()
    {
        /*enemyCollider.enabled = false;
        Vector3 direction = GetDirection();     
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, sightDistance, layerMask);
        enemyCollider.enabled = true;
        if (raycast.collider.CompareTag("Player"))
        {
            destination = new Vector3(Mathf.Floor(playerTransform.position.x), Mathf.Floor(playerTransform.position.y),0);
            FindPath(new Vector3((int)playerTransform.position.x, (int)playerTransform.position.y, 0));
        }*/
        GetRandomDirection();
        isNewTargetSet = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
    private void OnTriggerStay2D(Collider2D other)
    {

    }
    private Vector3 GetDirection()
    {
        Vector3 heading = playerTransform.position - transform.position;
        float distance = playerTransform.position.magnitude;
        Vector3 direction = heading / distance;
        return direction;
    }
    private void GetRandomDirection()
    {
        int randX, randY;
        do
        {
            randX = (int)(transform.position.x - sightDistance);
            randY = (int)(transform.position.y - sightDistance);
            randX += Random.Range(0, (int)sightDistance * 2 + 1);
            randY += Random.Range(0, (int)sightDistance * 2 + 1);
            randX = Mathf.Clamp(randX, 0, DungeonGenerator.instance.cols - 1);
            randY = Mathf.Clamp(randY, 0, DungeonGenerator.instance.rows - 1);
        } while (!DungeonGenerator.instance.dungeonMovementLayout[randY, randX]);
        targetPosition = new Vector3(randX, randY, 0);
    }
    public void StartMovement()
    {
        isEnemyTurn = true;
    }
    public override void OnMovementEnd()
    {
        isEnemyTurn = false;
    }
}
