using UnityEngine;
using System.Collections.Generic;

public class Corridor
{
    enum Directions
    {
        North, West, South, East,
    }
    public List<Vector2Int> breakPoints = new List<Vector2Int>();
    private Directions direction;
    public Corridor(){}
    public bool tryToGenerateCorridor(IntRange lengthRange, Room startingRoom, Room targetRoom, List<Room> rooms, List<Corridor> corridors)
    {
        int breakPointsGeneration = 0, targetRoomPositionGeneration=0;
        List<Vector2Int> possibleTargetRoomPositions = new List<Vector2Int>();
        Directions directionCopy;
        do
        {
            breakPointsGeneration = 0;
            findStartingPosition(startingRoom);
            do
            {
                directionCopy = direction;
                generateBreakPoints(lengthRange, ref directionCopy);
                breakPointsGeneration++;
            } while (checkForCorridorCollisionWithRooms(rooms) && breakPointsGeneration < 500);
            if (breakPointsGeneration < 500)
            {
                direction = directionCopy;
                possibleTargetRoomPositions = getPossibleTargetRoomPositions(targetRoom, rooms, corridors);
                checkForTargetRoomCollisionWithRooms(possibleTargetRoomPositions, targetRoom, rooms);
                checkForTargetRoomCollisionWithCorridors(possibleTargetRoomPositions, targetRoom, corridors);
                if (possibleTargetRoomPositions.Count > 0)
                {
                    targetRoom.position = possibleTargetRoomPositions[Random.Range(0, possibleTargetRoomPositions.Count)];
                    startingRoom.doorsPositions.Add(breakPoints[0]);
                    targetRoom.doorsPositions.Add(breakPoints[breakPoints.Count - 1]);
                    return true;
                }
            }
            targetRoomPositionGeneration++;
        } while (targetRoomPositionGeneration < 500);
        return false;
    }
    private void findStartingPosition(Room startingRoom)
    {
        do
        {
            breakPoints.Clear();
            direction = (Directions)Random.Range(0, 4);
            switch (direction)
            {
                case Directions.North:
                    breakPoints.Add(new Vector2Int(Random.Range(startingRoom.position.x, startingRoom.position.x + startingRoom.width),
                        startingRoom.position.y - 1));
                    break;
                case Directions.South:
                    breakPoints.Add(new Vector2Int(Random.Range(startingRoom.position.x, startingRoom.position.x + startingRoom.width),
                        startingRoom.position.y + startingRoom.height));
                    break;
                case Directions.East:
                    breakPoints.Add(new Vector2Int(startingRoom.position.x + startingRoom.width,
                        Random.Range(startingRoom.position.y, startingRoom.position.y + startingRoom.height)));
                    break;
                case Directions.West:
                    breakPoints.Add(new Vector2Int(startingRoom.position.x - 1,
                        Random.Range(startingRoom.position.y, startingRoom.position.y + startingRoom.height)));
                    break;
            }
        } while (startingRoom.isPositionCollidingWithDoors(breakPoints[0]));
    }
    private void generateBreakPoints(IntRange lengthRange, ref Directions direction)
    {
        int length, movesInSameDirection, rand;
        Directions opositeDirection = (Directions)(((int)direction + 2) % 4);
        Vector2Int currentPosition = breakPoints[0];
        length = lengthRange.Random();       
        movesInSameDirection = 0;
        if(breakPoints.Count > 1)
             breakPoints.RemoveRange(1, breakPoints.Count - 1);
        for (int i=0; i<length; i++)
        {
            switch (direction)
            {
                case Directions.North:
                    currentPosition.y--;
                    break;
                case Directions.South:
                    currentPosition.y++;
                    break;
                case Directions.East:
                    currentPosition.x++;
                    break;
                case Directions.West:
                    currentPosition.x--;
                    break;
            }
            movesInSameDirection++;
            rand = Random.Range(0, 10);
            if(rand<2 && movesInSameDirection > 1 && length - i > 1)
            {
                Directions randomDirecion;
                do
                {
                    randomDirecion = (Directions)Random.Range(0, 4);
                } while (randomDirecion == direction || randomDirecion == opositeDirection);
                direction = randomDirecion;
                opositeDirection= (Directions)(((int)direction + 2) % 4);
                breakPoints.Add(currentPosition);
                movesInSameDirection = 0;
            }
        }
        breakPoints.Add(currentPosition);
    }
    private bool checkForCorridorCollisionWithRooms(List<Room> rooms)
    {
        Vector2Int currentPosition;
        Vector2Int nextPosition;
        for(int i=0; i<rooms.Count-1; i++)
        {
            for(int j=0; j<breakPoints.Count - 1; j++)
            {
                currentPosition = breakPoints[j];
                nextPosition = breakPoints[j + 1];
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
                    if (rooms[i].isCollidingWitPosition(currentPosition))
                        return true;
                }               
            }
        }
        return false;
    }
    private List<Vector2Int> getPossibleTargetRoomPositions(Room targetRoom, List<Room> rooms, List<Corridor> corridors)
    {
        List<Vector2Int> possibleTargetRoomPositions = new List<Vector2Int>();        
        switch (direction)
        {
            case Directions.North:
                for (int i = 0; i < targetRoom.width; i++)
                {
                    possibleTargetRoomPositions.Add(new Vector2Int(breakPoints[breakPoints.Count - 1].x - i,
                        breakPoints[breakPoints.Count - 1].y - targetRoom.height));
                }
                break;
            case Directions.South:                
                for (int i = 0; i < targetRoom.width; i++)
                {
                    possibleTargetRoomPositions.Add(new Vector2Int(breakPoints[breakPoints.Count - 1].x - i,
                        breakPoints[breakPoints.Count - 1].y + 1));
                }
                break;
            case Directions.East:
                for (int i = 0; i < targetRoom.height; i++)
                {
                    possibleTargetRoomPositions.Add(new Vector2Int(breakPoints[breakPoints.Count - 1].x + 1,
                        breakPoints[breakPoints.Count - 1].y - i));
                }
                break;
            case Directions.West:               
                for (int i = 0; i < targetRoom.height; i++)
                {
                    possibleTargetRoomPositions.Add(new Vector2Int(breakPoints[breakPoints.Count - 1].x - targetRoom.width,
                        breakPoints[breakPoints.Count - 1].y - i));
                }
                break;
        }
        return possibleTargetRoomPositions;
    }
    private void checkForTargetRoomCollisionWithRooms(List<Vector2Int> possibleTargetRoomPositions, Room targetRoom, List<Room> rooms)
    {
        for (int i = 0; i < possibleTargetRoomPositions.Count; i++)
        {
            targetRoom.position = possibleTargetRoomPositions[i];
            for (int j = 0; j < rooms.Count - 1; j++)
            {
                if (targetRoom.isColidingWithRoom(rooms[j]))
                {
                    possibleTargetRoomPositions.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
        targetRoom.position = Vector2Int.zero;
    }
    private void checkForTargetRoomCollisionWithCorridors(List<Vector2Int> possibleTargetRoomPositions, Room targetRoom, List<Corridor> corridors)
    {
        for (int i = 0; i < possibleTargetRoomPositions.Count; i++)
        {
            targetRoom.position = possibleTargetRoomPositions[i];
            for (int j = 0; j < corridors.Count; j++)
            {
                if (targetRoom.isCollidingWithCorridor(corridors[j]))
                {
                    possibleTargetRoomPositions.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }
}