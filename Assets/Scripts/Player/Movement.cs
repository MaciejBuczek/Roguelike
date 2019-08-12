using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    tileType[,] dungeonMap;
	// Use this for initialization
	void Start () {
        initializeValues();
    }    
    // Update is called once per frame
    void Update () {
		
	}
    void initializeValues()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        dungeonMap = gameManager.GetComponent<DungeonGenerator>().dungeon;

    }
}
