using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPrinter : MonoBehaviour {

    public GameObject[] tiles;
    private MapGenerator mapGenerator;
    private string tilesRepresentations;

	// Use this for initialization
	void Start () {
        mapGenerator = GetComponent<MapGenerator>();
        tilesRepresentations = "@o#";
        generateDungeon(mapGenerator.map, mapGenerator.rows, mapGenerator.cols);

    }
	// Update is called once per frame
	void Update () {
		
	}
    void generateDungeon(char[,] map, int rows, int cols)
    {
        new GameObject("Map");
        GameObject cell;
        char tileReprezentation;
        Vector3 tilePosition=new Vector3();
        for(int i=0; i<rows; i++)
        {
           for(int j=0; j<cols; j++)
            {
                tileReprezentation = map[i, j];
                tilePosition.Set(i, j, 0);
                cell=Instantiate(tiles[tilesRepresentations.IndexOf(tileReprezentation)], tilePosition, transform.rotation);
                cell.transform.parent = GameObject.Find("Map").transform;
            }
        }
    }
}
