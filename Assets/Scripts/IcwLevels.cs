using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class IcwLevels : MonoBehaviour
{
    public static int levelnum = 0;
    public GameObject[] enemypool;
    public static int EnemyByName(string name) 
    { 
        Dictionary<string, int> dict = new Dictionary<string, int>() 
        {{"Enemy", 0}, {"EnemyDestroyer", 1}, {"EnemySuperDestroyer", 2}};
        int value = -1;
        dict.TryGetValue(name, out value);
        return value; 
    }

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
        if (levelnum < xmllevel.Length && levelnum >= 0)
        {
            for(int i = 0; i< xmllevel[levelnum].enemy.Length; i++ )
            {
                int enemynumber;
                enemynumber = EnemyByName(xmllevel[levelnum].enemy[i].name);
                if (enemynumber >= 0 && enemynumber < enemypool.Length)
                    Instantiate(enemypool[enemynumber]);
            }
        } else
        {
            for (int i=0; i < ((levelnum  % 10) / 3 + 2); i++)
                Instantiate(enemypool[0]);
            for (int i = 0; (i < (levelnum % 100) / 10 / 3 + 1); i++)
                Instantiate(enemypool[1]);

        }
    }
}


