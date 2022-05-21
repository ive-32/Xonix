using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Assets.Scripts;


public class IcwGrid : MonoBehaviour
{
    public GameObject basetile;
    public Tilemap floor;
    private TileBase floortile;

    // Start is called before the first frame update
    public void AddFloor(Vector2Int _coord)
    {
        AddFloor(_coord.x, _coord.y);
    }

    public void AddFloor(int x, int y)
    {
        floor.SetTile(new Vector3Int(x, y, 0), floortile);
    }


    private GameObject Addtile(GameObject tile, Vector2Int _coord)
    {
        IcwGameClass.GameTile gametile;
        gametile = new IcwGameClass.GameTile
        {
            coords = _coord,
            body = Instantiate(tile, floor.GetCellCenterWorld(((Vector3Int)_coord)), Quaternion.identity)
        };
        IcwGameClass.objects.Add(gametile);
        return gametile.body;
    }

    


    void Start()
    {
        floortile = IcwService.GetTileByName("Floor");
        for (int i = 0; i < IcwGameClass.sizeX ; i++)
        {
            for (int j = 0; j < 2; j++) AddFloor(i, j); 
            for (int j = IcwGameClass.sizeY - 2; j < IcwGameClass.sizeY; j++) AddFloor(i, j); 
        }
        
        for (int j = 2; j < IcwGameClass.sizeY - 2; j++)
        {
            for (int i = 0; i < 2; i++) AddFloor(i, j);
            for (int i = IcwGameClass.sizeX - 2; i < IcwGameClass.sizeX ; i++) AddFloor(i, j);
        }

        {
            GameObject enemy = IcwService.GetPrefabByName("Enemy");
            Vector2Int enemypos = new(Random.Range(3, IcwGameClass.sizeX - 3), Random.Range(3, IcwGameClass.sizeY - 3));
            GameObject bd = Addtile(enemy, enemypos);
            Rigidbody2D rg2d = bd.GetComponent<Rigidbody2D>();
            Vector3 vel = Random.insideUnitCircle.normalized * 5.0f;
            rg2d.velocity = vel;
        }

        {
            GameObject enemy = IcwService.GetPrefabByName("EnemyDestroyer");
            Vector2Int enemypos = new(Random.Range(3, IcwGameClass.sizeX - 3), Random.Range(3, IcwGameClass.sizeY - 3));
            GameObject bd = Addtile(enemy, enemypos);
            Rigidbody2D rg2d = bd.GetComponent<Rigidbody2D>();
            Vector3 vel = Random.insideUnitCircle.normalized * 5.0f;
            rg2d.velocity = vel;
        }

    }



}
