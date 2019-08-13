using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    //private tileType[,] dungeonMap;
    private List<Room> rooms;
    private List<Corridor> corridors;
    private int dungeonRows, dungeonCols;
    public GameObject player;

    // Use this for initialization
    void Start () {
        initializeValues();
        spawnPlayer();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void initializeValues()
    {
        DungeonGenerator dungeonGenerator = GetComponent<DungeonGenerator>();
        //dungeonMap = dungeonGenerator.dungeon;
        dungeonRows = dungeonGenerator.rows;
        dungeonCols = dungeonGenerator.cols;
        rooms = new List<Room>(dungeonGenerator.rooms);
        corridors = new List<Corridor>(dungeonGenerator.corridors);
    }
    void spawnPlayer()
    {
        GameObject playerGameObject;
        int randRoomIndex;
        Position playerPosition = new Position();
        Room randRoom;
        randRoomIndex = Random.Range(0, rooms.Count);
        randRoom = rooms[randRoomIndex];
        playerPosition.setPosition(randRoom.position.x + (randRoom.height / 2), randRoom.position.y + (randRoom.width / 2));
        playerPosition.x = dungeonRows - playerPosition.x - 1;
        playerGameObject = Instantiate(player, new Vector3(playerPosition.y, playerPosition.x, 0), transform.rotation);
        playerGameObject.name = "Player";
    }
}
