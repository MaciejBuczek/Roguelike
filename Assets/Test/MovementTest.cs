using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour {

    List<Vector3> positions = new List<Vector3>();
    int index = 0;
    public float radius = 0.01f;
    public float speed = 2;
    bool isMoving = false;

    // Use this for initialization
    void Start () {
        positions.Add(new Vector3(1, 0, 0));
        positions.Add(new Vector3(2, 0, 0));
        positions.Add(new Vector3(3, 0, 0));
        positions.Add(new Vector3(4, 0, 0));
        positions.Add(new Vector3(4, 1, 0));
        positions.Add(new Vector3(4, 2, 0));
        positions.Add(new Vector3(5, 2, 0));
        positions.Add(new Vector3(5, 3, 0));
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
            isMoving = true;
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, positions[index]) < radius)
            {
                transform.position = positions[index];
                index++;
                if (index == positions.Count)
                {
                    isMoving = false;
                    return;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, positions[index], speed * Time.deltaTime);
        }
	}
}
