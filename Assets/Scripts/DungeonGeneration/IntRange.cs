using System;

[Serializable]
public class IntRange{
    public int min;
    public int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
    public IntRange(int max)
    {
        this.min = 0;
        this.max = max;
    }

    public int Random()
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
}
