using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IcwLevelButton : MonoBehaviour
{
    public void FillStars(string levelname)
    {
        if (int.TryParse(levelname, out int levelnumber))
        {
            levelname = "Level" + levelnumber.ToString();
        }
        int levelstars = PlayerPrefs.GetInt(levelname);
        for (int j = 1; j <= levelstars; j++)
        {
            GameObject star = this.transform.Find("Star" + j.ToString()).gameObject;
            star.GetComponent<Image>().color = Color.yellow;
        }
    }
    public void FillStars(int levelnumber)
    {
        FillStars("Level" + levelnumber.ToString());
    }

}
