using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IcwScreenText : MonoBehaviour
{
    public int value = 0;
    public string objectcanvasname = "Canvas";
    public string nameofvalue;
    public int valuechangespeed;
    public int numline = 0;

    private GameObject canvas;
    private TextMeshProUGUI scoretextmeshobject;
    private int screenvalue = 0;

    protected virtual void ShowScreenValue()
    {
        if (scoretextmeshobject == null) return;
        if (nameofvalue != "")
            scoretextmeshobject.text = nameofvalue + ": " + screenvalue.ToString();
        else
            scoretextmeshobject.text = screenvalue.ToString();
    }
    public void SetLine(int i)
    {
        float canvasheight = canvas.GetComponent<RectTransform>().rect.height;
        float canvaswidth = canvas.GetComponent<RectTransform>().rect.width;
        scoretextmeshobject.ForceMeshUpdate();
        float lineheight = scoretextmeshobject.textInfo.lineInfo[0].lineHeight;
        //scoretextmeshobject.textInfo.
        //Debug.LogWarning("LineHeight " + scoretextmeshobject.textInfo.lineInfo[0].lineHeight.ToString());
        this.gameObject.transform.position = new Vector3(canvaswidth / 20, canvasheight - canvasheight / 20 - lineheight * i, 0);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        screenvalue = value;
        canvas = GameObject.Find(objectcanvasname);
        if (canvas == null) return;
        transform.SetParent(canvas.transform);
        float canvasheight = canvas.GetComponent<RectTransform>().rect.height;
        float canvaswidth = canvas.GetComponent<RectTransform>().rect.width;
        scoretextmeshobject = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        float _fontSize = canvasheight / 30;
        if (scoretextmeshobject != null)
        {
            RectTransform rt = scoretextmeshobject.GetComponent<RectTransform>();
            if (rt != null) rt.sizeDelta = new Vector2(canvaswidth * 0.8f, rt.rect.height);
            scoretextmeshobject.fontSize = _fontSize;
            SetLine(numline);
            ShowScreenValue();
        }
        if (valuechangespeed==0) valuechangespeed = 100;
    }

    protected virtual void OnGUI()
    {
        if (screenvalue != value)
        {
            int scoreschange = Mathf.RoundToInt(Mathf.Clamp(value - screenvalue, -1, 1) * valuechangespeed * Time.deltaTime);
            if (Mathf.Abs(scoreschange) > Mathf.Abs(value - screenvalue))
                scoreschange = value - screenvalue;
            screenvalue += scoreschange;
            ShowScreenValue();
        }
    }
}
