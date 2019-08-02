using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    public enum tileType
    {
        Wall, Floor, Door, Empty,
    }

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
    private List<Room> rooms;
    private List<Corridor> corridors;
    private GameObject tiles;

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
        dungeon = new tileType[rows, cols];
        roomsAmount = roomsRange.Random();
        rooms = new List<Room>();
        corridors = new List<Corridor>();
        tiles = new GameObject("dungeonTiles");
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
        int roomsIt = 0;
        int corridorsIt = 0;
        int entrancesAmount;
        Position roomPosition=new Position();
        rooms.Add(new Room());
        roomsIt++;
        rooms[0].generateRoom(roomWidthRange, roomHeightRange);
        rooms[0].position.setPosition(rows / 2, cols / 2);
        entrancesAmount = rooms[0].entrancesAmount;
        for (int i = 0; i < entrancesAmount; i++)
        {
            corridors.Add(new Corridor());
            rooms.Add(new Room());
            rooms[roomsIt].generateRoom(roomWidthRange, roomHeightRange);
            if(corridors[corridorsIt].generateCorridor(corridorLengthRange, rooms[roomsIt - 1], rooms[rooms.Count - 1], cols, rows, rooms, corridors))
            {
                corridorsIt++;
                roomsIt++;
            }
            else
            {
                corridors.RemoveAt(corridors.Count - 1);
                rooms.RemoveAt(rooms.Count - 1);
            }
        }        
        applayRooms(roomsIt);
        applayCorridors(corridorsIt);
    }
    void applayRooms(int roomsIt)
    {
        for (int i = 0; i < roomsIt; i++)
        {
            for (int x = rooms[i].position.x; x < rooms[i].position.x + rooms[i].height; x++)
            {
                for (int y = rooms[i].position.y; y < rooms[i].position.y + rooms[i].width; y++)
                {
                    dungeon[x, y] = tileType.Floor;
                }
            }
        }
    }
    void applayCorridors(int corridorsIt)
    {
        int corridorX, corridorY;
        for (int i = 0; i < corridorsIt; i++)
        {
            for (int j = 0; j < corridors[i].breakPoints.Count - 1; j++)
            {
                corridorX = corridors[i].breakPoints[j].x;
                corridorY = corridors[i].breakPoints[j].y;
                if (corridorX > corridors[i].breakPoints[j + 1].x)
                {
                    do
                    {
                        corridorX--;
                        dungeon[corridorX, corridorY] = tileType.Floor;
                    } while (corridorX != corridors[i].breakPoints[j + 1].x);
                }
                else if (corridorX < corridors[i].breakPoints[j + 1].x)
                {
                    do
                    {
                        corridorX++;
                        dungeon[corridorX, corridorY] = tileType.Floor;
                    } while (corridorX != corridors[i].breakPoints[j + 1].x);
                }
                else if (corridorY > corridors[i].breakPoints[j + 1].y)
                {
                    do
                    {
                        corridorY--;
                        dungeon[corridorX, corridorY] = tileType.Floor;
                    } while (corridorY != corridors[i].breakPoints[j + 1].y);
                }
                else
                {
                    do
                    {
                        corridorY++;
                        dungeon[corridorX, corridorY] = tileType.Floor;
                    } while (corridorY != corridors[i].breakPoints[j + 1].y);
                }
            }
            for(int j=0; j<corridors[i].doorPositions.Count; j++)
            {
                dungeon[corridors[i].doorPositions[j].x, corridors[i].doorPositions[j].y] = tileType.Door;
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
