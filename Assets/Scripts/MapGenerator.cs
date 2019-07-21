using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public char[,] map;
    public int rows=20, cols=20;
    public int maxRoomWidth=10, maxRoomHeight=10;
    public float density=0.5f;
    public float tunnelsDensity = 0.5f;

    // Use this for initialization
    void Start () {
        Debug.Log(Random.seed);
        initializeMap();
        generateTunnels();
        printMap();
	}
	// Update is called once per frame
	void Update () {
		
	}
    void initializeMap()
    {
        map = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map[i, j] = '#';
            }
        }
    }
    void printMap()
    {
        string mapDisplay="";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                mapDisplay += map[i, j];
            }
            mapDisplay += "\n";
        }
        Debug.Log(mapDisplay);
    }
    void generateTunnels()
    {
        int x, y, direction;
        float totalSpaceTaken = 0;
        float t1;
        bool a;
        //Debug.Log(totalSpaceTaken / (rows * cols - 2 * rows - 2 * (cols - 2)) < tunnelsDensity);
        do
        {
            x = Random.Range(1, rows - 1);
            y = Random.Range(1, cols - 1);
            direction = Random.Range(1, 5);
            generateBranch(x, y, direction, ref totalSpaceTaken);
        } while (totalSpaceTaken == 0);
        while (totalSpaceTaken / (rows * cols - 2 * rows - 2 * (cols - 2)) < tunnelsDensity)
        {
            do
            {
                x = Random.Range(1, rows - 1);
                y = Random.Range(1, cols - 1);
            } while (map[x, y] != 'o');
            direction = Random.Range(1, 5);
            generateBranch(x, y, direction, ref totalSpaceTaken);
            t1 = totalSpaceTaken / (rows * cols - 2 * rows - 2 * (cols - 2));
            a= totalSpaceTaken / (rows * cols - 2 * rows - 2 * (cols - 2)) < tunnelsDensity;
            //Debug.Log(totalSpaceTaken / (rows * cols) < tunnelsDensity);
        }
    }
    void generateBranch(int x, int y, int direction, ref float totalSpaceTaken, bool isConnected = false)
    {
        int[] possibleDirections = new int[3];
        int temp = 0;
        bool isColliding;
        int rand;
        switch (direction)
        {
            case 1:
                x--;
                break;
            case 2:
                x++;
                break;
            case 3:
                y--;
                break;
            case 4:
                y++;
                break;
        }
        if (x < rows - 1 && x > 0 && y < cols - 1 && y > 0 && map[x, y] != 'o')
        {
            for (int i = 1; i <= 4; i++)
            {
                if (i == direction)
                    continue;
                possibleDirections[temp] = i;
                temp++;
            }
            isColliding = false;
            switch (direction)
            {
                case 1:
                case 2:
                    if (map[x, y + 1] == 'o' || map[x, y - 1] == 'o')
                        isColliding = true;
                    break;
                case 3:
                case 4:
                    if (map[x - 1, y] == 'o' || map[x + 1, y] == 'o')
                        isColliding = true;
                    break;
            }
            if (!isColliding)
            {
                map[x, y] = 'o';
                totalSpaceTaken++;
                rand = Random.Range(1, 100);
                //if (rand <= 10)
                //{
                //    for (int i = 0; i < 3; i++)
                //    {
                //        generateBranch(x, y, possibleDirections[i], ref totalSpaceTaken);
                //    }
                //}else if(rand <= 15)
                //{
                //    rand = Random.Range(0, 3);
                //    for(int i=0; i<3; i++)
                //    {
                //        if(i!=rand)
                //            generateBranch(x, y, possibleDirections[i], ref totalSpaceTaken);
                //    }
                //}else if (rand <= 25)
                //{
                //    rand = Random.Range(0, 3);
                //    generateBranch(x, y, possibleDirections[rand], ref totalSpaceTaken);
                //}
                //else
                //{
                    generateBranch(x, y, direction, ref totalSpaceTaken);
                //}
            }
        }
    }
}
