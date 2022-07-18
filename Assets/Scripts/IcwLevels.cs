using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using Assets.Scripts;

public class IcwLevels : MonoBehaviour
{
    public static int levelnum = 0;
    public GameObject[] enemypool;
    public Sprite[] backgroundpool;

    
    public xmlLevel[] xmllevel;
    public void LoadLevelInfo()
    {
        TextAsset source;
        source = Resources.Load("Levels") as TextAsset;

        IcwXMLLevels tmpxmlLevel = new IcwXMLLevels();
        var xmlSerializer = new XmlSerializer(tmpxmlLevel.GetType());
        using (TextReader sr = new StringReader(source.ToString()))
        {
            tmpxmlLevel = (IcwXMLLevels)xmlSerializer.Deserialize(sr);
        }
        xmllevel = tmpxmlLevel.level;
        return;
    }

    
    public void StartLevel()
    {
        IcwObjects.gridclass.PrepareLevel();
        GameObject bckg = GameObject.Find("Background");
        if (levelnum < xmllevel.Length && levelnum >= 0)
        {
            for(int i = 0; i< xmllevel[levelnum].enemy.Length; i++ )
            {
                int enemynumber;
                enemynumber = IcwGame.EnemyByName(xmllevel[levelnum].enemy[i].name);
                if (enemynumber >= 0 && enemynumber < enemypool.Length)
                    Instantiate(enemypool[enemynumber]);
            }
            Object[] newbckg = Resources.LoadAll(xmllevel[levelnum].background, typeof(Sprite));
            if (newbckg.Length > 0) bckg.GetComponent<UnityEngine.UI.Image>().sprite = (Sprite)newbckg[0];

        }
        else
        {
            for (int i=0; i < ((levelnum  % 10) / 3 + 2); i++)
                Instantiate(enemypool[0]);
            for (int i = 0; (i < (levelnum % 100) / 10 / 3 + 1); i++)
                Instantiate(enemypool[1]);
            bckg.GetComponent<UnityEngine.UI.Image>().sprite = backgroundpool[Random.Range(0, 3)];
        }

        bckg.GetComponent<UnityEngine.UI.Image>().color = new Color(Random.Range(128, 256), Random.Range(128, 256), Random.Range(128, 256)); //Random.ColorHSV();
    }
}


