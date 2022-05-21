using System.Collections.Generic;
using UnityEngine;


public class IcwGameClass
{
    public static int sizeX = 40;
    public static int sizeY = 40;

    public class GameTile
    {
        public GameObject body;
        public Vector2Int coords;
    }

    public static bool IsInField(Vector3Int coords) 
    {
        bool res = (coords.x > 1 && coords.x < sizeX - 2 && coords.y > 1 && coords.y < sizeY - 2);
        return res;
    }


    public static List<GameTile> objects = new();
    public static GameTile GetTile(int x, int y)
    {
        //GameTile gmt = objects.Find(item => (item.coords.x == x) && (item.coords.y == y));
        return GetTile(new Vector2Int(x, y));
    }

    public static GameTile GetTile(Vector2Int coords)
    {
        GameTile gmt = objects.Find(item => ((item.coords.x == coords.x) && (item.coords.y == coords.y)));
        return gmt;
    }
    public static GameTile GetTile(Vector3Int coords)
    {
        return GetTile(new Vector2Int(coords.x, coords.y));
    }

}

