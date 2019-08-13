using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Wall, Floor, Door, Empty,
}

public class DungeonTile {

    public TileType tileType;
    public Position position;

    public DungeonTile(TileType tileType, Position position)
    {
        this.tileType = tileType;
        this.position = position;
    }
}
