using UnityEngine;
public class EnemyMovement : Movement
{
    public Transform playerTransform;
    public CircleCollider2D enemyCollider;
    public float sightDistance = 4;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void GetDestination()
    {
        enemyCollider.enabled = false;
        Vector3 direction = GetDirection();     
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, sightDistance, layerMask);
        enemyCollider.enabled = true;
        if (raycast.collider.CompareTag("Player"))
        {
            FindPath(new Vector3((int)playerTransform.position.x, (int)playerTransform.position.y, 0));
        }
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(!isMoving)
            GetDestination();
    }
    private Vector3 GetDirection()
    {
        Vector3 heading = playerTransform.position - transform.position;
        float distance = playerTransform.position.magnitude;
        Vector3 direction = heading / distance;
        return direction;
    }
}
