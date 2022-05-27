using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using Assets.Scripts;


public class IcwGame : MonoBehaviour
{

    static int fillroutine_deeplevel;
    private List<Vector3Int> fillpoints = new List<Vector3Int>();
    public enum EnumGameState { OnEarth, OnFlow }
    public static int sizeX = 25;
    public static int sizeY = 30;
    public static EnumGameState gamestate = EnumGameState.OnEarth;
    public TileBase floortile;
    public TileBase tracetile;
    public Tilemap floor;
    public GameObject grid;
    public int totalfilled = 0;
    public float targetfilled = 0.85f;
    float CurrentFilledPercent() => totalfilled / ((sizeX - 4.0f) * (sizeY - 4.0f));

    private GameObject player;
    private GameObject tenscores;
    private int[,] tmpfield = new int[sizeX, sizeY];
    public static List<GameObject> objects = new();
    private IcwScores scores;

    private void Start()
    {

        player = GameObject.Find("Player");
        floor = GameObject.Find("FloorTileMap").GetComponent<Tilemap>();
        grid = GameObject.Find("Grid");
        floortile = IcwService.GetTileByName("FloorTile");
        tracetile = IcwService.GetTileByName("TraceTile");
        scores = GameObject.Find("Scores").GetComponent<IcwScores>();
        totalfilled = 0;
    }

    public void PlaceFloorTile(int x, int y, TileBase _atile)
    {
        Vector3Int tileposition = new Vector3Int(x, y, 0);
        TileBase previoustile = floor.GetTile(tileposition);
        //GameObject traceobject = IcwService.GetPrefabByName("TraceObject");
        floor.SetTile(tileposition, _atile);
        if ((_atile==tracetile || _atile == floortile) && previoustile == null)
        {
            scores.AddScores(10, floor.GetCellCenterWorld(tileposition));
            if (_atile==tracetile) 
            {
                //GameObject tmpgo;
                //tmpgo = Instantiate(traceobject, floor.GetCellCenterWorld(tileposition), Quaternion.identity);
                //tmpgo.GetComponent<Animator>().Play("TraceObject");
                
            }
        }
        if (previoustile == tracetile)
        {
            //floor.ani
            //GameObject[] traceobjects = IcwService.GetAllObjectsByTag(floor.GetCellCenterWorld(tileposition), "TraceTag");
            //foreach (GameObject gm in traceobjects) Object.Destroy(gm);
        }
        if (_atile == null && previoustile == floortile) totalfilled--;
        if (_atile == floortile && previoustile != floortile) totalfilled++;
    }

    public void PlayerWasHit()
    {
        player.GetComponent<IcwPlayer>().currentpos = player.GetComponent<IcwPlayer>().startpositionbeforefloat;
        player.GetComponent<IcwPlayer>().PlayerVelocity = Vector2.zero;
        gamestate = EnumGameState.OnEarth;
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
            {
                Vector3Int tmptilepos = new Vector3Int(i, j, 0);
                if (floor.GetTile(tmptilepos) == tracetile)
                {
                    PlaceFloorTile(tmptilepos.x, tmptilepos.y, null);
                }
            }
    }

    private void FillFromPoint(Vector3Int startpoint, int value)
    {
        fillroutine_deeplevel++;
        if (startpoint.x >= sizeX || startpoint.x < 0 || startpoint.y >= sizeY || startpoint.y < 0) return;
        if (tmpfield[startpoint.x, startpoint.y] != 0) return;
        tmpfield[startpoint.x, startpoint.y] = value;
        Vector3 tmpvector = Vector3.up;
        for (int i = 0; i < 9; i++)
        {
            tmpvector = Quaternion.AngleAxis(45, Vector3.forward) * tmpvector;
            Vector3Int tmpvectorint = Vector3Int.RoundToInt(tmpvector);
            if (tmpfield[startpoint.x + tmpvectorint.x, startpoint.y + tmpvectorint.y] == 0)
            {
                if (fillroutine_deeplevel < 100) FillFromPoint(tmpvectorint + startpoint, value);
                else fillpoints.Add(tmpvectorint + startpoint);
            }
        }
        fillroutine_deeplevel--;
    }

    public void FillFieldAfterFlow()
    {   //fills captured area 
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
            {   // fill tmparray field values and change tracetiles to floortiles
                Vector3Int currtilepos = new Vector3Int(i, j, 0);
                TileBase currtb = floor.GetTile(currtilepos);
                if (currtb != null)
                {
                    tmpfield[i, j] = 2;
                    if (currtb.name == "TraceTile") PlaceFloorTile(currtilepos.x, currtilepos.y, floortile);
                } else tmpfield[i, j] = 0;
            }
        // get contignous area for each enemy 
        GameObject[] enemylist = GameObject.FindGameObjectsWithTag("Enemy");
        fillpoints.Clear();
        foreach (GameObject enemy in enemylist)
        {
            Vector3Int startpos = floor.WorldToCell(enemy.transform.position);
            fillpoints.Add(startpos);
        }
        while (fillpoints.Count > 0)
        {
            Vector3Int currfillpoint = fillpoints[^1];
            fillpoints.RemoveAt(fillpoints.Count - 1);
            FillFromPoint(currfillpoint, 1);
        }
        // fills areas where enemy not detected
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
                if (tmpfield[i, j] == 0) PlaceFloorTile(i, j, floortile);
    }

    public void PlayerOnTile(int x, int y)
    {
        Vector3Int playerpositionvector = new Vector3Int(x, y, 0);
        TileBase currtb = floor.GetTile(playerpositionvector);
        if (currtb != null)
        {
            if (gamestate != EnumGameState.OnEarth)
            {   //Finish flow
                Input.ResetInputAxes();
                player.GetComponent<IcwPlayer>().PlayerVelocity = Vector2.zero;
                FillFieldAfterFlow();
            }
            gamestate = EnumGameState.OnEarth;
            return;
        }
        if (IcwGame.gamestate == IcwGame.EnumGameState.OnEarth)
        {   //Start flow
            IcwGame.gamestate = IcwGame.EnumGameState.OnFlow;
        }
        if (gamestate == EnumGameState.OnFlow)
        {
            PlaceFloorTile(playerpositionvector.x, playerpositionvector.y, tracetile);
        }
        return;
    }
    public void PlayerMovingLogic(Vector3 startpos, Vector3 endpos)
    {   //check all tiles wich was intersected by player moving vector 
        Vector3Int start2dpos = floor.WorldToCell(startpos);
        Vector3Int end2dpos = floor.WorldToCell(endpos);
        if (end2dpos == start2dpos) return;
        if (gamestate == EnumGameState.OnEarth)
            player.GetComponent<IcwPlayer>().startpositionbeforefloat = floor.GetCellCenterWorld(start2dpos);
        Vector3Int direction = end2dpos - start2dpos;
        for (int i = 1; i <= direction.magnitude; i++)
        {
            start2dpos += direction;
            PlayerOnTile(start2dpos.x, start2dpos.y);
        };
        if (CurrentFilledPercent() >= targetfilled) LevelWin();
    }
    void LevelWin()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnGUI()
    {

        GUI.Label(new Rect(1,1,100,20), ((totalfilled )/((sizeX - 4.0f) * (sizeY - 4.0f))).ToString("00 %"));
        GUI.Label(new Rect(1, 20, 100, 20), (totalfilled).ToString("0"));
        GUI.Label(new Rect(1, 40, 100, 20), ((sizeX - 4) * (sizeY - 4)).ToString("0"));

    }


}

