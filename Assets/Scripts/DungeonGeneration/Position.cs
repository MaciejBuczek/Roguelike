using System;

[Serializable]
public class Position {

    public int x;
    public int y;

    public Position()
    {

    }
    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Position(Position position)
    {
        this.x = position.x;
        this.y = position.y;
    }
    public void setPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void setPosition(Position position)
    {
        this.x = position.x;
        this.y = position.y;
    }
    public static bool operator ==(Position a, Position b)
    {
        if (a.x == b.x && a.y == b.y)
            return true;
        else
            return false;
    }
    public static bool operator !=(Position a, Position b)
    {
        if (a.x == b.x && a.y == b.y)
            return false;
        else
            return true;
    }
}
