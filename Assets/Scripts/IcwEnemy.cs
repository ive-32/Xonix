using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IcwEnemy : MonoBehaviour
{
    public const float enemyspeed = 5.0f;
    protected Rigidbody2D rg2d;
    protected Vector2 lastvelocity;
    protected Vector2 enemyvelocity;
    protected GameObject FieldTiles;
    protected GameObject TraceTiles;
    protected Tilemap floor;
    protected IcwGame game;
    protected GameObject player;
    protected IcwScores scores;
    protected Vector3Int oldposition;
    private float freezetime = 0;
    private bool ivegotscoresonthismove;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        game = GameObject.Find("mainGame").GetComponent<IcwGame>();
        player = GameObject.Find("Player");
        FieldTiles = GameObject.Find("FieldTiles");
        TraceTiles = GameObject.Find("TraceTiles");
        scores = GameObject.Find("Scores").GetComponent<IcwScores>();
        transform.position = new(Random.Range(3, IcwGame.sizeX - 3), Random.Range(3, IcwGame.sizeY - 3));
        rg2d = GetComponent<Rigidbody2D>();
        floor = Object.FindObjectOfType<Tilemap>();
        enemyvelocity = Random.insideUnitCircle.normalized * enemyspeed;
        rg2d.velocity = enemyvelocity;
        oldposition = floor.WorldToCell(rg2d.position);
        ivegotscoresonthismove = false;
    }
    
    private void TryToGiveScore()
    {
        if (IcwGame.gamestate == IcwGame.EnumGameState.OnEarth) { ivegotscoresonthismove = false; return; }
        if (ivegotscoresonthismove) return;
        for (int i = 0; i < TraceTiles.transform.childCount; i++)
        {
            if (Vector3.Distance(TraceTiles.transform.GetChild(i).position, this.transform.position) < 2)
            {
                //scores.AddScores(100, scores.gameObject.transform.position, true, "!Sooo!\n!Close!");
                scores.AddScores(100, player.GetComponent<IcwPlayer>().startpositionbeforefloat, true, "!Sooo!\n!Close!");
                ivegotscoresonthismove = true;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (oldposition == floor.WorldToCell(rg2d.position)) freezetime += Time.fixedDeltaTime; else freezetime = 0;
        if (enemyvelocity.magnitude < enemyspeed) { enemyvelocity = enemyvelocity.normalized * enemyspeed; rg2d.velocity = enemyvelocity; }
        if (freezetime > 2 ) { enemyvelocity = Random.insideUnitCircle.normalized * enemyspeed; rg2d.velocity = enemyvelocity; }
        oldposition = floor.WorldToCell(rg2d.position);
        enemyvelocity = rg2d.velocity;
        TryToGiveScore();

    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        List<ContactPoint2D> ctp2dlist = new();
        collision.GetContacts(ctp2dlist);
        for (int i = 0; i < ctp2dlist.Count; i++)
        {
            if (ctp2dlist[i].collider.transform.parent == TraceTiles.transform)
            {   //PlayerTrace hit by Enemy
                game.PlayerWasHit();
                return;
            }
        }
        
    }
}
