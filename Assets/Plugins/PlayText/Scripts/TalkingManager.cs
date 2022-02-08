using GraphSpace;
using PlayTextSupport;
using UnityEngine;

/// <summary>
/// Here is a sample to demonstrate how to communicate with PlayText.
/// </summary>
public class TalkingManager : MonoBehaviour
{
    public DialogueGraph Graph;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            EventCenter.GetInstance().EventTriggered("PlayText.Play", Graph);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            EventCenter.GetInstance().EventTriggered("PlayText.OptionUp");
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            EventCenter.GetInstance().EventTriggered("PlayText.OptionDown");
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            EventCenter.GetInstance().EventTriggered("PlayText.Stop");
        }
    }
}
