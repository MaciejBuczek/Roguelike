using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public bool isObstacle;
    public bool isVisited;
    public bool hasParent;
    public float localGoal;
    public float globalGoal;
    public Vector3Int position;
    public List<Vector3Int> neighboursPositions;
    public Vector3Int parentPosition;

    public Node(bool isObstacle, Vector3Int position)
    {
        this.isObstacle = isObstacle;
        this.position = position;
        isVisited = false;
        hasParent = false;
        neighboursPositions = new List<Vector3Int>();
    }
}
