using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwBooster : MonoBehaviour
{
    public enum BoostersType {Shield = 1, SpeedUp = 2, AdditionalLive = 3, SimpleEnemies = 4};
    public GameObject[] boosterspool;
    public bool OnField;
    public static float TimeSinceLastBooster = 0.0f;

    [System.NonSerialized] public BoostersType objtype;
    [System.NonSerialized] public float timetolive; // live time while booster stay on field 
    [System.NonSerialized] public float timetoeffect; // time while booster effect is enable
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        OnField = true;
        timetolive = IcwObjects.gamespeed * 10;
        timetoeffect = IcwObjects.gamespeed * 10;
    }
    public bool GetByPlayer()
    {
        return Vector3.Distance(IcwObjects.playerclass.gameObject.transform.position, this.transform.position) < 1.0f;
    }

    public static bool HaveBooster(BoostersType _atype)
    {
        for (int i = 0; i < IcwObjects.BoosterTiles.transform.childCount; i++)
        {
            IcwBooster tmpbooster = IcwObjects.BoosterTiles.transform.GetChild(i).gameObject.GetComponent<IcwBooster>();
            if (tmpbooster.objtype == _atype && !tmpbooster.OnField) return true;
        }
        return false;
    }
    public static IcwBooster TryToGetActiveBooster(BoostersType _atype)
    {
        for (int i = 0; i < IcwObjects.BoosterTiles.transform.childCount; i++)
        {
            IcwBooster tmpbooster = IcwObjects.BoosterTiles.transform.GetChild(i).gameObject.GetComponent<IcwBooster>();
            if (tmpbooster.objtype == _atype && !tmpbooster.OnField) return tmpbooster;
        }
        return null;
    }

    public static void AddBooster()
    {
        int index = Random.Range(0, IcwObjects.boosterclass.boosterspool.Length);
        GameObject newbooster;
        int x, y, i;
        i = 0;
        do
        {
            x = Random.Range(2, IcwGame.sizeX - 2);
            y = Random.Range(2, IcwGame.sizeY - 2);
            
        } while (IcwObjects.gridclass.fieldprojection[x, y] !=0 && i<10);
        if (IcwObjects.gridclass.fieldprojection[x, y] != 0) return;
        
        newbooster = Instantiate(IcwObjects.boosterclass.boosterspool[index]);
        newbooster.transform.position = IcwService.AttachToTile(new Vector3Int(x, y, 0));
        newbooster.transform.parent = IcwObjects.BoosterTiles.transform;
    }

    public virtual void ApplyEffect()
    {
        //Debug.LogWarning("Base class Apply effect " + this.GetType().Name);

    }


    // Update is called once per frame
    public virtual void Update()
    {
        if (this.name == "BonusTiles")
        {
            TimeSinceLastBooster += Time.deltaTime;
            if (TimeSinceLastBooster > Random.Range(30.0f, 35.0f)) 
            {
                TimeSinceLastBooster = Random.Range(0.0f, 15.0f);
                AddBooster();
            }
            return;
        }

        if (GetByPlayer())
        {
            IcwBooster tmpbooster = TryToGetActiveBooster(this.objtype);
            if (tmpbooster != null)
            {   //already have same booster. just update timetolive in active booster
                tmpbooster.timetolive = timetoeffect;
                Object.Destroy(this.gameObject);
                return;
            }

            this.ApplyEffect();
            OnField = false;

            Vector3 pos = IcwService.AttachToTile(new Vector3(-1 , IcwGame.sizeY - (int)objtype, 0));
            this.transform.position = pos;

            timetolive = timetoeffect;
        }

       
        timetolive -= Time.deltaTime;
        if (timetolive<0 ) 
        {   if (!OnField) 
            { 
                //Booster was picked up by player and must be destroyed carefully;
            }
            Object.Destroy(this.gameObject);
        }
    }
}
