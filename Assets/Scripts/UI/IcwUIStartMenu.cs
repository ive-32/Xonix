using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using TMPro;
using UnityEngine.EventSystems;

public class IcwUIStartMenu : MonoBehaviour
{
    public Toggle toggle;
    public GameObject levelbutton;

    private void Start()
    {
        GameObject.Find("UICanvas").transform.Find("StartGamePanel").gameObject.SetActive(true);
        GameObject.Find("UICanvas").transform.Find("SelectLevelPanel").gameObject.SetActive(false);

        PlayerPrefs.SetInt("UseGestures", 1);
    }

    public void OnUseGestureClick()
    {
        if (toggle.isOn)
            PlayerPrefs.SetInt("UseGestures", 1);
        else
            PlayerPrefs.SetInt("UseGestures", 0);
    }

    public void OnNewGameClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnSelectLevelClick()
    {
        GameObject.Find("UICanvas").transform.Find("StartGamePanel").gameObject.SetActive(false);
        GameObject selectpanel = GameObject.Find("UICanvas").transform.Find("SelectLevelPanel").gameObject;
        selectpanel.SetActive(true);
        IcwLevels tmplvl = gameObject.AddComponent<IcwLevels>();
        tmplvl.LoadLevelInfo();
        int levelcount = tmplvl.xmllevel.Length;
        int xcount = 3;
        RectTransform panelRectTransform = selectpanel.GetComponent<RectTransform>();
        float faspect = panelRectTransform.rect.width / panelRectTransform.rect.height;
        if (panelRectTransform.rect.width > panelRectTransform.rect.height) xcount = 4; 
        int icurrposx = 1, icurrposy = 1;
        for (int i = 0; i < levelcount; i++)
        {   // Set buttons to grid on panel
            float panelsizex = panelRectTransform.rect.width;
            float icellsizex = 0.9f / xcount;
            float icellsizey = icellsizex * faspect;
            float ibuttonsize = panelsizex * 0.9f / xcount * 0.8f;
            float ibuttonborder = ibuttonsize / panelsizex * 0.1f;
            
            GameObject butt = Instantiate(levelbutton);
            RectTransform rt = butt.GetComponent<RectTransform>();
            rt.SetParent(selectpanel.transform);
            rt.pivot = new Vector2(0.0f, 1.0f);
            rt.sizeDelta = new Vector2(ibuttonsize , ibuttonsize );

            rt.localPosition = new Vector3(0, 0, 0);
            rt.anchorMin = new Vector2(0.05f + ibuttonborder + icellsizex * (icurrposx - 1), 0.95f - icellsizey * (icurrposy - 1));
            rt.anchorMax = new Vector2(0.05f + ibuttonborder + icellsizex * (icurrposx - 1), 0.95f - icellsizey * (icurrposy - 1));
            if (icurrposx < xcount) icurrposx++; else { icurrposx = 1; icurrposy++; }

            GameObject txt = butt.transform.Find("Text").gameObject;
            TextMeshProUGUI tmptexxt = txt.GetComponent<TextMeshProUGUI>();
            string levelnumber = "Level" + (i + 1).ToString();
            tmptexxt.text = levelnumber + "\n" + tmplvl.xmllevel[i].name;
            butt.GetComponent<IcwLevelButton>().FillStars(levelnumber);
            butt.GetComponent<Button>().onClick.AddListener(OnSelectSomeLevelClick);
            butt.name = i.ToString();
        }
    }

    public void OnSelectSomeLevelClick()
    {
        int levelnumber = 0;
        int.TryParse(EventSystem.current.currentSelectedGameObject.name, out levelnumber);
        PlayerPrefs.SetInt("CurrentLevel", levelnumber);
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

}
