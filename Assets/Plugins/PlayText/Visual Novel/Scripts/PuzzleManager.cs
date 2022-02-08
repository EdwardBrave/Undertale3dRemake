using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GraphSpace;
using PlayTextSupport;

public class PuzzleManager : MonoBehaviour
{
    public Talking talking;
    [Space(10)]
    public List<DialogueGraph> Puzzle00_Graphs;
    public GameObject Costume;
    [Space(10)]
    public List<DialogueGraph> Puzzle01_Graphs;
    public GameObject Hardware;
    public GameObject Tool;
    [Space(10)]
    public List<DialogueGraph> Puzzle02_Graphs;
    public InputField inputField;
    public GameObject Password;
    [Space(10)]
    public string Grabbed;

    string CostumeChoose;
    string HardwareChoose;

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("Puzzle", Puzzle);

        //00
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("ChooseCostume", (x) => { 
            Costume.SetActive(true);
            talking.LockE = true;
        });
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("Puzzle0_God", (x) => {
            if(CostumeChoose == "Angle")
            {
                PlayGraph(Puzzle00_Graphs[2]);
            }
            else
            {
                PlayGraph(Puzzle00_Graphs[3]);
            }
        });
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("Puzzle0_Outstock", (x) => {
            if (CostumeChoose == "Angle")
            {
                PlayGraph(Puzzle00_Graphs[3]);
            }
            else
            {
                PlayGraph(Puzzle00_Graphs[2]);
            }
        });

        //01
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("ChooseHardware", (x) => {
            Hardware.SetActive(true);
            talking.LockE = true;
        });
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("ChooseTool", (x) => {
            Tool.SetActive(true);
            talking.LockE = true;
        });

        //02
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("EnterCode", (x) => {
            Password.SetActive(true);
            talking.LockE = true;
        });

        //CheckWhatPlayerGrabbed
        EventCenter.GetInstance().AddEventListener<List<ReceiveValueClass>>("CheckWhatPlayerGrabbed", CheckWhatPlayerGrabbed);
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("PlayerGrab", PlayerGrab);

        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("End", End);
    }

    void End(List<EventValueClass> v)
    {
        SceneManager.LoadScene("StartScene");
    }

    void Puzzle(List<EventValueClass> Value)
    {
        if (Value[0].intValue == 0)
        {
            PlayGraph(Puzzle00_Graphs[0]);
            Debug.Log("Puzzle 0");
        }
        else if (Value[0].intValue == 1)
        {
            PlayGraph(Puzzle01_Graphs[0]);
            Debug.Log("Puzzle 1");
        }
        else if (Value[0].intValue == 2)
        {
            PlayGraph(Puzzle02_Graphs[0]);
            Debug.Log("Puzzle 2");
        }
    }

    public void BadCostume()
    {
        CostumeChoose = "Bad";
        PlayGraph(Puzzle00_Graphs[1]);
        Costume.SetActive(false);
        talking.LockE = false;
    }

    public void AngleCostume()
    {
        CostumeChoose = "Angle";
        PlayGraph(Puzzle00_Graphs[1]);
        Costume.SetActive(false);
        talking.LockE = false;
    }

    public void Shovel()
    {
        HardwareChoose = "Shovel";
        PlayGraph(Puzzle01_Graphs[1]);
        Hardware.SetActive(false);
        talking.LockE = false;
    }

    public void Cage()
    {
        HardwareChoose = "Cage";
        PlayGraph(Puzzle01_Graphs[1]);
        Hardware.SetActive(false);
        talking.LockE = false;
    }

    public void Cheese()
    {
        if(HardwareChoose == "Cage")
        {
            PlayGraph(Puzzle01_Graphs[2]);
        }
        else
        {
            PlayGraph(Puzzle01_Graphs[3]);
        }
        Tool.SetActive(false);
        talking.LockE = false;
    }

    public void FishNet()
    {
        if (HardwareChoose == "Cage")
        {
            PlayGraph(Puzzle01_Graphs[3]);
        }
        else
        {
            PlayGraph(Puzzle01_Graphs[2]);
        }
        Tool.SetActive(false);
        talking.LockE = false;
    }

    public void PassEnter()
    {
        if(inputField.text == "3975")
        {
            PlayGraph(Puzzle02_Graphs[1]);
        }
        else
        {
            PlayGraph(Puzzle02_Graphs[2]);
        }
        Password.SetActive(false);
        talking.LockE = false;
    }

    void PlayGraph(DialogueGraph graph)
    {
        StartCoroutine(IE_PG(graph));
    }

    void PlayerGrab(List<EventValueClass> Value)
    {
        Grabbed = Value[0].stringValue;
    }

    void CheckWhatPlayerGrabbed(List<ReceiveValueClass> Value)
    {
        Debug.Log("Checked");
        for (int i = 0; i < Value.Count; i++)
        {
            if(Value[i].Value == Grabbed)
            {
                ReceiveFromCode.FinishThisNode(Value, i);
                break;
            }
        }
    }

    IEnumerator IE_PG(DialogueGraph graph)
    {
        yield return null;
        EventCenter.GetInstance().EventTriggered("PlayText.Play", graph);
    }
}
