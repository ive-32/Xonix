using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts;

public class IcwScores : MonoBehaviour
{
    struct AtomicScoreShift
    {
        public int scorevalue;
        public int currtime;
        public Vector3 scoreposition;
        public bool keepunique;
        public bool isoptimized;
        public string comment;
    }

    private List<AtomicScoreShift> scoreshifts = new List<AtomicScoreShift>();
    public IcwScreenText scoresobject;
    public IcwScreenText filledpercentsobject;
    public IcwScreenText serviceline;

    public GameObject screentext;
    public GameObject splashscores;

   
    public int Scores 
    { 
        get { if (scoresobject != null) return scoresobject.value; else return 0; } 
        set { if (scoresobject != null) scoresobject.value = value; } 
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject body = Instantiate(screentext); 
        scoresobject = body.GetComponent<IcwScreenText>();
        scoresobject.nameofvalue = "Scores";
        scoresobject.numline = 0;

        body = Instantiate(screentext);
        filledpercentsobject = body.GetComponent<IcwScreenText>();
        filledpercentsobject.nameofvalue = "Completed %";
        filledpercentsobject.valuechangespeed = 10;
        filledpercentsobject.numline = 1;

        /*body = Instantiate(screentext);
        serviceline = body.GetComponent<IcwScreenText>();
        float dpi = Screen.dpi;
        if (dpi == 0) dpi = Mathf.Min(Screen.currentResolution.height, Screen.currentResolution.width) / 3;
        float positionsensitivity = dpi / 100;
        serviceline.nameofvalue = positionsensitivity.ToString()+ " | " + dpi;
        serviceline.valuechangespeed = 10;
        serviceline.numline = 2;
        */
    }
    public void AddScores(int value, Vector3 position = default, bool keepunique = false, string comment = "") 
    {
        int currenttimeforscoreshift = Time.renderedFrameCount;
        scoresobject.value += value;
        AtomicScoreShift tmpscoreshift = new AtomicScoreShift();
        tmpscoreshift.currtime = Time.renderedFrameCount;
        tmpscoreshift.scoreposition = position;
        tmpscoreshift.scorevalue = value;
        tmpscoreshift.keepunique = keepunique;
        tmpscoreshift.isoptimized = false;
        tmpscoreshift.comment = comment;
        if (keepunique) 
        {
            if (scoreshifts.Exists(item => item.keepunique && Vector3.Distance(item.scoreposition, position) < 0.1f)) return;
            scoreshifts.Add(tmpscoreshift); 
            return; 
        }
        if (position == default) return;
        scoreshifts.Add(tmpscoreshift);
    }

    private void OptimizeScoreAppearing()
    {
        List<AtomicScoreShift> listtooptimization;
        listtooptimization = scoreshifts.FindAll(item => !item.keepunique);
        if (listtooptimization.Count < 2) return;
        scoreshifts.RemoveAll(item => !item.keepunique);

        AtomicScoreShift optimizedscoreshift = new AtomicScoreShift();
        foreach(AtomicScoreShift sh in listtooptimization)
        {
            optimizedscoreshift.scoreposition += sh.scoreposition;
            optimizedscoreshift.scorevalue += sh.scorevalue;
        }
        optimizedscoreshift.isoptimized = true;
        optimizedscoreshift.keepunique = true;
        optimizedscoreshift.scoreposition = optimizedscoreshift.scoreposition / listtooptimization.Count;
        if (optimizedscoreshift.scorevalue > 500) optimizedscoreshift.comment = "!Good!";
        if (optimizedscoreshift.scorevalue > 800) optimizedscoreshift.comment = "!!!Strike!!!";
        scoreshifts.Add(optimizedscoreshift);
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreshifts.Count == 0) return;
        OptimizeScoreAppearing();
        foreach (AtomicScoreShift scoreshift in scoreshifts)
        {
            GameObject tmptenscores;
            tmptenscores = Instantiate(splashscores, scoreshift.scoreposition, Quaternion.identity);
            TextMeshPro tmptexxt = tmptenscores.GetComponentInChildren<TextMeshPro>();
            tmptexxt.text = "+" + scoreshift.scorevalue.ToString();
            if (scoreshift.comment != "") tmptexxt.text += "\n" + scoreshift.comment;
            Animator anim = tmptenscores.GetComponent<Animator>();
            anim.Play("SplashScore");
            Transform tmpaprtsys = tmptenscores.transform.Find("Particle System");
            if (scoreshift.isoptimized || scoreshift.comment != "")
            {
                tmpaprtsys.gameObject.SetActive(false);
                tmptenscores.transform.localScale = new Vector3(3, 3, 0);
            }
            else
            {
                tmpaprtsys.gameObject.SetActive(false);
            }
            Destroy(tmptenscores, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        scoreshifts.Clear();
    }
}
