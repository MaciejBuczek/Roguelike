using UnityEngine;
using System.Collections.Generic;

public class Corridor
{
    enum Directions
    {
        North,West,South,East,
    }

    public List<Position> breakPoints = new List<Position>();
    public List<Position> doorPositions = new List<Position>();

    public bool generateCorridor(IntRange lengthRange, Room startingRoom, Room nextRoom, List<Room> rooms, List<Corridor> corridors)
    {
        Position currentPosition = new Position();
        List<Position> possibleRoomPositions = new List<Position>();
        Directions currentDirection = Directions.North;
        Directions newDirection = Directions.North;
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
            } while (corridorGeneration < 1000 && isCorridorCollidingWithRooms(rooms));
            if (corridorGeneration < 1000)
            {
                currentDirection = newDirection;
                possibleRoomPositions = findRoomStartingPositions(currentDirection, nextRoom, rooms, corridors);
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
    private void findCorridorStartingPosition(ref Directions currentDirection, Position currentPosition, Room startingRoom)
    {
        do
        {
            currentDirection = (Directions)Random.Range(0, 4);
            switch (currentDirection)
            {
                case Directions.North:
                    currentPosition.setPosition(startingRoom.position.x, Random.Range(startingRoom.position.y, startingRoom.position.y + (startingRoom.width - 1)));
                    break;
                case Directions.South:
                    currentPosition.setPosition(startingRoom.position.x + startingRoom.height - 1,
                        Random.Range(startingRoom.position.y, startingRoom.position.y + (startingRoom.width - 1)));
                    break;
                case Directions.West:
                    currentPosition.setPosition(Random.Range(startingRoom.position.x, startingRoom.position.x + (startingRoom.height - 1)), startingRoom.position.y);
                    break;
                case Directions.East:
                    currentPosition.setPosition(Random.Range(startingRoom.position.x, startingRoom.position.x + (startingRoom.height - 1)),
                        startingRoom.position.y + startingRoom.width - 1);
                    break;
            }
        } while (startingRoom.isPositionCollidingWithEntrances(currentPosition, (int)currentDirection));
    }
    private List<Position> findRoomStartingPositions(Directions currentDirection, Room nextRoom, List<Room> rooms, List<Corridor> corridors)
    {
        List<Position> possibleRoomPositions = new List<Position>();
        switch (currentDirection)
        {
            case Directions.North:
                for(int i=0; i<nextRoom.width; i++)
                {
                    possibleRoomPositions.Add(new Position(breakPoints[breakPoints.Count - 1].x - (nextRoom.height - 1), breakPoints[breakPoints.Count - 1].y - i));
                }
                break;
            case Directions.South:
                for (int i = 0; i < nextRoom.width; i++)
                {
                    possibleRoomPositions.Add(new Position(breakPoints[breakPoints.Count - 1].x, breakPoints[breakPoints.Count - 1].y - i));
                }
                break;
            case Directions.West:
                for (int i = 0; i < nextRoom.height; i++)
                {
                    possibleRoomPositions.Add(new Position(breakPoints[breakPoints.Count - 1].x - i, breakPoints[breakPoints.Count - 1].y - (nextRoom.width - 1)));
                }
                break;
            case Directions.East:
                for (int i = 0; i < nextRoom.height; i++)
                {
                    possibleRoomPositions.Add(new Position(breakPoints[breakPoints.Count - 1].x - i, breakPoints[breakPoints.Count - 1].y));
                }
                break;
        }       
        searchForRoomCollisionWithOtherRooms(possibleRoomPositions, nextRoom, rooms);
        searchForRoomCollisionWithCorridors(possibleRoomPositions, nextRoom, corridors);
        return possibleRoomPositions;
    }
    private void generateBreakPoints(Directions currentDirection, ref Directions newDirection, Position currentPosition, IntRange lengthRange)
    {
        int length, rand, continuedDirection = 0 ;
        Directions opositeDirection;
        Directions randomDirection;
        length = lengthRange.Random();
        breakPoints.Add(new Position(currentPosition.x, currentPosition.y));
        opositeDirection = (Directions)(((int)currentDirection + 2) % 4);
        for (int i = length; i >= 0; i--)
        {
            switch (currentDirection)
            {
                case Directions.North:
                    currentPosition.x--;
                    break;
                case Directions.South:
                    currentPosition.x++;
                    break;
                case Directions.West:
                    currentPosition.y--;
                    break;
                case Directions.East:
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
                    randomDirection = (Directions)Random.Range(0, 4);
                } while (randomDirection == opositeDirection || randomDirection == currentDirection);
                currentDirection = randomDirection;
                opositeDirection = (Directions)(((int)currentDirection + 2) % 4);
                breakPoints.Add(new Position(currentPosition.x, currentPosition.y));
                continuedDirection = 0;
            }
        }
        breakPoints.Add(new Position(currentPosition.x, currentPosition.y));
        newDirection = currentDirection;
    }
    private bool isCorridorCollidingWithRooms(List<Room> rooms)
    {
        Position position= new Position();
        Position nextPosition = new Position();
        bool isNotFirstIteration;
        for (int i = 0; i < rooms.Count - 1; i++)
        {
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
                    if (isNotFirstIteration && rooms[i].isCollidingWithPosition(position))
                        return true;
                    isNotFirstIteration = true;
                }
            }
        }
        return false;
    }
    private void searchForRoomCollisionWithOtherRooms(List<Position> possibleRoomPositions, Room nextRoom, List<Room> rooms)
    {   
        for (int i = 0; i < possibleRoomPositions.Count; i++)
        {
            nextRoom.position.setPosition(possibleRoomPositions[i]);
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