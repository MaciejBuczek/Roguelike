using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private List<Room> rooms;
    private List<Corridor> corridors;

    // Use this for initialization
    void Start () {
        Initialize();
        PlacePlayer();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Initialize()
    {
        DungeonGenerator dungeonGenerator = GetComponent<DungeonGenerator>();
        rooms = new List<Room>(dungeonGenerator.rooms);
        corridors = new List<Corridor>(dungeonGenerator.corridors);
    }
    void PlacePlayer()
    {
        Room randRoom;
        Vector3 randPosition;
        randRoom = rooms[Random.Range(0, rooms.Count)];
        randPosition=new Vector3(randRoom.position.x + (randRoom.width / 2), randRoom.position.y + (randRoom.height / 2), 0);
        GameObject.FindGameObjectWithTag("Player").transform.position = randPosition;
    }
}
