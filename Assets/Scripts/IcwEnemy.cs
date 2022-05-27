using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IcwEnemy : MonoBehaviour
{
    protected Rigidbody2D rg2d;
    protected Vector2 lastvelocity;
    protected Vector2 enemyvelocity;
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
        scores = GameObject.Find("Scores").GetComponent<IcwScores>();
        rg2d = GetComponent<Rigidbody2D>();
        floor = Object.FindObjectOfType<Tilemap>();
        enemyvelocity = Random.insideUnitCircle.normalized * 5.0f;
        rg2d.velocity = enemyvelocity;
        oldposition = floor.WorldToCell(rg2d.position);
        ivegotscoresonthismove = false;
    }


    protected virtual void FixedUpdate()
    {
        if (oldposition == floor.WorldToCell(rg2d.position)) freezetime += Time.fixedDeltaTime; else freezetime = 0;
        if (freezetime >2 ) enemyvelocity = Random.insideUnitCircle.normalized * 5.0f;
        rg2d.velocity = enemyvelocity;
        oldposition = floor.WorldToCell(rg2d.position);
        if (IcwGame.gamestate == IcwGame.EnumGameState.OnFlow && !ivegotscoresonthismove)
        {
            Vector3Int tileposition = floor.WorldToCell(this.gameObject.transform.position);
            for (int i = -2; i < 3; i++)
                for (int j = -2; j < 3; j++)
                    if (floor.GetTile(new Vector3Int(tileposition.x + i, tileposition.y + j, 0)) == game.tracetile)
                    {
                        ivegotscoresonthismove = true;
                        scores.AddScores(100, tileposition, true, comment: "!So Close!");
                        i = j = 3;
                    }
        }
        if (IcwGame.gamestate == IcwGame.EnumGameState.OnEarth) ivegotscoresonthismove = false;
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        List<ContactPoint2D> ctp2dlist = new();
        collision.GetContacts(ctp2dlist);
        Vector2 collisionnormal = Vector2.zero;
        foreach (ContactPoint2D ctp2d in ctp2dlist)
        {
            collisionnormal += ctp2d.normal;
            Vector3Int tileposition = floor.WorldToCell(ctp2d.point - 0.1f * ctp2d.normal);
            if (floor.GetTile(tileposition) == game.tracetile)
            {   //PlayerTrace hit by Enemy
                game.PlayerWasHit();
            }
        }
        enemyvelocity = Vector2.Reflect(enemyvelocity, collisionnormal.normalized);
    }   
}
