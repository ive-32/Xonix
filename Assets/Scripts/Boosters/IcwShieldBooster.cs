using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwShieldBooster : IcwBooster 
{
    //private GameObject previoustracetileprefab;

    public override void Start()
    {
        base.Start();
        objtype = BoostersType.Shield;
        //previoustracetileprefab = IcwObjects.gridclass.tracetileprefab;
    }

    public override void ApplyEffect()
    {
        if (!HaveBooster(this.objtype))
        {
            IcwObjects.playerclass.transform.Find("ShieldEffect").gameObject.SetActive(true);
            //previoustracetileprefab = IcwObjects.gridclass.tracetileprefab;
            //IcwObjects.gridclass.tracetileprefab = IcwObjects.gridclass.fieldtileprefab;
            IcwObjects.playerlogicclass.currenttracetileobject = IcwGrid.FieldObjectsTypes.Field;
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
            //IcwObjects.gridclass.tracetileprefab = previoustracetileprefab;
            IcwObjects.playerlogicclass.currenttracetileobject = IcwGrid.FieldObjectsTypes.Trace;
        }
    }
}
