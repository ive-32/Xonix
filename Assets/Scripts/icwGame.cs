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
    public GameObject[] enemypool;
    [System.NonSerialized] public static int sizeX = 25;
    [System.NonSerialized] public static int sizeY = 30;
    [System.NonSerialized] public static EnumGameState gamestate = EnumGameState.OnEarth;
    [System.NonSerialized] public Tilemap floor;
    [System.NonSerialized] public GameObject grid;
    [System.NonSerialized] public IcwGrid gridclass;
    [System.NonSerialized] public float targetfilled = 0.85f;
    float CurrentFilledPercent() => gridclass.FieldTiles.transform.childCount / ((sizeX - 4.0f) * (sizeY - 4.0f));

    private IcwPlayer player;
    private IcwScores scores;
    private int[,] tmpfieldprojection; //= new int[sizeX, sizeY];
    static Dictionary<string, int> enemynames = new Dictionary<string, int>()
        {{"enemy", 0}, {"enemydestroyer", 1}, {"enemysuperdestroyer", 2}};

    public static int EnemyByName(string name)
    {
        int value = -1;
        enemynames.TryGetValue(name.ToLower(), out value);
        return value;
    }

    private void Awake()
    {
        Debug.LogWarning(Camera.main.pixelWidth);
        if (Application.platform == RuntimePlatform.Android)
        {
            sizeX = 25;
            sizeY = 30;
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer )
        {
            sizeX = 40;
            sizeY = 30;
        }
        player = GameObject.Find("Player").GetComponent<IcwPlayer>();
        floor = GameObject.Find("FloorTileMap").GetComponent<Tilemap>();
        grid = GameObject.Find("Grid");
        gridclass = grid.GetComponent<IcwGrid>();
        scores = GameObject.Find("Scores").GetComponent<IcwScores>();
    }

    private void Start()
    {
        tmpfieldprojection = new int[sizeX, sizeY];
        IcwLevels lvl = this.GetComponent<IcwLevels>();
        lvl.LoadLevelInfo();
        lvl.StartLevel();
    }

    public void ChangeTraceObjects(IcwGrid.FieldObjectsTypes newobject = IcwGrid.FieldObjectsTypes.Empty)
    {
        int childcount = gridclass.TraceTiles.transform.childCount;
        for (int i = childcount - 1; i>-1 ; i--)
        {
            Vector3 pos = gridclass.TraceTiles.transform.GetChild(i).position;
            gridclass.TraceTiles.transform.GetChild(i).gameObject.GetComponent<IcwTraceTilePrefab>().DestroyTile();
            if (newobject != IcwGrid.FieldObjectsTypes.Empty) 
                gridclass.AddTile(pos.x, pos.y, newobject);
        }
    }

    public void PlayerWasHit()
    {
        player.currentpos = player.startpositionbeforefloat;
        player.PlayerVelocity = Vector2.zero;
        gamestate = EnumGameState.OnEarth;
        ChangeTraceObjects();
    }

    private void FillFromPoint(Vector3Int startpoint, int value)
    {
        fillroutine_deeplevel++;
        if (startpoint.x >= sizeX || startpoint.x < 0 || startpoint.y >= sizeY || startpoint.y < 0) return;
        if (tmpfieldprojection[startpoint.x, startpoint.y] != 0) return;
        tmpfieldprojection[startpoint.x, startpoint.y] = value;
        Vector3 tmpvector = Vector3.up;
        for (int i = 0; i < 9; i++)
        {
            tmpvector = Quaternion.AngleAxis(45, Vector3.forward) * tmpvector;
            Vector3Int tmpvectorint = Vector3Int.RoundToInt(tmpvector);
            if (tmpfieldprojection[startpoint.x + tmpvectorint.x, startpoint.y + tmpvectorint.y] == 0)
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
                if (gridclass.fieldprojection[i, j] != (int)IcwGrid.FieldObjectsTypes.Empty)
                     tmpfieldprojection[i, j] = 2;
                else tmpfieldprojection[i, j] = 0;
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
                if (tmpfieldprojection[i, j] == 0) 
                {   
                    gridclass.AddFieldTile(i, j);
                    scores.AddScores(10, floor.GetCellCenterWorld(new Vector3Int(i, j, 0)));
                }
        ChangeTraceObjects(IcwGrid.FieldObjectsTypes.Field);
    }

    public void PlayerOnTile(int x, int y)
    {
        if (gridclass.fieldprojection[x,y]!=0)
        {
            if (gamestate != EnumGameState.OnEarth)
            {   //Finish flow
                Input.ResetInputAxes();
                player.PlayerVelocity = Vector2.zero;
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
            gridclass.AddTraceTile(x, y);
            scores.AddScores(1,floor.GetCellCenterWorld(new Vector3Int(x, y, 0)));
        }
        return;
    }
    public void PlayerMovingLogic(Vector3 startpos, Vector3 endpos)
    {   //check all tiles wich was intersected by player moving vector 
        Vector3Int start2dpos = floor.WorldToCell(startpos);
        Vector3Int end2dpos = floor.WorldToCell(endpos);
        if (end2dpos == start2dpos) return;
        if (gamestate == EnumGameState.OnEarth)
            player.startpositionbeforefloat = floor.GetCellCenterWorld(start2dpos);
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
        IcwLevels.levelnum++; 
        SceneManager.LoadScene("GameScene");
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
            {
                SceneManager.LoadScene("TitleScreenScene");
                return;
            }
        }
    }

    private void OnGUI()
    {
        scores.filledpercentsobject.value = Mathf.FloorToInt(CurrentFilledPercent()*100);
    }


}

