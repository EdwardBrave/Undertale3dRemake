using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayTextSupport;
using GraphSpace;
using DG.Tweening;

public class BackgroundManager : MonoBehaviour
{
    //0-School
    //1-Shelf
    public List<Image> BackGroundList = new List<Image>();
    public Image BlackScreen;
    Image currentActive = null;
    

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("ChangeStage", (x) => { StartCoroutine(IE_CS(x)); });
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("BGM", BGM);
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("StopBGM", StopBGM);
    }

    IEnumerator IE_CS(List<EventValueClass> Value)
    {
        if (currentActive != null)
        {
            if (currentActive.gameObject.name != Value[0].stringValue)
            {
                BlackScreen.DOColor(new Color(0, 0, 0, 1), 1.5f);
                yield return new WaitForSeconds(1.5f);
                currentActive.gameObject.SetActive(false);
                currentActive = BackGroundList.Find((x) => { return x.gameObject.name == Value[0].stringValue; });
                currentActive.gameObject.SetActive(true);
                BlackScreen.DOColor(new Color(0, 0, 0, 0), 1.5f);
                yield return new WaitForSeconds(1.5f);
            }
        }
        else
        {
            currentActive = BackGroundList.Find((x) => { return x.gameObject.name == Value[0].stringValue; });
            currentActive.gameObject.SetActive(true);
            BlackScreen.DOColor(new Color(0, 0, 0, 0), 1.5f);
            yield return new WaitForSeconds(1.5f);
        }
        EventNode.FinishThisNode(Value);
    }

    void BGM(List<EventValueClass> Value)
    {
        FrameWork.AudioMgr.GetInstance().PlayBackMusic("Audio/" + Value[0].stringValue, v:0.5f, FadeIn:true);
    }

    void StopBGM(List<EventValueClass> Value)
    {
        FrameWork.AudioMgr.GetInstance().StopBackMusic(true);
    }
}
