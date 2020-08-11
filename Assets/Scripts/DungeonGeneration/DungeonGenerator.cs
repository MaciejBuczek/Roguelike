using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Floor,Door,Wall,Empty
}

public class DungeonGenerator : MonoBehaviour
{
    #region Singleton
    public static DungeonGenerator instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more then one instance of dungeon generator found");
            return;
        }
        instance = this;
    }
    #endregion

    [HideInInspector]
    public int rows;
    [HideInInspector]
    public int cols;
    public IntRange roomsRange = new IntRange(4, 8);
    public IntRange roomWidthRange = new IntRange(2, 6);
    public IntRange roomHeightRange = new IntRange(2, 6);
    public IntRange corridorLengthRange = new IntRange(0, 5);
    public GameObject[] DungeonTiles;
    private int roomsAmount;

    public GameObject[,] dungeonGameObjectArray;
    public TileType[,] dungeonTileTypeLayout;
    public bool[,] dungeonMovementLayout;
    public List<Room> rooms;
    public List<Corridor> corridors;

    public bool useSeed = false;
    public int seed = 0;

    public bool IsPositionOutOfBounds(Vector2Int position)
    {
        if (position.x < 0 || position.x >= DungeonGenerator.instance.cols)
            return true;
        if (position.y < 0 || position.y >= DungeonGenerator.instance.rows)
            return true;
        return false;
    }

    private void Start()
    {
        initializeValues();
        generateRoomsAndCorridors();
        shiftRoomAndCorridors();
        generateDungeonTileTypeLayout();
        generateDungeonMovementLayout();
    }
    void initializeValues()
    {
        new GameObject("Dungeon");
        roomsAmount = roomsRange.Random();
        rooms = new List<Room>();
        corridors = new List<Corridor>();
        if (!useSeed)
            seed = Random.Range(-1000, 1000);
        Random.seed = seed;
    }
    void generateRoomsAndCorridors()
    {
        int availablePaths = 0;
        rooms.Add(new Room(roomWidthRange, roomHeightRange, false));
        rooms[0].position.Set(0, 0);
        availablePaths += rooms[0].exitsAmount;
        generateBranchesForRoom(rooms[0], ref availablePaths);
    }
    void generateBranchesForRoom(Room room, ref int availablePaths)
    {
        bool canBeDeadEnd;
        for (int i = 0; i < room.exitsAmount; i++)
        {
            if (rooms.Count == roomsAmount)
                return;
            if (availablePaths > 1)
                canBeDeadEnd = true;
            else
                canBeDeadEnd = false;
            rooms.Add(new Room(roomWidthRange, roomHeightRange, canBeDeadEnd));
            corridors.Add(new Corridor());
            if (corridors[corridors.Count - 1].tryToGenerateCorridor(corridorLengthRange, room, rooms[rooms.Count - 1], rooms, corridors))
            {
                if (rooms.Count == roomsAmount)
                    return;
                availablePaths--;
                availablePaths += rooms[rooms.Count - 1].exitsAmount;
                generateBranchesForRoom(rooms[rooms.Count - 1], ref availablePaths);
            }
            else
            {
                rooms.RemoveAt(rooms.Count - 1);
                corridors.RemoveAt(corridors.Count - 1);
                i--;
                room.exitsAmount--;
            }
        }
    }
    void shiftRoomAndCorridors()
    {
        int minX = 0, maxX = 0, minY = 0, maxY = 0;
        foreach (Corridor corridor in corridors)
        {
            foreach (Vector2Int breakPoint in corridor.breakPoints)
            {
                if (breakPoint.x < minX)
                    minX = breakPoint.x;
                if (breakPoint.x > maxX)
                    maxX = breakPoint.x;
                if (breakPoint.y < minY)
                    minY = breakPoint.y;
                if (breakPoint.y > maxY)
                    maxY = breakPoint.y;
            }
        }
        foreach (Room room in rooms)
        {
            if (room.position.x < minX)
                minX = room.position.x;
            if (room.position.x + room.width - 1 > maxX)
                maxX = room.position.x + room.width - 1;
            if (room.position.y < minY)
                minY = room.position.y;
            if (room.position.y + room.height - 1 > maxY)
                maxY = room.position.y + room.height - 1;
        }
        rows = Mathf.Abs(minY) + Mathf.Abs(maxY) + 3;
        cols = Mathf.Abs(minX) + Mathf.Abs(maxX) + 3;
        foreach (Room room in rooms)
        {
            room.position = room.position + new Vector2Int(Mathf.Abs(minX) + 1, Mathf.Abs(minY) + 1);
            for (int i = 0; i < room.doorsPositions.Count; i++)
            {
                room.doorsPositions[i] = room.doorsPositions[i] + new Vector2Int(Mathf.Abs(minX) + 1, Mathf.Abs(minY) + 1);
            }
        }
        foreach (Corridor corridor in corridors)
        {
            for (int i = 0; i < corridor.breakPoints.Count; i++)
            {
                corridor.breakPoints[i] = corridor.breakPoints[i] + new Vector2Int(Mathf.Abs(minX) + 1, Mathf.Abs(minY) + 1);
            }
        }
    }
    void generateDungeonTileTypeLayout()
    {
        dungeonTileTypeLayout = new TileType[rows, cols];
        dungeonGameObjectArray = new GameObject[rows, cols];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                dungeonTileTypeLayout[y, x] = TileType.Wall;
            }
        }
        bool[,] isInstantiated = new bool[rows, cols];       
        instantiateRooms(isInstantiated);
        instantiateCorridors(isInstantiated);
    }
    void instantiateRooms(bool[,] isInstantiated)
    {
        GameObject tile;
        GameObject newRoom;
        int roomNo = 0;
        new GameObject("Rooms").transform.parent=GameObject.Find("Dungeon").transform;
        foreach (Room room in rooms)
        {
            newRoom = new GameObject("Room" + roomNo);
            newRoom.transform.parent = GameObject.Find("Rooms").transform;
            foreach (Vector2Int doorPosition in room.doorsPositions)
            {
                if (!isInstantiated[doorPosition.y, doorPosition.x])
                {
                    tile = Instantiate(DungeonTiles[2], new Vector2(doorPosition.x, doorPosition.y), transform.rotation);
                    tile.transform.parent = newRoom.transform;
                    dungeonTileTypeLayout[doorPosition.y, doorPosition.x] = TileType.Door;
                    dungeonGameObjectArray[doorPosition.y, doorPosition.x] = tile;
                    isInstantiated[doorPosition.y, doorPosition.x] = true;
                }
            }
            for (int y = room.position.y - 1; y <= room.position.y + room.height; y++)
            {
                for (int x = room.position.x - 1; x <= room.position.x + room.width; x++)
                {
                    if (y == room.position.y - 1 || y == room.position.y + room.height || x == room.position.x - 1 || x == room.position.x + room.width)
                    {
                        if (!isInstantiated[y, x])
                        {
                            dungeonTileTypeLayout[y, x] = TileType.Wall;
                            tile = Instantiate(DungeonTiles[0], new Vector2(x, y), transform.rotation);
                            tile.transform.parent = newRoom.transform;
                            isInstantiated[y, x] = true;
                            dungeonGameObjectArray[y, x] = tile;
                        }
                    }
                    else
                    {
                        if (!isInstantiated[y, x])
                        {
                            dungeonTileTypeLayout[y, x] = TileType.Floor;
                            tile = Instantiate(DungeonTiles[1], new Vector2(x, y), transform.rotation);
                            tile.transform.parent = newRoom.transform;
                            isInstantiated[y, x] = true;
                            dungeonGameObjectArray[y,x] = tile;
                        }
                    }                    
                }
            }          
            roomNo++;
        }
    }
    void instantiateCorridors(bool[,] isInstantiated)
    {
        GameObject tile;
        GameObject newCorridor;
        Vector2Int currentPosition, nextPosition;
        int corridorNo = 0;
        new GameObject("Corridors").transform.parent = GameObject.Find("Dungeon").transform;
        foreach (Corridor corridor in corridors)
        {
            newCorridor = new GameObject("Corridor" + corridorNo);
            newCorridor.transform.parent = GameObject.Find("Corridors").transform;
            for (int i = 0; i < corridor.breakPoints.Count - 1; i++)
            {
                currentPosition = corridor.breakPoints[i];
                nextPosition = corridor.breakPoints[i + 1];
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
                    if (!isInstantiated[currentPosition.y, currentPosition.x] && currentPosition != corridor.breakPoints[corridor.breakPoints.Count - 1])
                    {
                        tile = Instantiate(DungeonTiles[1], new Vector2(currentPosition.x, currentPosition.y), transform.rotation);
                        tile.transform.parent = newCorridor.transform;
                        dungeonTileTypeLayout[currentPosition.y, currentPosition.x] = TileType.Floor;
                        dungeonGameObjectArray[currentPosition.y, currentPosition.x] = tile;
                        isInstantiated[currentPosition.y, currentPosition.x] = true;
                    }
                }
            }
            corridorNo++; ;
        }
        foreach(Transform corridor in GameObject.Find("Corridors").transform)
        {
            List<GameObject> tiles = new List<GameObject>();
            Transform lastGameObjectTransform = null;
            foreach(Transform childTile in corridor.transform)
            {
                lastGameObjectTransform = childTile;
                Vector2Int position = new Vector2Int((int)childTile.position.x, (int)childTile.position.y);
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (!isInstantiated[position.y + y, position.x + x])
                        {
                            tile = Instantiate(DungeonTiles[0], new Vector2(position.x + x, position.y + y), transform.rotation);
                            tiles.Add(tile);
                            dungeonTileTypeLayout[position.y + y, position.x + x] = TileType.Wall;
                            dungeonGameObjectArray[position.y + y, position.x + x] = tile;
                            isInstantiated[position.y + y, position.x + x] = true;
                        }
                    }
                }
            }
            foreach (GameObject childTile in tiles)
                childTile.transform.parent = corridor.transform;
        }
    }
    void generateDungeonMovementLayout()
    {
        dungeonMovementLayout = new bool[rows, cols];
        for(int y=0; y<rows; y++)
        {
            for(int x=0; x<cols; x++)
            {
                switch (dungeonTileTypeLayout[y, x])
                {
                    case TileType.Floor:
                    case TileType.Door:
                        dungeonMovementLayout[y, x] = true;
                        break;
                    default:
                        dungeonMovementLayout[y, x] = false;
                        break;
                }
            }
        }
    }
}
