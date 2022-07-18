using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IcwScreenText : MonoBehaviour
{
    public int value = 0;
    public int valuechangespeed;
    private int screenvalue = 0;

    private TextMeshProUGUI scoretextmeshobject;

    protected virtual void ShowScreenValue()
    {
        if (scoretextmeshobject == null) return;
        scoretextmeshobject.text = screenvalue.ToString();
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        screenvalue = value;
        scoretextmeshobject = this.GetComponent<TextMeshProUGUI>();

        ShowScreenValue();
        if (valuechangespeed==0) valuechangespeed = 100;
    }

    protected virtual void OnGUI()
    {
        if (screenvalue != value)
        {
            int scoreschange = Mathf.RoundToInt(Mathf.Clamp(value - screenvalue, -1, 1) * valuechangespeed * Time.deltaTime);
            if (scoreschange == 0) 
                scoreschange = Mathf.Clamp(value - screenvalue, -1, 1);
            if (Mathf.Abs(scoreschange) > Mathf.Abs(value - screenvalue))
                scoreschange = value - screenvalue;
            screenvalue += scoreschange;
            ShowScreenValue();
        }
    }
}
