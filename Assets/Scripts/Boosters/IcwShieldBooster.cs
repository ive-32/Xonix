using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwShieldBooster : IcwBooster 
{
    public IcwGrid.FieldObjectsTypes newtracetile;
    private IcwGrid.FieldObjectsTypes prevoiustracetile;
    public override void Start()
    {
        base.Start();
        objtype = BoostersType.Shield;

    }

    public override void ApplyEffect()
    {
        if (!HaveBooster(this.objtype))
        {
            IcwObjects.playerclass.transform.Find("ShieldEffect").gameObject.SetActive(true);
            prevoiustracetile = IcwObjects.playerlogicclass.currenttracetileobject;
            IcwObjects.playerlogicclass.currenttracetileobject = newtracetile;
            IcwObjects.scoresclass.AddScores(100, new Vector3(IcwGame.sizeX / 2, 1, 0), true, "Iron\nButt\nnow");
        }
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
            IcwObjects.playerclass.transform.Find("ShieldEffect").gameObject.SetActive(false);
            IcwObjects.playerlogicclass.currenttracetileobject = prevoiustracetile;
        }
    }
}
