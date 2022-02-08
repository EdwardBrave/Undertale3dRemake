using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTextSupport;

public class CharacterManager : MonoBehaviour
{

    public List<GameObject> CharacterList;

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("PlayText.NextDialogue", NextDialogue);
        EventCenter.GetInstance().AddEventListener("PlayText.Darren.None", Darren);
        EventCenter.GetInstance().AddEventListener("PlayText.Gun.None", Gun);
        EventCenter.GetInstance().AddEventListener("PlayText.Jimmy.None", Jimmy);
        EventCenter.GetInstance().AddEventListener("PlayText.Keyboard.None", Keyboard);
        EventCenter.GetInstance().AddEventListener("PlayText.Money.None", Money);
        EventCenter.GetInstance().AddEventListener("PlayText.Pager.Broken", Pager);
        EventCenter.GetInstance().AddEventListener("PlayText.Pager.Complete", Pager_C);
        EventCenter.GetInstance().AddEventListener("PlayText.Tube.None", Tube);
        EventCenter.GetInstance().AddEventListener("PlayText.Gwyneth.None", MainCharacter);
    }

    void NextDialogue()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].SetActive(false);
        }
    }

    void Darren() => CharacterList[0].SetActive(true);

    void Gun() => CharacterList[1].SetActive(true);

    void Jimmy() => CharacterList[2].SetActive(true);

    void Keyboard() => CharacterList[3].SetActive(true);

    void Money() => CharacterList[4].SetActive(true);

    void Pager() => CharacterList[5].SetActive(true);

    void Pager_C() => CharacterList[8].SetActive(true);

    void Tube() => CharacterList[6].SetActive(true);

    void MainCharacter() => CharacterList[7].SetActive(true);
}
