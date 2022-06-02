using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;


public class IcwGrid : MonoBehaviour
{
    public enum FieldObjectsTypes {Empty = 0, Trace = 5, Field = 10, Border = 20};
    public GameObject floortileprefab;
    public GameObject tracetileprefab;
    public GameObject bordertileprefab;

    [System.NonSerialized] public GameObject BorderTiles;
    [System.NonSerialized] public GameObject FieldTiles;
    [System.NonSerialized] public GameObject TraceTiles;
    [System.NonSerialized] public Tilemap floor;
    [System.NonSerialized] public int[,] fieldprojection = new int[IcwGame.sizeX, IcwGame.sizeY];

    private GameObject AddObjectAtTile(GameObject obj, Vector2Int _coord)
    {
        GameObject body = Instantiate(obj, floor.GetCellCenterWorld(((Vector3Int)_coord)), Quaternion.identity);
        return body;
    }

    public void AddBorder(int x, int y)
    {
        AddObjectAtTile(bordertileprefab, new Vector2Int(x, y));
    }

    public void AddFieldTile(int x, int y)
    {
        AddObjectAtTile(floortileprefab, new Vector2Int(x, y));
    }

    public void AddTraceTile(int x, int y)
    {
        AddObjectAtTile(tracetileprefab, new Vector2Int(x, y));
    }

    public void AddTile(float worldx, float worldy, FieldObjectsTypes newobject)
    {
        Vector3Int tilepos = floor.WorldToCell(new Vector3(worldx, worldy, 0));
        switch ((int)newobject)
        {
            case (int)FieldObjectsTypes.Border: AddBorder(tilepos.x, tilepos.y); break;
            case (int)FieldObjectsTypes.Field: AddFieldTile(tilepos.x, tilepos.y); break;
            case (int)FieldObjectsTypes.Trace: AddTraceTile(tilepos.x, tilepos.y); break;
        }
    }
    public void ClearTraceInFieldProjection(FieldObjectsTypes newvalue = FieldObjectsTypes.Empty)
    {
        for (int i = 0; i < IcwGame.sizeX; i++)
            for (int j = 0; j < IcwGame.sizeX; j++)
                if (fieldprojection[i, j] == (int)FieldObjectsTypes.Trace) 
                    fieldprojection[i, j] = (int)newvalue;
    }


    void Start()
    {
        floor = GameObject.Find("FloorTileMap").GetComponent<Tilemap>(); // this.gameObject.GetComponentInChildren<Tilemap>();
        BorderTiles = GameObject.Find("BorderTiles");
        FieldTiles = GameObject.Find("FieldTiles");
        TraceTiles = GameObject.Find("TraceTiles");

        for (int i = 0; i < IcwGame.sizeX; i++)
            for (int j = 0; j < IcwGame.sizeY; j++) fieldprojection[i, j] = 0;


        for (int i = 0; i < IcwGame.sizeX ; i++)
        {
            for (int j = 0; j < 2; j++) AddBorder(i, j); 
            for (int j = IcwGame.sizeY - 2; j < IcwGame.sizeY; j++) AddBorder(i, j); 
        }
        
        for (int j = 2; j < IcwGame.sizeY - 2; j++)
        {
            for (int i = 0; i < 2; i++) AddBorder(i, j);
            for (int i = IcwGame.sizeX - 2; i < IcwGame.sizeX ; i++) AddBorder(i, j);
        }

        /*{
            GameObject enemy = IcwService.GetPrefabByName("Enemy");
            Vector2Int enemypos = new(Random.Range(3, IcwGame.sizeX - 3), Random.Range(3, IcwGame.sizeY - 3));
            GameObject bd = AddObjectAtTile(enemy, enemypos);
        }

        {
            GameObject enemy = IcwService.GetPrefabByName("EnemyDestroyer");
            Vector2Int enemypos = new(Random.Range(3, IcwGame.sizeX - 3), Random.Range(3, IcwGame.sizeY - 3));
            GameObject bd = AddObjectAtTile(enemy, enemypos);
        }
        {
            GameObject enemy = IcwService.GetPrefabByName("EnemySuperDestroyer");
            Vector2Int enemypos = new(Random.Range(3, IcwGame.sizeX - 3), Random.Range(3, IcwGame.sizeY - 3));
            GameObject bd = AddObjectAtTile(enemy, enemypos);
        }*/


    }



}
