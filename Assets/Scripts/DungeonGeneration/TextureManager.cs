﻿using System.Collections;
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
        ldTop, rdTop, ldBot, rdBot, tdLeft, bdLeft, tdRight, bdRight,
    };
    public Sprite[] floorSprites;
    public Sprite[] doorSprites;
    public Sprite[] wallSprites;
    GameObject[,] dungeonGameObjectArray;
    TileType[,] dungeonTileTypeLayout;
    int rows, cols;
    // Start is called before the first frame update
    void Start()
    {
        initializeValues();
        applayFloorTextures();
        applyRoomsTextures();
    }
    void initializeValues()
    {
        dungeonGameObjectArray = GetComponent<DungeonGenerator>().dungeonGameObjectArray;
        dungeonTileTypeLayout = GetComponent<DungeonGenerator>().dungeonTileTypeLayout;
        rows = GetComponent<DungeonGenerator>().rows;
        cols = GetComponent<DungeonGenerator>().cols;
    }
    void applayFloorTextures()
    {
        for(int y=0; y<rows; y++)
        {
            for(int x=0; x<cols; x++)
            {
                if (dungeonTileTypeLayout[y, x] == TileType.Floor)
                    dungeonGameObjectArray[y, x].GetComponent<SpriteRenderer>().sprite = floorSprites[0];
            }
        }
    }
    void applyDoorTextures(Room room)
    {
        List<Room> rooms = GetComponent<DungeonGenerator>().rooms;
        SpriteRenderer spriteRenderer;
        foreach (Vector2Int doorPosition in room.doorsPositions)
        {
            spriteRenderer = dungeonGameObjectArray[doorPosition.y, doorPosition.x].GetComponent<SpriteRenderer>();
            if (doorPosition.y == room.position.y - 1)
            {
                spriteRenderer.sprite = doorSprites[(int)doorIDs.doorBottom];
            }
            else if (doorPosition.y == room.position.y + room.height)
            {
                spriteRenderer.sprite= doorSprites[(int)doorIDs.doorTop];
            }
            else if(doorPosition.x == room.position.x - 1)
            {
                spriteRenderer.sprite = doorSprites[(int)doorIDs.doorLeft];
            }
            else
            {
                spriteRenderer.sprite = doorSprites[(int)doorIDs.doorRight];
            }
        }
    }
    void applyRoomsTextures()
    {
        List<Room> rooms = GetComponent<DungeonGenerator>().rooms;
        foreach(Room room in rooms)
        {
            applyDoorTextures(room);
            applyRoomWallTextures(room);
            applySpecialWallTextures(room);
        }
    }
    void applyRoomWallTextures(Room room)
    {
        Vector2Int position, startingPosition, moveVector;
        SpriteRenderer spriteRenderer;
        wallIDs wallID=0;
        position = new Vector2Int(room.position.x - 1, room.position.y - 1);
        startingPosition= new Vector2Int(room.position.x - 1, room.position.y - 1);
        moveVector = new Vector2Int();
        do
        {
            spriteRenderer = dungeonGameObjectArray[position.y, position.x].GetComponent<SpriteRenderer>();
            if (dungeonTileTypeLayout[position.y, position.x] == TileType.Door)
            {
                position += moveVector;
                continue;
            }
            if (position == startingPosition)
            {
                spriteRenderer.sprite = wallSprites[(int)wallIDs.cornerBottomLeft];
                moveVector = new Vector2Int(1, 0);
                wallID = wallIDs.bottom;
            }
            else if (position.x == room.position.x + room.width && position.y == room.position.y - 1)
            {
                spriteRenderer.sprite = wallSprites[(int)wallIDs.cornerBottomRight];
                moveVector = new Vector2Int(0, 1);
                wallID = wallIDs.right;
            }
            else if (position.x==room.position.x+room.width && position.y == room.position.y + room.height)
            {
                spriteRenderer.sprite = wallSprites[(int)wallIDs.cornerTopRight];
                moveVector = new Vector2Int(-1, 0);
                
                wallID = wallIDs.top;
            }
            else if (position.x==room.position.x-1 && position.y == room.position.y + room.height)
            {
                spriteRenderer.sprite = wallSprites[(int)wallIDs.cornerTopLeft];
                moveVector = new Vector2Int(0, -1);
                wallID = wallIDs.left;
            }
            else
            {
                spriteRenderer.sprite = wallSprites[(int)wallID];
            }
            position += moveVector;
        } while (position != startingPosition);
    }
    void applySpecialWallTextures(Room room)
    {
        SpriteRenderer spriteRenderer;
        foreach(Vector2Int doorPosition in room.doorsPositions)
        {
            if (doorPosition.y == room.position.y + room.height)
            {
                spriteRenderer = dungeonGameObjectArray[doorPosition.y, doorPosition.x - 1].GetComponent<SpriteRenderer>();
                if (doorPosition.x - 1 == room.position.x - 1)
                    spriteRenderer.sprite = wallSprites[(int)wallIDs.tdCornerTopLeft];
                else
                    spriteRenderer.sprite = wallSprites[(int)wallIDs.ldTop];
                spriteRenderer = dungeonGameObjectArray[doorPosition.y, doorPosition.x + 1].GetComponent<SpriteRenderer>();
                if (doorPosition.x + 1 == room.position.x + room.width)
                    spriteRenderer.sprite = wallSprites[(int)wallIDs.tdCornerTopRight];
                else
                    spriteRenderer.sprite = wallSprites[(int)wallIDs.rdTop];
            }

        }
    }
}