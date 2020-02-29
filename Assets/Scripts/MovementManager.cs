using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private Node[,] nodes;
    private DungeonGenerator dungeonGenerator;

    #region Singleton
    public static MovementManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More then one instance of movement manager found");
            return;
        }
        Instance = this;
    }
    #endregion

    private void Start()
    {
        dungeonGenerator = DungeonGenerator.instance;
        CreateNodeArray();
    }
    private void CreateNodeArray()
    {
        nodes = new Node[dungeonGenerator.rows, dungeonGenerator.cols];
        for (int y = 0; y < dungeonGenerator.rows; y++)
        {
            for (int x = 0; x < dungeonGenerator.cols; x++)
            {
                nodes[y, x] = new Node(false, new Vector3Int(x, y, 0));

                if (y > 0)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x, y - 1, 0));
                if (y < dungeonGenerator.rows - 1)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x, y + 1, 0));
                if (x > 0)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x - 1, y, 0));
                if (x < dungeonGenerator.cols - 1)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x + 1, y, 0));
                if (y > 0 && x > 0)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x - 1, y - 1, 0));
                if (y > 0 && x < dungeonGenerator.cols - 1)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x + 1, y - 1, 0));
                if (y < dungeonGenerator.rows - 1 && x > 0)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x - 1, y + 1, 0));
                if (y < dungeonGenerator.rows - 1 && x < dungeonGenerator.cols - 1)
                    nodes[y, x].neighboursPositions.Add(new Vector3Int(x + 1, y + 1, 0));
            }
        }
        for (int y = 0; y < dungeonGenerator.rows; y++)
        {
            for (int x = 0; x < dungeonGenerator.cols; x++)
            {
                if (!dungeonGenerator.dungeonMovementLayout[y, x])
                    nodes[y, x].isObstacle = true;
            }
        }
    }
    private void SortNodeList(List<Node> nodes)
    {
        Node tempNode;
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes.Count - 1; j++)
            {
                if (nodes[j].globalGoal > nodes[j + 1].globalGoal)
                {
                    tempNode = nodes[j];
                    nodes[j] = nodes[j + 1];
                    nodes[j + 1] = tempNode;
                }
            }
        }
    }
    public List<Vector3> GeneratePath(Vector2Int start, Vector2Int destination)
    {
        Node currentNode;
        List<Vector3> path = new List<Vector3>();
        List<Node> notTestedNodes = new List<Node>();
        for (int y = 0; y < dungeonGenerator.rows; y++)
        {
            for (int x = 0; x < dungeonGenerator.cols; x++)
            {
                nodes[y, x].isVisited = false;
                nodes[y, x].globalGoal = Mathf.Infinity;
                nodes[y, x].localGoal = Mathf.Infinity;
                nodes[y, x].parentPosition = Vector3Int.zero;
                nodes[y, x].hasParent = false;
            }
        }
        currentNode = nodes[start.y, start.x];
        nodes[start.y, start.x].localGoal = 0;
        nodes[start.y, start.x].globalGoal = GetDistance(nodes[start.y, start.x], nodes[start.y, start.x]);
        notTestedNodes.Add(nodes[start.y, start.x]);
        while (notTestedNodes.Count != 0 && currentNode != nodes[destination.y, destination.x])
        {
            SortNodeList(notTestedNodes);
            foreach (Node node in notTestedNodes)
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
            foreach (Vector3Int nodePosition in currentNode.neighboursPositions)
            {
                if (!nodes[nodePosition.y, nodePosition.x].isVisited && !nodes[nodePosition.y, nodePosition.x].isObstacle &&
                    notTestedNodes.IndexOf(nodes[nodePosition.y, nodePosition.x]) == -1)
                    notTestedNodes.Add(nodes[nodePosition.y, nodePosition.x]);
                float newLocalGoal = currentNode.localGoal + GetDistance(currentNode, nodes[nodePosition.y, nodePosition.x]);
                if (newLocalGoal < nodes[nodePosition.y, nodePosition.x].localGoal)
                {
                    nodes[nodePosition.y, nodePosition.x].hasParent = true;
                    nodes[nodePosition.y, nodePosition.x].parentPosition = currentNode.position;
                    nodes[nodePosition.y, nodePosition.x].localGoal = newLocalGoal;
                    nodes[nodePosition.y, nodePosition.x].globalGoal = newLocalGoal +
                        GetDistance(nodes[nodePosition.y, nodePosition.x], nodes[destination.y, destination.x]);

                }
            }
        }
        currentNode = nodes[destination.y, destination.x];
        while (currentNode.hasParent)
        {
            path.Add(currentNode.position);
            currentNode = nodes[currentNode.parentPosition.y, currentNode.parentPosition.x];
        }
        path.Reverse();
        return path;
    }
    private float GetDistance(Node a, Node b)
    {
        return Mathf.Sqrt((a.position.x - b.position.x) * (a.position.x - b.position.x) + (a.position.y - b.position.y) * (a.position.y - b.position.y)); ;
    }
}
