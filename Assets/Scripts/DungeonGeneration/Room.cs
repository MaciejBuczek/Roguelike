using UnityEngine;
using System.Collections.Generic;

public class Room
{

    public Vector2Int position = new Vector2Int();
    public List<Vector2Int> doorsPositions = new List<Vector2Int>();
    public int width;
    public int height;
    public int exitsAmount;

    public Room(IntRange widthRange, IntRange heightRange, bool canBeDeadEnd)
    {
        int rand;
        width = widthRange.Random();
        height = heightRange.Random();
        if (canBeDeadEnd)
        {
            rand = Random.Range(0, 10);
            if (rand < 3)
            {
                exitsAmount = 0;
            }
            else
                getExitsAmount();
        }
        else
            getExitsAmount();
    }
    private void getExitsAmount()
    {
        int max = 0;
        if (width == 2)
            max += 2;
        else
        {
            max += (height / 3) * 2;
        }
        if (height == 2)
            max += 2;
        else
        {
            max += (width / 3) * 2;
        }
        Mathf.Clamp(max, 1, 5);
        exitsAmount = Random.Range(1, max);
    }
    public bool isPositionCollidingWithDoors(Vector2Int position)
    {
        foreach(Vector2Int doorPosition in doorsPositions)
        {
            if (Mathf.Abs(doorPosition.x-position.x) == 1 || Mathf.Abs(doorPosition.y - position.y) == 1)
                return true;
        }
        return false;
    }
    public bool isCollidingWitPosition(Vector2Int position)
    {
        Vector2Int topLeftCorner = new Vector2Int(this.position.x-1, this.position.y-1);
        Vector2Int bottomRightCorner = new Vector2Int(this.position.x + width, this.position.y+height);
        if (position.x >= topLeftCorner.x && position.x <= bottomRightCorner.x && position.y >= topLeftCorner.y && position.y <= bottomRightCorner.y)
            return true;
        else
            return false;
    }
    public bool isColidingWithRoom(Room room)
    {
        for(int y= room.position.y; y<room.position.y+room.height; y++)
        {
            for(int x=room.position.x; x<room.position.x+room.width; x++)
            {
                if (isCollidingWitPosition(new Vector2Int(x, y)))
                    return true;
            }
        }
        return false;
    }
    public bool isCollidingWithCorridor(Corridor corridor)
    {
        Vector2Int currentPosition;
        Vector2Int nextPosition;
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
                if (currentPosition != corridor.breakPoints[corridor.breakPoints.Count - 1] &&
                    isCollidingWitPosition(currentPosition))
                    return true;
            }
        }
        return false;
    }
}
