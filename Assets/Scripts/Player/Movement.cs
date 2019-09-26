using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //private TileType[,] dungeonTiles;
    private bool[,] dungeonMovementLayout;
    private Vector2Int targetPosition;
    private List<Vector3> movementPositions;
    private Node[,] nodes;
    int rows, cols;

    private bool isNewTargetPositionSet = false;
    private bool isMoving = false;   

    private int index=0;
    private bool canContinueMoving = true;
    public float snapRadius = 0.01f;
    public float speed = 4;

	// Use this for initialization
	void Start () {
        initializeValues();
        createNodeArray();
    }    
    // Update is called once per frame
    void Update () {
        checkForNewTargetPosition();
        generateNewPath();
        movePlayer();
    }

    void initializeValues()
    {       
        GameObject gameManager = GameObject.Find("GameManager");
        dungeonMovementLayout = gameManager.GetComponent<DungeonGenerator>().dungeonMovementLayout;
        rows = gameManager.GetComponent<DungeonGenerator>().rows;
        cols = gameManager.GetComponent<DungeonGenerator>().cols;
        targetPosition = new Vector2Int();
        movementPositions = new List<Vector3>();
    }
    void checkForNewTargetPosition()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isMoving)
            {
                canContinueMoving = false;
            }
            else
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.x = Mathf.Round(mousePosition.x);
                if (mousePosition.x < 0 || mousePosition.x >= cols)
                    return;
                mousePosition.y = Mathf.Round(mousePosition.y);
                if (mousePosition.y < 0 || mousePosition.y >= rows)
                    return;
                if(dungeonMovementLayout[(int)mousePosition.y,(int)mousePosition.x])
                {
                    targetPosition.Set((int)Mathf.Round(mousePosition.x), (int)Mathf.Round(mousePosition.y));
                    isNewTargetPositionSet = true;
                    canContinueMoving = true;
                    Input.ResetInputAxes();                 
                }
            }
        }
    }
    void generateNewPath()
    {
        if (isNewTargetPositionSet)
        {
            findPath();
            if (movementPositions.Count > 0)
                isMoving = true;
            isNewTargetPositionSet = false;
            if (transform.GetChild(0).transform.position.x != transform.position.x && transform.GetChild(0).transform.position.y != transform.position.y)
            {
                transform.GetChild(0).GetComponent<CameraController>().lerpToPosition(transform.position, Time.time, 0.15f);
            }
        }
    }
    void findPath()
    {
        Node currentNode;
        List<Node> notTestedNodes = new List<Node>();
        int playerXPosition, playerYPosition;
        for(int y=0; y<rows; y++)
        {
            for(int x=0; x<cols; x++)
            {
                nodes[y, x].isVisited = false;
                nodes[y, x].globalGoal = Mathf.Infinity;
                nodes[y, x].localGoal = Mathf.Infinity;
                nodes[y, x].parentPosition = Vector3Int.zero;
                nodes[y, x].hasParent = false;
            }
        }
        playerXPosition = (int)transform.position.x;
        playerYPosition = (int)transform.position.y;
        currentNode = nodes[playerYPosition, playerXPosition];
        nodes[playerYPosition, playerXPosition].localGoal = 0;
        nodes[playerYPosition, playerXPosition].globalGoal = distance(nodes[playerYPosition, playerXPosition], nodes[playerYPosition, playerXPosition]);
        notTestedNodes.Add(nodes[playerYPosition, playerXPosition]);
        while(notTestedNodes.Count != 0 && currentNode != nodes[targetPosition.y, targetPosition.x])
        {
            sortNodeList(notTestedNodes);
            foreach(Node node in notTestedNodes)
            {
                if (node.isVisited)
                {
                    notTestedNodes.Remove(node);
                    break;
                }
            }
            if (notTestedNodes.Count == 0)
                break;
            currentNode = notTestedNodes[0];
            currentNode.isVisited = true;
            nodes[currentNode.position.y, currentNode.position.x].isVisited = true;
            foreach(Vector3Int nodePosition in currentNode.neighboursPositions)
            {
                if (!nodes[nodePosition.y, nodePosition.x].isVisited && !nodes[nodePosition.y, nodePosition.x].isObstacle &&
                    notTestedNodes.IndexOf(nodes[nodePosition.y,nodePosition.x])==-1)
                    notTestedNodes.Add(nodes[nodePosition.y, nodePosition.x]);
                float newLocalGoal = currentNode.localGoal + distance(currentNode, nodes[nodePosition.y, nodePosition.x]);
                if(newLocalGoal < nodes[nodePosition.y, nodePosition.x].localGoal)
                {
                    nodes[nodePosition.y, nodePosition.x].hasParent = true;
                    nodes[nodePosition.y, nodePosition.x].parentPosition = currentNode.position;
                    nodes[nodePosition.y, nodePosition.x].localGoal = newLocalGoal;
                    nodes[nodePosition.y, nodePosition.x].globalGoal = newLocalGoal + 
                        distance(nodes[nodePosition.y, nodePosition.x],nodes[targetPosition.y,targetPosition.x]);

                }
            }
        }
        movementPositions.Clear();
        currentNode = nodes[targetPosition.y, targetPosition.x];
        while(currentNode.hasParent)
        {
            movementPositions.Add(currentNode.position);
            currentNode = nodes[currentNode.parentPosition.y, currentNode.parentPosition.x];
        }
        movementPositions.Reverse();
        isNewTargetPositionSet = false;
    }
    void movePlayer()
    {
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, movementPositions[index]) < snapRadius)
            {
                transform.position = movementPositions[index];
                index++;
                if (index == movementPositions.Count || !canContinueMoving)
                {
                    isMoving = false;
                    index = 0;
                    return;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, movementPositions[index], speed * Time.deltaTime);
        }
    }
    void createNodeArray()
    {       
        nodes = new Node[rows, cols];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {                
                nodes[y, x] = new Node(false, new Vector3Int(x, y, 0));  

                if (y > 0)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x, y - 1, 0));
                if (y < rows - 1)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x, y + 1, 0));
                if (x > 0)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x - 1, y, 0));
                if (x < cols - 1)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x + 1, y, 0));
                if (y > 0 && x > 0)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x - 1, y - 1, 0));
                if (y > 0 && x < cols - 1)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x + 1, y - 1, 0));
                if (y < rows - 1 && x > 0)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x - 1, y + 1, 0));
                if (y < rows - 1 && x < cols - 1)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x + 1, y + 1, 0));
            }
        }
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if(!dungeonMovementLayout[y, x])
                    nodes[y, x].isObstacle = true;
            }
        }
    }
    void sortNodeList(List<Node> nodes)
    {
        Node tempNode;
        for(int i=0; i<nodes.Count; i++)
        {
            for(int j=0; j<nodes.Count -1; j++)
            {
                if(nodes[j].globalGoal > nodes[j + 1].globalGoal)
                {
                    tempNode = nodes[j];
                    nodes[j] = nodes[j + 1];
                    nodes[j + 1] = tempNode;
                }
            }
        }
    }
    float distance(Node a, Node b)
    {
        return Mathf.Sqrt((a.position.x - b.position.x) * (a.position.x - b.position.x) + (a.position.y - b.position.y) * (a.position.y - b.position.y));
    }
}
