using UnityEngine;
using System.Collections.Generic;

public class Room{

    public Position position = new Position();
    public List<Position> entrancePositions =new List<Position>();
    public int width;
    public int height;
    public int entrancesAmount;
    private Position[] EntrancesPositions;

    public void generateRoom(IntRange widthRange, IntRange heightRange, bool canBeDeadEnd)
    {
        width = widthRange.Random();
        height = heightRange.Random();
        calculateAmountOfEntrances(canBeDeadEnd);
    }
    void calculateAmountOfEntrances(bool canBeDeadEnd)
    {
        int max = 0;
        int min = 1;
        if (width == 2)
            max += 2;
        else
        {
            max += (height/3) * 2;
        }
        if (height == 2)
            max += 2;
        else
        {
            max += (width / 3) * 2;
        }
        if (!canBeDeadEnd)
            min++;
        Mathf.Clamp(max, min, 5);
        entrancesAmount = Random.Range(min, max);
    }
    public bool isCollidingWithOtherEntrances(Position newEntrancePosition, int direction)
    {
        if (entrancePositions.Count == 0)
            return false;
        else
        {
            switch (direction)
            {
                case 0:
                case 2:
                    foreach(Position entrancePosition in entrancePositions)
                    {
                        if (entrancePosition.x == newEntrancePosition.x && Mathf.Abs(newEntrancePosition.y - entrancePosition.y) == 1)
                            return true;
                    }
                    break;
                case 1:
                case 3:
                    foreach (Position entrancePosition in entrancePositions)
                    {
                        if (entrancePosition.y == newEntrancePosition.y && Mathf.Abs(newEntrancePosition.x - entrancePosition.x) == 1)
                            return true;
                    }
                    break;
            }
        }
        return false;
    }
    public bool isPositionCollidingWithRoom(Position position)
    {
        Position leftTopCorner = new Position(this.position.x - 1, this.position.y - 1);
        Position rightBottomCorner = new Position(this.position.x + height, this.position.y + width);
        if (position.x >= leftTopCorner.x && position.x <= rightBottomCorner.x && position.y >= leftTopCorner.y && position.y <= rightBottomCorner.y)
            return true;
        return false;
    }
    public bool isColidingWithOtherRoom(Room otherRoom)
    {
        Position leftTopCorner = new Position(position.x - 1, position.y - 1);
        Position rightBottomCorner = new Position(position.x + height, position.y + width);
        Position tempPosition = new Position();
        for(int x=0; x<otherRoom.height; x++)
        {
            for(int y=0; y<otherRoom.width; y++)
            {
                tempPosition.setPosition(otherRoom.position.x + x, otherRoom.position.y + y);
                if (tempPosition.x >= leftTopCorner.x && tempPosition.x <= rightBottomCorner.x &&
                    tempPosition.y >= leftTopCorner.y && tempPosition.y <= rightBottomCorner.y)
                    return true;
            }
        }
        return false;
    }
    public bool isCollidingWithCorridor(Corridor corridor)
    {
        Position RoomTopLeftCorner = new Position(position.x - 1, position.y - 1);
        Position RoomBottomRightCorner = new Position(position.x + height, position.y + width);
        Position currentBreakPoint = new Position();
        Position nextBreakPoint = new Position();
        for (int j = 0; j < corridor.breakPoints.Count - 1; j++)
        {
            currentBreakPoint.setPosition(corridor.breakPoints[j]);
            nextBreakPoint.setPosition(corridor.breakPoints[j + 1]);
            while (currentBreakPoint != nextBreakPoint)
            {
                if (currentBreakPoint.x > nextBreakPoint.x)
                {
                    currentBreakPoint.x--;
                }
                else if (currentBreakPoint.x < nextBreakPoint.x)
                {
                    currentBreakPoint.x++;
                }
                else if (currentBreakPoint.y > nextBreakPoint.y)
                {
                    currentBreakPoint.y--;
                }
                else
                {
                    currentBreakPoint.y++;
                }
                if (currentBreakPoint.x >= RoomTopLeftCorner.x && currentBreakPoint.x <= RoomBottomRightCorner.x 
                    && currentBreakPoint.y >= RoomTopLeftCorner.y && currentBreakPoint.y <= RoomBottomRightCorner.y)
                    return true;
            }
        }
        return false;
    }
}
