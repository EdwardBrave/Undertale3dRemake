using PlayTextSupport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MidgroundManager : MonoBehaviour
{
    public List<Image> MidGroundList = new List<Image>();
    Image currentActive;

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("MidG", (x) => { IE_CS(x); });
    }

    void IE_CS(List<EventValueClass> Value)
    {
        currentActive = MidGroundList.Find((x) => { return x.gameObject.name == Value[0].stringValue; });
        currentActive.gameObject.SetActive(Value[1].boolValue);
    }
}
