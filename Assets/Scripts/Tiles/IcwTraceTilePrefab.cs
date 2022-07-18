using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwTraceTilePrefab : IcwBaseTile
{
    [System.NonSerialized] public float damaged;
    [System.NonSerialized] float damagespeed = IcwObjects.gamespeed * 0.075f;
    SpriteRenderer spriterenderer;
    protected override void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        objtype = IcwGrid.FieldObjectsTypes.Trace;
        base.Start();
        transform.SetParent(IcwObjects.TraceTiles.transform);
        damaged = 10000;
    }

    public void HitByEnemy()
    {
        damaged = damagespeed;
        spriterenderer.color = Color.red;
        if (Vector3.Distance(IcwObjects.playerclass.transform.position, this.transform.position) < 1.2f)
            IcwObjects.playerlogicclass.PlayerWasHit();
    }

    public override void Update()
    {
        if (damaged < 10000)
        {
            damaged -= Time.deltaTime;
            if (damaged<0)
            {
                for (int i = 0; i < IcwObjects.TraceTiles.transform.childCount; i++)
                {
                    if (Vector3.Distance(IcwObjects.TraceTiles.transform.GetChild(i).position, this.transform.position) < 1.2f)
                        IcwObjects.TraceTiles.transform.GetChild(i).GetComponent<IcwTraceTilePrefab>().HitByEnemy();
                }
            }
        }
        base.Update();
    }
    public override void DestroyTile()
    {
        base.DestroyTile();
    }
}
