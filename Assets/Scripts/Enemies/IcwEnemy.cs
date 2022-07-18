using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class IcwEnemy : MonoBehaviour
    {
        public float enemyspeed = IcwObjects.gamespeed * 3.0f;
        public Rigidbody2D rg2d;
        protected Vector2 lastvelocity;
        protected Vector3Int oldposition;
        private float freezetime = 0;
        private bool ivegotscoresonthismove;
        public GameObject parent = null;
        public float timetolive = 10000;
        public float agro = 0;
        public bool saveMyParameters = false;


        protected virtual void Awake()
        {
            rg2d = GetComponent<Rigidbody2D>();
        }


        protected virtual void Start()
        {
            this.transform.SetParent(IcwObjects.Enemies.transform);

            if (!saveMyParameters)
            {
                transform.localPosition = new(Random.Range(3, IcwGame.sizeX - 3), Random.Range(3, IcwGame.sizeY - 3));
                rg2d.velocity = Random.insideUnitCircle.normalized * enemyspeed;
            }
            oldposition = IcwObjects.floor.WorldToCell(rg2d.position);
            ivegotscoresonthismove = false;
        }

        private void TryToGiveScore()
        {
            if (IcwObjects.playerlogicclass.playerstate == IcwPlayerLogic.EnumPlayerState.OnEarth) { ivegotscoresonthismove = false; return; }
            if (ivegotscoresonthismove) return;
            for (int i = 0; i < IcwObjects.TraceTiles.transform.childCount; i++)
            {
                if (Vector3.Distance(IcwObjects.TraceTiles.transform.GetChild(i).localPosition, this.transform.localPosition) < 2)
                {
                    IcwObjects.scoresclass.AddScores(100, IcwObjects.playerclass.startpositionbeforefloat, true, "!Sooo!\n!Close!");
                    ivegotscoresonthismove = true;
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (oldposition == IcwObjects.floor.WorldToCell(rg2d.position)) freezetime += Time.fixedDeltaTime; else freezetime = 0;
            if (rg2d.velocity.magnitude != enemyspeed) rg2d.velocity = rg2d.velocity.normalized * enemyspeed;
            if (freezetime > 2) { rg2d.velocity = Random.insideUnitCircle.normalized * enemyspeed; }
            {   // if velocity too close to axis
                float tmpvelvectortan = Mathf.Abs(rg2d.velocity.x / rg2d.velocity.y);
                Vector2 vel = rg2d.velocity;
                if (tmpvelvectortan < 0.2f) { vel.x *= 3; rg2d.velocity = vel.normalized * enemyspeed; }
                if (tmpvelvectortan > 5.0f) { vel.y *= 3; rg2d.velocity = vel.normalized * enemyspeed; }

            }


            oldposition = IcwObjects.floor.WorldToCell(rg2d.position);
            TryToGiveScore();

            if (timetolive < 10000) // for temporary enemies
            {
                if (timetolive > Time.fixedDeltaTime)
                {
                    timetolive -= Time.fixedDeltaTime;
                }
                else
                {
                    Object.Destroy(this.gameObject);
                    if (parent != null)
                    {
                        parent.SetActive(true);
                        parent.transform.localPosition = transform.localPosition;
                        parent.GetComponent<Rigidbody2D>().velocity = rg2d.velocity;
                    }
                }
            }
        }


        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            List<ContactPoint2D> ctp2dlist = new();
            collision.GetContacts(ctp2dlist);
            for (int i = 0; i < ctp2dlist.Count; i++)
            {
                if (ctp2dlist[i].collider.transform.parent == IcwObjects.TraceTiles.transform)
                {   //PlayerTrace hit by Enemy
                    //IcwObjects.gameclass.PlayerWasHit();
                    IcwTraceTilePrefab trtile;
                    trtile = ctp2dlist[i].collider.GetComponent<IcwTraceTilePrefab>();
                    trtile.HitByEnemy();
                    agro += 30; // add aggro for generate new enemy
                    return;
                }
            }

        }
    }
}