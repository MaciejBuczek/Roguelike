using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour
{
    enum doorIDs
    {
        doorTop, doorBottom, doorLeft, doorRight
    };
    enum wallIDs
    {
        cornerTopLeft, cornerTopRight, cornerBottomLeft, cornerBottomRight, top, bottom, left,right,
        ldCornerTopLeft, rdCornerTopRight, ldCornerBottomLeft, rdCornerBottomRight, tdCornerTopLeft, tdCornerTopRight, bdCornerBottomLeft, bdCorerBottomRight,
        ldTop, rdTop, ldBottom, rdBottom, tdLeft, bdLeft, tdRight, bdRight, insideCornerLeftTop, insideCornerRightTop, insideCornerLeftBottom, insideCornerRightBottom,
    };
    public Sprite[] floorSprites;
    public Sprite[] doorSprites;
    public Sprite[] wallSprites;
    GameObject[,] dungeonGameObjectArray;
    TileType[,] dungeonTileTypeLayout;
    bool[,] hasAssignedSprite;
    int rows, cols;
    // Start is called before the first frame update
    void Start()
    {
        initializeValues();
        applayFloorSprites();
        applyRoomsSprites();
        applyCorridorsSprites();
    }
    void initializeValues()
    {
        dungeonGameObjectArray = GetComponent<DungeonGenerator>().dungeonGameObjectArray;
        dungeonTileTypeLayout = GetComponent<DungeonGenerator>().dungeonTileTypeLayout;
        rows = GetComponent<DungeonGenerator>().rows;
        cols = GetComponent<DungeonGenerator>().cols;
        hasAssignedSprite = new bool[rows, cols];
    }
    void applayFloorSprites()
    {
        for(int y=0; y<rows; y++)
        {
            for(int x=0; x<cols; x++)
            {
                if (compareWithDungeonTileType(x, y, TileType.Floor))
                    setSprite(x, y, floorSprites[0]);
            }
        }
    }
    void applyDoorsprites(Room room)
    {
        List<Room> rooms = GetComponent<DungeonGenerator>().rooms;
        Sprite sprite;
        foreach (Vector2Int doorPosition in room.doorsPositions)
        {
            if (doorPosition.y == room.position.y - 1)
                sprite = doorSprites[(int)doorIDs.doorBottom];
            else if (doorPosition.y == room.position.y + room.height)
                sprite= doorSprites[(int)doorIDs.doorTop];
            else if(doorPosition.x == room.position.x - 1)
                sprite = doorSprites[(int)doorIDs.doorLeft];
            else
                sprite = doorSprites[(int)doorIDs.doorRight];
            setSprite(doorPosition.x, doorPosition.y, sprite);
        }
    }
    void applyRoomsSprites()
    {
        List<Room> rooms = GetComponent<DungeonGenerator>().rooms;
        foreach(Room room in rooms)
        {
            applyRoomConnectionSprites(room);
            applyDoorsprites(room);
            applyRoomWallSprites(room);
        }
    }
    void applyRoomWallSprites(Room room)
    {
        Vector2Int position, startingPosition, moveVector;
        Sprite sprite;
        wallIDs wallID=0;
        position = new Vector2Int(room.position.x - 1, room.position.y - 1);
        startingPosition= new Vector2Int(room.position.x - 1, room.position.y - 1);
        moveVector = new Vector2Int();
        do
        {
            if (dungeonTileTypeLayout[position.y, position.x] == TileType.Door)
            {
                position += moveVector;
                continue;
            }
            if (position == startingPosition)
            {
                sprite = wallSprites[(int)wallIDs.cornerBottomLeft];
                moveVector = new Vector2Int(1, 0);
                wallID = wallIDs.bottom;
            }
            else if (position.x == room.position.x + room.width && position.y == room.position.y - 1)
            {
                sprite = wallSprites[(int)wallIDs.cornerBottomRight];
                moveVector = new Vector2Int(0, 1);
                wallID = wallIDs.right;
            }
            else if (position.x==room.position.x+room.width && position.y == room.position.y + room.height)
            {
                sprite = wallSprites[(int)wallIDs.cornerTopRight];
                moveVector = new Vector2Int(-1, 0);
                
                wallID = wallIDs.top;
            }
            else if (position.x==room.position.x-1 && position.y == room.position.y + room.height)
            {
                sprite = wallSprites[(int)wallIDs.cornerTopLeft];
                moveVector = new Vector2Int(0, -1);
                wallID = wallIDs.left;
            }
            else
            {
                sprite = wallSprites[(int)wallID];
            }
            setSprite(position.x, position.y, sprite);
            position += moveVector;
        } while (position != startingPosition);
    }
    void applyRoomConnectionSprites(Room room)
    {
        Sprite sprite;
        foreach(Vector2Int doorPosition in room.doorsPositions)
        {
            if (doorPosition.y == room.position.y + room.height)
            {
                if (doorPosition.x - 1 == room.position.x - 1)
                    sprite = wallSprites[(int)wallIDs.tdCornerTopLeft];
                else
                    sprite = wallSprites[(int)wallIDs.ldTop];
                setSprite(doorPosition.x - 1, doorPosition.y, sprite);
                if (doorPosition.x + 1 == room.position.x + room.width)
                    sprite = wallSprites[(int)wallIDs.tdCornerTopRight];
                else
                    sprite = wallSprites[(int)wallIDs.rdTop];
                setSprite(doorPosition.x + 1, doorPosition.y, sprite);
            }else if (doorPosition.y == room.position.y - 1)
            {
                if (doorPosition.x - 1 == room.position.x - 1)
                    sprite = wallSprites[(int)wallIDs.bdCornerBottomLeft];
                else
                    sprite = wallSprites[(int)wallIDs.ldBottom];
                setSprite(doorPosition.x - 1, doorPosition.y, sprite);
                if (doorPosition.x + 1 == room.position.x + room.width)
                    sprite = wallSprites[(int)wallIDs.bdCorerBottomRight];
                else
                    sprite = wallSprites[(int)wallIDs.rdBottom];
                setSprite(doorPosition.x + 1, doorPosition.y, sprite);
            }
            else if (doorPosition.x == room.position.x + room.width)
            {
                if (doorPosition.y - 1 == room.position.y - 1)
                    sprite = wallSprites[(int)wallIDs.rdCornerBottomRight];
                else
                    sprite = wallSprites[(int)wallIDs.bdRight];
                setSprite(doorPosition.x, doorPosition.y - 1, sprite);
                if (doorPosition.y + 1 == room.position.y + room.height)
                    sprite = wallSprites[(int)wallIDs.rdCornerTopRight];
                else
                    sprite = wallSprites[(int)wallIDs.tdRight];
                setSprite(doorPosition.x, doorPosition.y + 1, sprite);
            }
            else if(doorPosition.x == room.position.x - 1)
            {
                if (doorPosition.y - 1 == room.position.y - 1)
                    sprite = wallSprites[(int)wallIDs.ldCornerBottomLeft];
                else
                    sprite = wallSprites[(int)wallIDs.bdLeft];
                setSprite(doorPosition.x, doorPosition.y - 1, sprite);
                if (doorPosition.y + 1 == room.position.y + room.height)
                    sprite = wallSprites[(int)wallIDs.ldCornerTopLeft];
                else
                    sprite = wallSprites[(int)wallIDs.tdLeft];
                setSprite(doorPosition.x, doorPosition.y + 1, sprite);
            }
        }
    }
    void applyCorridorsSprites()
    {
        List<Corridor> corridors = GetComponent<DungeonGenerator>().corridors;
        Vector2Int currentPosition, nextPosition;
        int vertialDircetion = 0, horizontalDirection = 0;
        foreach (Corridor corridor in corridors)
        {
            for (int i = 0; i < corridor.breakPoints.Count - 1; i++)
            {
                currentPosition = corridor.breakPoints[i];
                nextPosition = corridor.breakPoints[i + 1];
                while (currentPosition != nextPosition)
                {
                    
                    if (currentPosition.x > nextPosition.x)
                    {
                        currentPosition.x--;
                        horizontalDirection = -1;
                        vertialDircetion = 0;
                    }
                    else if (currentPosition.x < nextPosition.x)
                    {
                        currentPosition.x++;
                        horizontalDirection = 1;
                        vertialDircetion = 0;
                    }
                    else if (currentPosition.y > nextPosition.y)
                    {
                        currentPosition.y--;
                        vertialDircetion = 1;
                        horizontalDirection = 0;
                    }
                    else
                    {
                        currentPosition.y++;
                        vertialDircetion = - 1;
                        horizontalDirection = 0;
                    }
                    applyCorridorInsideCornerSprites(currentPosition.x, currentPosition.y, horizontalDirection, vertialDircetion);
                    applyCorridorWallSprites(currentPosition, nextPosition);
                }
            }
            for (int i=1; i < corridor.breakPoints.Count - 1; i++)
            {
                applyCorridorOutsideCornerSprites(corridor.breakPoints[i]);
            }
        }
    }
    void applyCorridorWallSprites(Vector2Int currentPosition, Vector2Int nextPosition)
    {
        int direction;
        int x, y;
        x = currentPosition.x;
        y = currentPosition.y;
        if (currentPosition.y == nextPosition.y && currentPosition.x != nextPosition.x)
        {
            if (nextPosition.x > currentPosition.x)
                direction = 1;
            else
                direction = -1;
            if (compareWithDungeonTileType(x + direction, y - 1, TileType.Wall) && compareWithDungeonTileType(x, y - 1, TileType.Wall))
                setSprite(x, y - 1, wallSprites[(int)wallIDs.bottom]);
            if (compareWithDungeonTileType(x + direction, y + 1, TileType.Wall) && compareWithDungeonTileType(x, y + 1, TileType.Wall))
                setSprite(x, y + 1, wallSprites[(int)wallIDs.top]);
        }
        if (currentPosition.x == nextPosition.x && currentPosition.y != nextPosition.y)
        {
            if (nextPosition.y > currentPosition.y)
                direction = 1;
            else
                direction = -1;
            if (compareWithDungeonTileType(x - 1, y + direction, TileType.Wall) && compareWithDungeonTileType(x - 1, y, TileType.Wall))
                setSprite(x - 1, y, wallSprites[(int)wallIDs.left]);
            if (compareWithDungeonTileType(x + 1, y + direction, TileType.Wall) && compareWithDungeonTileType(x + 1, y, TileType.Wall))
                setSprite(x + 1, y, wallSprites[(int)wallIDs.right]);
        }
    }
    void applyCorridorInsideCornerSprites(int x, int y, int horizontalDirection, int verticalDirection)
    {
        if (compareWithDungeonTileType(x - 1, y + 1, TileType.Floor))
        {
            setSprite(x - 1, y, wallSprites[(int)wallIDs.insideCornerLeftBottom]);
            setSprite(x, y + 1, wallSprites[(int)wallIDs.insideCornerRightTop]);
        }
        if (compareWithDungeonTileType(x + 1, y + 1, TileType.Floor))
        {
            setSprite(x + 1, y, wallSprites[(int)wallIDs.insideCornerRightBottom]);
            setSprite(x, y + 1, wallSprites[(int)wallIDs.insideCornerLeftTop]);
        }
        
    }
    void applyCorridorOutsideCornerSprites(Vector2Int position)
    {
        if (compareWithDungeonTileType(position.x - 1, position.y, TileType.Floor) && compareWithDungeonTileType(position.x, position.y + 1, TileType.Floor)) 
        {
            setSprite(position.x + 1, position.y - 1, wallSprites[(int)wallIDs.cornerBottomRight]);
            setSprite(position.x, position.y - 1, wallSprites[(int)wallIDs.bottom]);
            setSprite(position.x + 1, position.y, wallSprites[(int)wallIDs.right]);
        }
        else if (compareWithDungeonTileType(position.x + 1, position.y, TileType.Floor) && compareWithDungeonTileType(position.x, position.y + 1, TileType.Floor)) 
        {
            setSprite(position.x - 1, position.y - 1, wallSprites[(int)wallIDs.cornerBottomLeft]);
            setSprite(position.x, position.y - 1, wallSprites[(int)wallIDs.bottom]);
            setSprite(position.x - 1, position.y, wallSprites[(int)wallIDs.left]);
        }
        else if(compareWithDungeonTileType(position.x - 1, position.y, TileType.Floor) && compareWithDungeonTileType(position.x,position.y - 1, TileType.Floor))
        {
            setSprite(position.x + 1, position.y + 1, wallSprites[(int)wallIDs.cornerTopRight]);
            setSprite(position.x, position.y + 1, wallSprites[(int)wallIDs.top]);
            setSprite(position.x + 1, position.y, wallSprites[(int)wallIDs.right]);
        }
        else if(compareWithDungeonTileType(position.x+1, position.y, TileType.Floor) && compareWithDungeonTileType(position.x, position.y - 1, TileType.Floor))
        {
            setSprite(position.x - 1, position.y + 1, wallSprites[(int)wallIDs.cornerTopLeft]);
            setSprite(position.x, position.y + 1, wallSprites[(int)wallIDs.top]);
            setSprite(position.x - 1, position.y, wallSprites[(int)wallIDs.left]);
        }
    }
    void setSprite(int x, int y, Sprite sprite)
    {
        if (!hasAssignedSprite[y, x])
        {
            dungeonGameObjectArray[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
            hasAssignedSprite[y, x] = true;
        }
    }
    bool compareWithDungeonTileType(int x, int y, TileType tileType)
    {
        return dungeonTileTypeLayout[y, x] == tileType;
    }
}
