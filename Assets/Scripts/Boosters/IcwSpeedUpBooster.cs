using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwSpeedUpBooster : IcwBooster
{
    float playeroldspeed;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        objtype = BoostersType.SpeedUp;
        playeroldspeed = IcwObjects.playerclass.playerspeed;
    }

    public override void ApplyEffect()
    {
        if (!HaveBooster(this.objtype))
        {
            playeroldspeed = IcwObjects.playerclass.playerspeed;
            IcwObjects.playerclass.playerspeed = IcwObjects.gamespeed * 6.0f;
        }
        IcwObjects.playerclass.transform.Find("SpeedUpEffect").gameObject.SetActive(true);
        IcwObjects.scoresclass.AddScores(100, new Vector3(IcwGame.sizeX / 2, 1, 0), true, "t-t-t--turbo!!!");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    private void OnDestroy()
    {
        if (!OnField)
        {
            IcwObjects.playerclass.playerspeed = playeroldspeed;
            IcwObjects.playerclass.transform.Find("SpeedUpEffect").gameObject.SetActive(false);
        }
    }
}
