using UnityEngine;
using System.Collections.Generic;

public class Corridor
{
    enum directions
    {
        North,West,South,East,
    }

    public List<Position> breakPoints = new List<Position>();
    public List<Position> doorPositions = new List<Position>();

    public bool generateCorridor(IntRange lengthRange, Room startingRoom, Room nextRoom, int dungeonCols, int dungeonRows, List<Room> rooms, List<Corridor> corridors)
    {
        Position currentPosition = new Position();
        List<Position> possibleRoomPositions = new List<Position>();
        directions currentDirection = directions.North;
        directions newDirection = directions.North;
        int corridorGeneration=0, roomGeneration=0;        
        do
        {
            findCorridorStartingPosition(ref currentDirection, currentPosition, startingRoom);
            do
            {
                breakPoints.Clear();
                doorPositions.Clear();
                generateBreakPoints(currentDirection, ref newDirection, new Position(currentPosition.x, currentPosition.y), lengthRange);
                corridorGeneration++;
            } while (corridorGeneration < 1000 && isCorridorCollidingWithRooms(rooms, dungeonRows, dungeonCols));
            if (corridorGeneration < 1000)
            {
                currentDirection = newDirection;
                possibleRoomPositions = findRoomStartingPositions(currentDirection, nextRoom, dungeonRows, dungeonCols, rooms, corridors);
                if (possibleRoomPositions.Count != 0)
                {
                    nextRoom.position.setPosition(possibleRoomPositions[Random.Range(0, possibleRoomPositions.Count)]);
                    startingRoom.entrancePositions.Add(new Position(breakPoints[0]));
                    nextRoom.entrancePositions.Add(new Position(breakPoints[breakPoints.Count - 1]));
                    return true;
                }
            }
            else
                roomGeneration++;
        } while (roomGeneration < 1000);
        return false;
    }
    private void findCorridorStartingPosition(ref directions currentDirection, Position currentPosition, Room startingRoom)
    {
        do
        {
            currentDirection = (directions)Random.Range(0, 4);
            switch (currentDirection)
            {
                case directions.North:
                    currentPosition.setPosition(startingRoom.position.x, Random.Range(startingRoom.position.y, startingRoom.position.y + (startingRoom.width - 1)));
                    break;
                case directions.South:
                    currentPosition.setPosition(startingRoom.position.x + startingRoom.height - 1,
                        Random.Range(startingRoom.position.y, startingRoom.position.y + (startingRoom.width - 1)));
                    break;
                case directions.West:
                    currentPosition.setPosition(Random.Range(startingRoom.position.x, startingRoom.position.x + (startingRoom.height - 1)), startingRoom.position.y);
                    break;
                case directions.East:
                    currentPosition.setPosition(Random.Range(startingRoom.position.x, startingRoom.position.x + (startingRoom.height - 1)),
                        startingRoom.position.y + startingRoom.width - 1);
                    break;
            }
        } while (startingRoom.isCollidingWithOtherEntrances(currentPosition, (int)currentDirection));
    }
    private List<Position> findRoomStartingPositions(directions currentDirection, Room nextRoom, int dungeonRows, int dungeonCols, List<Room> rooms, List<Corridor> corridors)
    {
        List<Position> possibleRoomPositions = new List<Position>();
        switch (currentDirection)
        {
            case directions.North:
                for(int i=0; i<nextRoom.width; i++)
                {
                    possibleRoomPositions.Add(new Position(breakPoints[breakPoints.Count - 1].x - (nextRoom.height - 1), breakPoints[breakPoints.Count - 1].y - i));
                }
                break;
            case directions.South:
                for (int i = 0; i < nextRoom.width; i++)
                {
                    possibleRoomPositions.Add(new Position(breakPoints[breakPoints.Count - 1].x, breakPoints[breakPoints.Count - 1].y - i));
                }
                break;
            case directions.West:
                for (int i = 0; i < nextRoom.height; i++)
                {
                    possibleRoomPositions.Add(new Position(breakPoints[breakPoints.Count - 1].x - i, breakPoints[breakPoints.Count - 1].y - (nextRoom.width - 1)));
                }
                break;
            case directions.East:
                for (int i = 0; i < nextRoom.height; i++)
                {
                    possibleRoomPositions.Add(new Position(breakPoints[breakPoints.Count - 1].x - i, breakPoints[breakPoints.Count - 1].y));
                }
                break;
        }       
        searchForRoomCollisionWithRooms(possibleRoomPositions, nextRoom, dungeonRows, dungeonCols, rooms);
        searchForRoomCollisionWithCorridors(possibleRoomPositions, nextRoom, corridors);
        return possibleRoomPositions;
    }
    private void generateBreakPoints(directions currentDirection, ref directions newDirection, Position currentPosition, IntRange lengthRange)
    {
        int length, rand, continuedDirection = 0 ;
        directions opositeDirection;
        directions randomDirection;
        length = lengthRange.Random();
        breakPoints.Add(new Position(currentPosition.x, currentPosition.y));
        opositeDirection = (directions)(((int)currentDirection + 2) % 4);
        for (int i = length; i >= 0; i--)
        {
            switch (currentDirection)
            {
                case directions.North:
                    currentPosition.x--;
                    break;
                case directions.South:
                    currentPosition.x++;
                    break;
                case directions.West:
                    currentPosition.y--;
                    break;
                case directions.East:
                    currentPosition.y++;
                    break;
            }
            continuedDirection++;
            if (i == 1 || i == length)
                doorPositions.Add(new Position(currentPosition.x, currentPosition.y));
            rand = Random.Range(0, 10);
            if (rand < 2 && i > 1 && i < length && continuedDirection > 1)
            {
                do
                {
                    randomDirection = (directions)Random.Range(0, 4);
                } while (randomDirection == opositeDirection || randomDirection == currentDirection);
                currentDirection = randomDirection;
                opositeDirection = (directions)(((int)currentDirection + 2) % 4);
                breakPoints.Add(new Position(currentPosition.x, currentPosition.y));
                continuedDirection = 0;
            }
        }
        breakPoints.Add(new Position(currentPosition.x, currentPosition.y));
        newDirection = currentDirection;
    }
    private bool isCorridorCollidingWithRooms(List<Room> rooms, int dungeonRows, int dungeonCols)
    {
        Position position= new Position();
        Position nextPosition = new Position();
        bool isNotFirstIteration;
        for (int i = 0; i < breakPoints.Count; i++)
        {
            if (breakPoints[i].x <= 0 || breakPoints[i].x >= dungeonRows || breakPoints[i].y <= 0 || breakPoints[i].y >= dungeonCols)
            {
                return true;
            }
        }
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            /*for (int j = 1; j < breakPoints.Count; j++)
            {
                if (rooms[i].isPositionCollidingWithRoom(breakPoints[j]))
                    return true;
            }*/
            isNotFirstIteration = false;
            for(int j=0; j<breakPoints.Count - 1; j++)
            {
                position.setPosition(breakPoints[j]);
                nextPosition.setPosition(breakPoints[j + 1]);
                while (position != nextPosition)
                {
                    if (position.x > nextPosition.x)
                        position.x--;
                    else if (position.x < nextPosition.x)
                        position.x++;
                    else if (position.y > nextPosition.y)
                        position.y--;
                    else
                        position.y++;
                    if (isNotFirstIteration && rooms[i].isPositionCollidingWithRoom(position))
                        return true;
                    isNotFirstIteration = true;
                }
            }
        }
        return false;
    }
    private void searchForRoomCollisionWithRooms(List<Position> possibleRoomPositions, Room nextRoom, int dungeonRows, int dungeonCols, List<Room> rooms)
    {   
        for (int i = 0; i < possibleRoomPositions.Count; i++)
        {
            nextRoom.position.setPosition(possibleRoomPositions[i]);
            if (nextRoom.position.x <= 0 || nextRoom.position.x + (nextRoom.height - 1) >= dungeonRows
                || nextRoom.position.y <= 0 || nextRoom.position.y + (nextRoom.width - 1) >= dungeonCols)
            {
                possibleRoomPositions.RemoveAt(i);
                i--;
            }
            for (int j = 0; j <= rooms.Count - 2; j++)
            {
                if (nextRoom.isColidingWithOtherRoom(rooms[j]))
                {
                    possibleRoomPositions.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }
    private void searchForRoomCollisionWithCorridors(List<Position> possibleRoomPositions, Room nextRoom, List<Corridor> corridors)
    {
        for (int i = 0; i < possibleRoomPositions.Count; i++)
        {
            nextRoom.position.setPosition(possibleRoomPositions[i]);
            for (int j = 0; j <= corridors.Count - 2; j++)
            {
                if (nextRoom.isCollidingWithCorridor(corridors[j]))
                {
                    possibleRoomPositions.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }
}