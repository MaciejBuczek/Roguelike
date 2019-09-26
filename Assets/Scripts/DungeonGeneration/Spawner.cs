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
        rooms = new List<Room>(dungeonGenerator.rooms);
        corridors = new List<Corridor>(dungeonGenerator.corridors);
    }
    void spawnPlayer()
    {
        GameObject playerGameObject;
        Vector2 playerPosition = new Vector2Int();
        Room randRoom;
        randRoom = rooms[Random.Range(0, rooms.Count)];
        playerPosition.Set(randRoom.position.x + (randRoom.width / 2), randRoom.position.y + (randRoom.height / 2));
        playerGameObject = Instantiate(player, playerPosition, transform.rotation);
        playerGameObject.name = "Player";
    }
}
