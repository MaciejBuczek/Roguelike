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
        Position roomPosition = new Position();
        List<Position> nextRoomPositions = new List<Position>();
        directions currentDirection = directions.North;
        directions newDirection = directions.North;
        int generationNo;
        findCorridorStartingPosition(ref currentDirection, currentPosition, startingRoom);
        generationNo = 0;
        do
        {
            do
            {
                breakPoints.Clear();
                doorPositions.Clear();
                generateBreakPoints(currentDirection, ref newDirection, new Position(currentPosition.x, currentPosition.y), lengthRange);
            } while (checkForCorridorCollision(rooms, dungeonRows, dungeonCols) && generationNo < 100);
            currentDirection = newDirection;
            findRoomStartingPosition(currentDirection, roomPosition, nextRoom);
            if (!checkForRoomCollision(roomPosition, nextRoom, dungeonRows, dungeonCols, rooms) && !checkForRoomCollisionWithCorridors(nextRoom, corridors))
            {
                startingRoom.entrancePositions.Add(new Position(breakPoints[0]));
                nextRoom.entrancePositions.Add(new Position(breakPoints[breakPoints.Count - 1]));
                return true;
            }
            generationNo++;
        } while (generationNo < 500);
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
    private void findRoomStartingPosition(directions currentDirection, Position roomPosition, Room nextRoom)
    {
        List<Position> roomStartingPositions = new List<Position>();
        switch (currentDirection)
        {
            case directions.North:
                
                roomPosition.setPosition(breakPoints[breakPoints.Count - 1].x - (nextRoom.height - 1), breakPoints[breakPoints.Count - 1].y);
                break;
            case directions.South:
                roomPosition.setPosition(breakPoints[breakPoints.Count - 1].x, breakPoints[breakPoints.Count - 1].y);
                break;
            case directions.West:
                roomPosition.setPosition(breakPoints[breakPoints.Count - 1].x, breakPoints[breakPoints.Count - 1].y - (nextRoom.width - 1));
                break;
            case directions.East:
                roomPosition.setPosition(breakPoints[breakPoints.Count - 1].x, breakPoints[breakPoints.Count - 1].y);
                break;
        }
        nextRoom.position.setPosition(roomPosition.x, roomPosition.y);
    }
    private void generateBreakPoints(directions currentDirection, ref directions newDirection, Position currentPosition, IntRange lengthRange)
    {
        int length, rand;
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
            if (i == 1 || i == length)
                doorPositions.Add(new Position(currentPosition.x, currentPosition.y));
            rand = Random.Range(0, 10);
            if (rand < 2 && i != 0 && i != length)
            {
                do
                {
                    randomDirection = (directions)Random.Range(0, 4);
                } while (randomDirection == opositeDirection || randomDirection == currentDirection);
                currentDirection = randomDirection;
                opositeDirection = (directions)(((int)currentDirection + 2) % 4);
                breakPoints.Add(new Position(currentPosition.x, currentPosition.y));
            }
        }
        breakPoints.Add(new Position(currentPosition.x, currentPosition.y));
        newDirection = currentDirection;
    }
    private bool checkForCorridorCollision(List<Room> rooms, int dungeonRows, int dungeonCols)
    {
        for(int i=0; i<breakPoints.Count; i++)
        {
            if(breakPoints[i].x <= 0 || breakPoints[i].x>=dungeonRows || breakPoints[i].y<=0 || breakPoints[i].y >= dungeonCols)
            {
                return true;
            }
        }
        for(int i=0; i<rooms.Count; i++)
        {
            for(int j=1; j<breakPoints.Count; j++)
            {
                if (rooms[i].isPositionCollidingWithRoom(breakPoints[j]))
                    return true;
            }
        }
        return false;
    }
    private bool checkForRoomCollision(Position roomPosition, Room nextRoom, int dungeonRows, int dungeonCols, List<Room> rooms)
    {
        if (roomPosition.x <= 0 || roomPosition.x + (nextRoom.height - 1) >= dungeonRows || roomPosition.y <= 0 || roomPosition.y + (nextRoom.width - 1) >= dungeonCols)
            return true;
        for(int i=0; i<=rooms.Count-2; i++)
        {
            if (nextRoom.isColidingWithOtherRoom(rooms[i]))
                return true;
        }
        return false;
    }
    private bool checkForRoomCollisionWithCorridors(Room nextRoom, List<Corridor> corridors)
    {
        for(int i=0; i<=corridors.Count-2; i++)
        {
            if (nextRoom.isCollidingWithCorridor(corridors[i]))
                return true;
        }
        return false;
    }
}