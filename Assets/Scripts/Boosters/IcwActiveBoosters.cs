using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwActiveBoosters : MonoBehaviour
{
    Dictionary<IcwBooster.BoostersType, float> activeboosters = new Dictionary<IcwBooster.BoostersType, float>();

    private void AddBooster(IcwBooster.BoostersType _boostername, float _timetolive)
    {
        if (activeboosters.ContainsKey(_boostername)) activeboosters[_boostername] += _timetolive;
        else activeboosters.Add(_boostername, _timetolive);
       
    }

    private bool HaveBooster(IcwBooster.BoostersType _boostername)
    {
        return activeboosters.ContainsKey(_boostername);
    }

    void Update()
    {
       /* if (activeboosters.Count > 0)
        {
            List<IcwBooster.BoostersType> keys = new List<IcwBooster.BoostersType>(activeboosters.Keys);
            foreach (IcwBooster.BoostersType akey in keys)
            {
                activeboosters[akey] -= Time.deltaTime;
                if (activeboosters[akey] < 0)
                {
                    IcwObjects.playerclass.transform.Find("ShieldEffect").gameObject.SetActive(false);
                    activeboosters.Remove(akey);
                }
            }
        }*/
    }
}
