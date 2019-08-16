using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DungeonGenerator : MonoBehaviour {

    public int rows = 30;
    public int cols = 30;
    public IntRange roomsRange = new IntRange(4,8);
    public IntRange roomWidthRange = new IntRange(2, 6);
    public IntRange roomHeightRange = new IntRange(2, 6);
    public IntRange corridorLengthRange = new IntRange(0, 5);
    //private IntRange corridorsRange;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] emptyTiles;
    public GameObject[] doorTiles;
    private int roomsAmount;

    public DungeonTile[,] dungeonTiles;
    public List<Room> rooms;
    public List<Corridor> corridors;

    public bool useSeed = false;
    public int seed=0;

    void Start ()
    {      
        if(!useSeed)
            seed = Random.Range(-3000, 3000);
        Random.InitState(seed);
        initializeValues();
        createRoomsAndCorridors();
        initializeDungeonMap();
        applayRooms();
        applayCorridors();
        cropDungeon();
        instantiateDungeon();
        test();
	}

    void initializeValues()
    {
        new GameObject("dungeonTiles");
        roomsAmount = roomsRange.Random();
        rooms = new List<Room>();
        corridors = new List<Corridor>();       
    }
    void createRoomsAndCorridors()
    {
        int roomsIt = 0, roomCount = 1, possibleBranches = 0;
        int entrancesAmount;
        int generation = 0;
        bool canBeDeadEnd, isGenerated=false;
        rooms.Add(new Room());
        rooms[0].generateRoom(roomWidthRange, roomHeightRange, true);
        rooms[0].position.setPosition(0, 0);
        entrancesAmount = rooms[0].entrancesAmount;
        possibleBranches += rooms[0].entrancesAmount;
        do
        {
            while (roomCount != roomsAmount && generation < roomsAmount + 1)
            {
                for (int i = 0; i < entrancesAmount; i++)
                {
                    if (possibleBranches > 1)
                        canBeDeadEnd = true;
                    else
                        canBeDeadEnd = false;
                    corridors.Add(new Corridor());
                    rooms.Add(new Room());
                    rooms[rooms.Count - 1].generateRoom(roomWidthRange, roomHeightRange, canBeDeadEnd);
                    if (corridors[corridors.Count - 1].generateCorridor(corridorLengthRange, rooms[roomsIt], rooms[rooms.Count - 1], rooms, corridors))
                    {
                        roomCount++;
                        if (roomCount == roomsAmount)
                            break;
                    }
                    else
                    {
                        corridors.RemoveAt(corridors.Count - 1);
                        rooms.RemoveAt(rooms.Count - 1);
                        rooms[roomsIt].entrancesAmount--;
                        entrancesAmount--;
                        i--;
                    }
                }
                roomsIt++;
                generation++;
            }
            if (generation < roomsAmount + 1)
                isGenerated = true;
        } while (!isGenerated);
    }
    void initializeDungeonMap()
    {
        int minX=0, maxX=0, minY=0, maxY=0; 
        foreach (Room room in rooms)
        {
            if (minY > room.position.x)
                minY = room.position.x;
            if (maxY < room.position.x + room.height - 1)
                maxY = room.position.x + room.height - 1;
            if (minX > room.position.y)
                minX = room.position.y;
            if (maxX < room.position.y + room.width - 1)
                maxX = room.position.y + room.width - 1;
        }
        foreach(Corridor corridor in corridors)
        {
            foreach(Position breakPoint in corridor.breakPoints)
            {
                if (minY > breakPoint.x)
                    minY = breakPoint.x;
                if (maxY < breakPoint.x)
                    maxY = breakPoint.x;
                if (minX > breakPoint.y)
                    minX = breakPoint.y;
                if (maxX < breakPoint.y)
                    maxX = breakPoint.y;
            }
        }
        rows = Mathf.Abs(minY) + Mathf.Abs(maxY) + 5;
        cols = Mathf.Abs(minX) + Mathf.Abs(maxX) + 5;
        dungeonTiles = new DungeonTile[rows, cols];
        for(int y=0; y<rows; y++)
        {
            for (int x = 0; x < cols; x++)
                dungeonTiles[y, x] = new DungeonTile(TileType.Wall, new Position(x, y));
        }
        foreach(Room room in rooms)
        {
            room.position.setPosition(room.position.x + Mathf.Abs(minY) + 2, room.position.y + Mathf.Abs(minX) + 2);
        }
        foreach(Corridor corridor in corridors)
        {
            foreach(Position breakpoint in corridor.breakPoints) {
                breakpoint.setPosition(breakpoint.x + Mathf.Abs(minY) + 2, breakpoint.y + Mathf.Abs(minX) + 2);
            }
            foreach(Position doorPosition in corridor.doorPositions)
            {
                doorPosition.setPosition(doorPosition.x + Mathf.Abs(minY) + 2, doorPosition.y + Mathf.Abs(minX) + 2);
            }
        }
    }
    void applayRooms()
    {
        foreach (Room room in rooms)
        {
            for (int x = room.position.x; x < room.position.x + room.height; x++)
            {
                for (int y = room.position.y; y < room.position.y + room.width; y++)
                {
                    dungeonTiles[x, y].tileType = TileType.Floor;
                }
            }
        }
    }
    void applayCorridors()
    {
        Position currentPosition = new Position();
        Position nextPosition = new Position();       
        foreach (Corridor corridor in corridors)
        {
            for (int j = 0; j < corridor.breakPoints.Count - 1; j++)
            {
                currentPosition.setPosition(corridor.breakPoints[j]);
                nextPosition.setPosition(corridor.breakPoints[j + 1]);
                while (currentPosition != nextPosition)
                {
                    if (currentPosition.x > nextPosition.x)
                        currentPosition.x--;
                    else if (currentPosition.x < nextPosition.x)
                        currentPosition.x++;
                    else if (currentPosition.y > nextPosition.y)
                        currentPosition.y--;
                    else
                        currentPosition.y++;
                    dungeonTiles[currentPosition.x, currentPosition.y].tileType = TileType.Floor;
                }
            }
            for (int j = 0; j < corridor.doorPositions.Count; j++)
            {
                dungeonTiles[corridor.doorPositions[j].x, corridor.doorPositions[j].y].tileType = TileType.Door;
            }
        }
    }      
    void cropDungeon()
    {
        bool isWall;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                isWall = false;
                if (dungeonTiles[i, j].tileType == TileType.Wall)
                {
                    if ((i == 0 || j == 0 || i == rows - 1 || j == cols - 1))
                        dungeonTiles[i, j].tileType = TileType.Empty;
                    else
                    {
                        for (int k = i - 1; k < i + 2; k++)
                        {
                            for (int l = j - 1; l < j + 2; l++)
                            {
                                if (dungeonTiles[k, l].tileType == TileType.Floor || dungeonTiles[k, l].tileType == TileType.Door)
                                {
                                    isWall = true;
                                    break;
                                }
                            }
                        }
                        if (!isWall)
                            dungeonTiles[i, j].tileType = TileType.Empty;
                    }
                }
            }
        }
    }
    void instantiateDungeon()
    {
        GameObject tile;
        GameObject randomTile = new GameObject();
        Destroy(randomTile);
        int x = 0;
        for (int i = rows - 1; i >= 0; i--)
        {
            for (int y = cols - 1; y >= 0; y--)
            {
                switch (dungeonTiles[i, y].tileType)
                {
                    case TileType.Wall:
                        randomTile = wallTiles[Random.Range(0, wallTiles.Length)];
                        break;
                    case TileType.Floor:
                        randomTile = floorTiles[Random.Range(0, floorTiles.Length)];
                        break;
                    case TileType.Empty:
                        randomTile = emptyTiles[Random.Range(0, emptyTiles.Length)];
                        break;
                    case TileType.Door:
                        randomTile = doorTiles[Random.Range(0, doorTiles.Length)];
                        break;
                }
                tile = Instantiate(randomTile, new Vector3(y, x, 0), transform.rotation);
                tile.transform.parent = GameObject.Find("dungeonTiles").transform;
            }
            x++;
        }
    }
    void test()
    {
        string dungeon = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                switch (dungeonTiles[i, j].tileType)
                {
                    case TileType.Door:
                    case TileType.Floor:
                        dungeon += '%';
                        break;
                    case TileType.Wall:
                        dungeon += '#';
                        break;
                    case TileType.Empty:
                        dungeon += '@';
                        break;
                }
            }
            dungeon += '\n';
        }
        Debug.Log(dungeon);
    }

}
