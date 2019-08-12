using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tileType
{
    Wall, Floor, Door, Empty,
}

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

    public tileType[,] dungeon;
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
        cropDungeon();
        instantiateDungeon();
	}

    void initializeValues()
    {
        new GameObject("dungeonTiles");
        dungeon = new tileType[rows, cols];
        roomsAmount = roomsRange.Random();
        rooms = new List<Room>();
        corridors = new List<Corridor>();       
        for(int i=0; i<rows; i++)
        {
            for(int j=0; j<cols; j++)
            {
                dungeon[i, j] = tileType.Wall;
            }
        }
    }
    void createRoomsAndCorridors()
    {
        int roomsIt = 0, roomCount = 0, possibleBranches = 0;
        int entrancesAmount;
        bool canBeDeadEnd;
        rooms.Add(new Room());
        rooms[0].generateRoom(roomWidthRange, roomHeightRange, true);
        rooms[0].position.setPosition(rows / 2, cols / 2);
        entrancesAmount = rooms[0].entrancesAmount;
        possibleBranches += rooms[0].entrancesAmount;
        while (roomCount != roomsAmount)
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
                if (corridors[corridors.Count - 1].generateCorridor(corridorLengthRange, rooms[roomsIt], rooms[rooms.Count - 1], cols, rows, rooms, corridors))
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
        }
        applayRooms();
        applayCorridors();
    }
    void applayRooms()
    {
        foreach (Room room in rooms)
        {
            for (int x = room.position.x; x < room.position.x + room.height; x++)
            {
                for (int y = room.position.y; y < room.position.y + room.width; y++)
                {
                    dungeon[x, y] = tileType.Floor;
                }
            }
        }
    }
    void applayCorridors()
    {
        int corridorX, corridorY;
        Position nextPosition = new Position();
        foreach (Corridor corridor in corridors)
        {
            for (int j = 0; j < corridor.breakPoints.Count - 1; j++)
            {
                corridorX = corridor.breakPoints[j].x;
                corridorY = corridor.breakPoints[j].y;
                nextPosition = corridor.breakPoints[j + 1];

                if (corridorX > corridor.breakPoints[j + 1].x)
                {
                    do
                    {
                        corridorX--;
                        dungeon[corridorX, corridorY] = tileType.Floor;
                    } while (corridorX != corridor.breakPoints[j + 1].x);
                }
                else if (corridorX < corridor.breakPoints[j + 1].x)
                {
                    do
                    {
                        corridorX++;
                        dungeon[corridorX, corridorY] = tileType.Floor;
                    } while (corridorX != corridor.breakPoints[j + 1].x);
                }
                else if (corridorY > corridor.breakPoints[j + 1].y)
                {
                    do
                    {
                        corridorY--;
                        dungeon[corridorX, corridorY] = tileType.Floor;
                    } while (corridorY != corridor.breakPoints[j + 1].y);
                }
                else
                {
                    do
                    {
                        corridorY++;
                        dungeon[corridorX, corridorY] = tileType.Floor;
                    } while (corridorY != nextPosition.y);
                }
            }
            for (int j = 0; j < corridor.doorPositions.Count; j++)
            {
                dungeon[corridor.doorPositions[j].x, corridor.doorPositions[j].y] = tileType.Door;
            }
        }
    }
    void cropDungeon()
    {
        bool isWall;
        for(int i=0; i<rows; i++)
        {
            for(int j=0; j<cols; j++)
            {
                isWall = false;
                if (dungeon[i, j] == tileType.Wall)
                {
                    if ((i == 0 || j == 0 || i == rows - 1 || j == cols - 1))
                        dungeon[i, j] = tileType.Empty;
                    else
                    {
                        for(int k=i-1; k<i+2; k++)
                        {
                            for(int l=j-1; l<j+2; l++)
                            {
                                if (dungeon[k, l] == tileType.Floor || dungeon[k,l]==tileType.Door)
                                {
                                    isWall = true;
                                    break;
                                }
                            }
                        }
                        if (!isWall)
                            dungeon[i, j] = tileType.Empty;
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
        int x=0;
        for (int i=rows - 1; i>=0; i--)
        {
            for(int y=cols - 1; y>=0; y--)
            {
                switch (dungeon[i, y])
                {
                    case tileType.Wall:
                        randomTile = wallTiles[Random.Range(0,wallTiles.Length)];
                        break;
                    case tileType.Floor:
                        randomTile = floorTiles[Random.Range(0, floorTiles.Length)];
                        break;
                    case tileType.Empty:
                        randomTile = emptyTiles[Random.Range(0, emptyTiles.Length)];
                            break;
                    case tileType.Door:
                        randomTile = doorTiles[Random.Range(0, doorTiles.Length)];
                        break;
                }
                tile = Instantiate(randomTile, new Vector3(y, x, 0), transform.rotation);
                tile.transform.parent = GameObject.Find("dungeonTiles").transform;
            }
            x++;
        }
    }
}
