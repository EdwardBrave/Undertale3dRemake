using GraphSpace;
using PlayTextSupport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Here is a sample to demonstrate how to communicate with PlayText.
/// </summary>
public class Talking : MonoBehaviour
{
    public DialogueGraph StartGraph;

    public List<DialogueGraph> NextList;

    public bool LockE = false;

    private void Awake()
    {
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("NextGraph", NextGraph);
    }

    void NextGraph(List<EventValueClass> Values)
    {
        StartCoroutine(IE_NextGraph(Values[0].intValue));
    }

    IEnumerator IE_NextGraph(int index)
    {
        yield return null;
        Debug.Log(index);
        EventCenter.GetInstance().EventTriggered("PlayText.Play", NextList[index]);
    }

    private void Start()
    {
        EventCenter.GetInstance().EventTriggered("PlayText.Play", StartGraph);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !LockE)
        {
            EventCenter.GetInstance().EventTriggered("PlayText.Next");
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            EventCenter.GetInstance().EventTriggered("PlayText.OptionUp");
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            EventCenter.GetInstance().EventTriggered("PlayText.OptionDown");
        }

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    EventCenter.GetInstance().EventTriggered("PlayText.Stop");
        //}
    }
}
