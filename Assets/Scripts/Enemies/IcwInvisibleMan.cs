using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;

public class IcwInvisibleMan : IcwEnemy
{
    GameObject invisibleface;
    GameObject realface;
    GameObject partsystem;
    private float invisibleability = IcwObjects.gamespeed * 10;
    private float invisible = 0;


    protected override void Start()
    {
        invisibleface = this.transform.Find("InvisibleFace").gameObject;
        realface = this.transform.Find("RealFace").gameObject;
        partsystem = this.transform.Find("PartSystem").gameObject;
        invisible = Random.Range(-invisibleability, -invisibleability * 0.7f );
        base.Start();
    }


    protected override void FixedUpdate()
    {
        if (invisible < 0)
        {
            invisibleface.SetActive(false);
            realface.SetActive(true);
            partsystem.SetActive(false);
            invisible += Time.fixedDeltaTime;
            if (invisible>0) invisible = Random.Range(invisibleability *0.7f, invisibleability );
        } else
        {
            invisibleface.SetActive(true);
            realface.SetActive(false);
            partsystem.SetActive(true);
            invisible -= Time.fixedDeltaTime;
            if (invisible < 0) invisible = Random.Range(-invisibleability, -invisibleability * 0.7f);
        }

        base.FixedUpdate();
    }
}
