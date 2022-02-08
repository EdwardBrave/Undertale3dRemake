using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Shifting : MonoBehaviour
{
    public List<Image> ImageList = new List<Image>();
    public int Index = 0;
    public int NextIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < ImageList.Count; i++)
        {
            ImageList[i].color = new Color(1, 1, 1, 0);
        }
        StartCoroutine(IE_Shift());
    }

    IEnumerator IE_Shift()
    {
        while (true)
        {
            if(NextIndex >= ImageList.Count)
                NextIndex = 0;
            if (Index >= ImageList.Count)
                Index = 0;

            ImageList[NextIndex].transform.SetSiblingIndex(ImageList[NextIndex].transform.parent.childCount - 1);
            ImageList[NextIndex].transform.localScale = new Vector3(1.2f, 1.2f, 1f);
            ImageList[NextIndex].transform.DOMoveX(ImageList[NextIndex].transform.position.x + 100f, 10f);

            yield return null;

            ImageList[NextIndex].DOColor(Color.white, 3f);
            yield return new WaitForSeconds(3f);

            ImageList[Index].color = new Color(1, 1, 1, 0);
            DOTween.Complete(ImageList[Index].transform);
            ImageList[Index].transform.position = new Vector3(ImageList[Index].transform.position.x - 100f, ImageList[Index].transform.position.y);

            Index++;
            NextIndex++;
        }
    }
}
