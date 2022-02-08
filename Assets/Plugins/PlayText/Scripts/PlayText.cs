using Cinemachine;
using DG.Tweening;
using GraphSpace;
using PlayTextSupport;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum STATE
{
    OFF,
    TYPING,
    PAUSED
}

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayText : MonoBehaviour
{
    #region Variable
    public enum Current
    {
        Dialogue,
        Option,
        Event,
        ReceiveFromCode
    }
    [HideInInspector]
    public DialogueGraph dialogueGraph;
    [HideInInspector]
    public DialogueProfile dialogueProfile;
    [HideInInspector]
    public CinemachineVirtualCamera cam;

    [Header("Typing")]
    [HideInInspector]
    public string Language;
    [HideInInspector]
    public float TypingSpeed;
    readonly float SkippingTypingSpeed = 5000f;
    Dictionary<int, List<string>> EventList = new Dictionary<int, List<string>>();
    XNode.Node CurrentNode;

    [Header("Display")]
    [HideInInspector]
    public Canvas canvas;
    CanvasScaler Scaler;
    [HideInInspector]
    public RectTransform Root;
    [HideInInspector]
    public Image Bubble;
    [HideInInspector]
    public RectTransform DisplayPanel;
    [HideInInspector]
    public TMP_Text DisplayText;
    [HideInInspector]
    public RectTransform OptionPanel;
    [HideInInspector]
    public Image BubblePointer;
    [HideInInspector]
    public float DefaultWidth = 560;
    [HideInInspector]
    public GameObject OptionObject;
    [HideInInspector]
    public Vector2 BubblePositionOffset;
    [HideInInspector]
    public Vector2 BubbleSizeOffset;
    [HideInInspector]
    public Vector2 PointerOffset;
    [HideInInspector]
    public Vector2 PointerLeftOffset;
    [HideInInspector]
    public Vector2 PointerRightOffset;
    [HideInInspector]
    public Vector2 PointerUpOffset;
    [HideInInspector]
    public bool BubbleRange;
    [HideInInspector]
    public Vector2 BubbleRangePosition;
    [HideInInspector]
    public Vector2 BubbleRangeSize;
    [HideInInspector]
    public bool ConvertHalfToFull;
    [HideInInspector]
    public bool IsBubbleFollow;
    [HideInInspector]
    public bool AllowCameraFollow = false;
    bool IsCameraFollow;
    bool IsShowBubble = false;
    [HideInInspector]
    public GameObject NextIcon;

    [Header("Effect")]
    [HideInInspector]
    public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.25f, 2.0f), new Keyframe(0.5f, 0), new Keyframe(0.75f, 2.0f), new Keyframe(1, 0f));
    [HideInInspector]
    public AnimationCurve BubblePositionCurve = new AnimationCurve(new Keyframe(0f, 0.5f), new Keyframe(0.5f, 1f), new Keyframe(1, 0.5f));
    [HideInInspector]
    public float SpeedMultiplier = 1.2f;
    [HideInInspector]
    public List<WaveEffect> waveEffects = new List<WaveEffect>();
    [HideInInspector]
    public List<JitterEffect> jitterEffects = new List<JitterEffect>();

    public class WaveEffect
    {
        public int StartIndex;
        public int EndIndex;
        public float CurveScale;
    }

    public class JitterEffect
    {
        public int StartIndex;
        public int EndIndex;
        public float AngleMultiplier;
        public float CurveScale;
    }

    [Header("Option")]
    int OptionIndex;
    public Color OptionColor = Color.white;
    Color DefaultColor = Color.white;
    bool IsOptionShowing = false;
    bool hasOptionShowed = false;
    List<GameObject> OptionGameObject = new List<GameObject>();

    Dictionary<string, Transform> TalkingPersonTransform = new Dictionary<string, Transform>();

    bool justEnter;
    bool IsFinished = false;

    public STATE state = STATE.OFF;
    public Current current = Current.Dialogue;

    float timervalue = 0f;
    int WordCount = 0;
    float DefaultTypingSpeed;

    int LastTimerValue = 0; 

    bool IsWaiting = false;
    bool IsWaitingTime = false;
    bool IsWaitingDefaultTime = false;
    float WaitingTime = 0.5f;
    float AlreadyWaitTime = 0f;

    bool IsTypingSpeed;
    bool IsSkipping;
    float ChangeTypingSpeed;

    bool IsChangingVolume;
    float Volume;

    float SizeRefSpeedX;
    float SizeRefSpeedY;

    bool EnableVertexEffect;

    Transform LastFollow;
    #endregion

    void Awake()
    {
        state = STATE.OFF;
        current = Current.Dialogue;
        DOTween.Init(true,true,LogBehaviour.Default);

        DefaultTypingSpeed = TypingSpeed;

        cam = GetComponent<CinemachineVirtualCamera>();
        Scaler = canvas.GetComponent<CanvasScaler>();
        if (AllowCameraFollow)
            FindPerson();

        //Here is the event that PlayText support. 
        EventCenter.GetInstance().AddEventListener<DialogueGraph>("PlayText.Play", StartTalking);
        EventCenter.GetInstance().AddEventListener("PlayText.Next", PlayTextNext);
        EventCenter.GetInstance().AddEventListener("PlayText.TalkingFinished", FinishTalking);
        EventCenter.GetInstance().AddEventListener("PlayText.Stop", StopTalking);
        EventCenter.GetInstance().AddEventListener<string>("PlayText.ChangeLanguage", ChangeLanguage);
        EventCenter.GetInstance().AddEventListener<int>("PlayText.OptionSelect", OptionSelect);
        EventCenter.GetInstance().AddEventListener<int>("PlayText.OptionHover", OptionHover);
        EventCenter.GetInstance().AddEventListener("PlayText.OptionUp", OptionUp);
        EventCenter.GetInstance().AddEventListener("PlayText.OptionDown", OptionDown);
    }

    void FindPerson()
    {
        foreach (var item in dialogueProfile.TalkingPerson)
        {
            GameObject obj = GameObject.Find(item.Name);
            if (obj != null)
                TalkingPersonTransform.Add(item.Name, obj.transform);
        }
    }

    void OptionUp()
    {
        if (current == Current.Option)
        {
            OptionIndex--;
            if (OptionIndex < 0)
            {
                OptionIndex = OptionGameObject.Count - 1;
            }
            if (IsChangingVolume)
                AudioMgr.GetInstance().PlayAudio(GetCurVoice(), Volume, false);
            else
                AudioMgr.GetInstance().PlayAudio(GetCurVoice(), 0.6f, false);
        }
    }

    void OptionDown()
    {
        if(current == Current.Option)
        {
            OptionIndex++;
            if (OptionIndex >= OptionGameObject.Count)
            {
                OptionIndex = 0;
            }
            if (IsChangingVolume)
                AudioMgr.GetInstance().PlayAudio(GetCurVoice(), Volume, false);
            else
                AudioMgr.GetInstance().PlayAudio(GetCurVoice(), 0.6f, false);
        } 
    }

    void FinishTalking()
    {
        IsFinished = true;
        cam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void StopTalking()
    {
        IsFinished = true;
        GoToState(STATE.OFF);
        cam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        dialogueGraph = null;
    }

    void ChangeLanguage(string Lan)
    {
        Language = Lan;
    }

    void OptionSelect(int Index)
    {
        OptionIndex = Index;
        PlayTextNext();
    }

    void OptionHover(int Index)
    {
        OptionIndex = Index;
    }

    private void Start()
    {
        state = STATE.OFF;
        current = Current.Dialogue;
    }

    void Update()
    {
        if (IsBubbleFollow)
        {
            if (!IsFinished)
                Bubble.rectTransform.sizeDelta = new Vector2(Mathf.SmoothDamp(Bubble.rectTransform.sizeDelta.x, DisplayPanel.sizeDelta.x + BubbleSizeOffset.x, ref SizeRefSpeedX, 0.15f), Mathf.SmoothDamp(Bubble.rectTransform.sizeDelta.y, DisplayPanel.sizeDelta.y + BubbleSizeOffset.y, ref SizeRefSpeedY, 0.15f));
            else
                Bubble.rectTransform.sizeDelta = new Vector2(20, 20);
        }

        if (AllowCameraFollow)
            cam.enabled = IsCameraFollow;
        else
            cam.enabled = false;

        if (current == Current.Dialogue)
        {
            if (dialogueGraph != null)
            {
                DialogueNode dia = dialogueGraph.current as DialogueNode;
                if (dia != null)
                {
                    if (TalkingPersonTransform.ContainsKey(dia.TalkingPerson))
                    {
                        Transform focusing = TalkingPersonTransform[dia.TalkingPerson];
                        if (!IsFinished)
                            cam.Follow = focusing;
                        else
                            cam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
                        if (IsBubbleFollow)
                        {
                            Vector2 screenPos = Camera.main.WorldToScreenPoint(focusing.position);//Change With Resolution
                            float Width = Screen.width;//Change With Resolution
                            float MidX = Width / 2;
                            Vector2 BubbleAnPos = new Vector2((BubblePositionCurve.Evaluate(screenPos.x / Width) * (screenPos.x - MidX)) + MidX, screenPos.y);
                            Vector2 PointerPosition = screenPos;
                            Vector2 PointerFinalOffset = PointerOffset;
                            if (Scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                            {
                                Vector2 CubePosition = (Vector2)canvas.transform.position + BubbleRangePosition;//Change With Resolution
                                Vector2 CubeSize = new Vector2(Screen.width * BubbleRangeSize.x, Screen.height * BubbleRangeSize.y);//Change With Resolution
                                Vector2 BubbleSizeDelta = Bubble.rectTransform.sizeDelta;//Never Change With Resolution
                                BubbleSizeDelta *= canvas.transform.localScale;//Convert to Change With Resolution;
                                if (BubbleAnPos.x + BubbleSizeDelta.x * (1 - Bubble.rectTransform.pivot.x) > CubePosition.x + (CubeSize.x / 2))//Right
                                {
                                    BubbleAnPos.x = CubePosition.x + (CubeSize.x / 2) - BubbleSizeDelta.x * (1 - Bubble.rectTransform.pivot.x);
                                    PointerPosition = BubbleAnPos;
                                    PointerFinalOffset = PointerRightOffset;
                                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, 90);
                                }
                                else if (BubbleAnPos.x - BubbleSizeDelta.x * (Bubble.rectTransform.pivot.x) < CubePosition.x - (CubeSize.x / 2))//Left
                                {
                                    BubbleAnPos.x = CubePosition.x - (CubeSize.x / 2) + BubbleSizeDelta.x * Bubble.rectTransform.pivot.x;
                                    PointerPosition = BubbleAnPos;
                                    PointerFinalOffset = PointerLeftOffset;
                                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, -90);
                                }
                                else if (BubbleAnPos.y + BubbleSizeDelta.y * (1 - Bubble.rectTransform.pivot.y) > CubePosition.y + (CubeSize.y / 2))//Up
                                {
                                    BubbleAnPos.y = CubePosition.y + (CubeSize.y / 2) - BubbleSizeDelta.y * (1 - Bubble.rectTransform.pivot.y);
                                    PointerPosition.y = CubePosition.y + (CubeSize.y / 2);
                                    PointerFinalOffset = PointerUpOffset;
                                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, 180);
                                }
                                else if (BubbleAnPos.y - BubbleSizeDelta.y * (Bubble.rectTransform.pivot.y) < CubePosition.y - (CubeSize.y / 2))//Down
                                {
                                    BubbleAnPos.y = CubePosition.y - (CubeSize.y / 2) + BubbleSizeDelta.y * Bubble.rectTransform.pivot.y;
                                    PointerPosition = BubbleAnPos;
                                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                                }
                                else
                                {
                                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                                }
                            }
                            Bubble.rectTransform.anchoredPosition = (BubbleAnPos / canvas.transform.localScale) - (Scaler.referenceResolution / 2) + BubblePositionOffset;//Anchor Never Change With Resolution
                            BubblePointer.rectTransform.anchoredPosition = (PointerPosition / canvas.transform.localScale) - (Scaler.referenceResolution / 2) + PointerFinalOffset;//Anchor Never Change With Resolution
                            if (cam.Follow != LastFollow)
                            {
                                Bubble.rectTransform.sizeDelta = new Vector2(20, 20);
                                LastFollow = cam.Follow;
                            }
                        }
                        else
                        {
                            if (cam.Follow != LastFollow)
                            {
                                LastFollow = cam.Follow;
                            }
                        }
                    }
                    else if(IsCameraFollow && AllowCameraFollow)
                    {
                        cam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
                        Debug.LogWarning("Couldn't Find Talking Person");
                    }
                }
            }
        }
        else if (current == Current.Option)
        {
            Transform focusing = GameObject.FindGameObjectWithTag("Player").transform;
            cam.Follow = focusing;
            if (cam.Follow != LastFollow)
            {
                if (IsBubbleFollow)
                    Bubble.rectTransform.sizeDelta = new Vector2(20, 20);
                LastFollow = cam.Follow;
            }
            if(IsBubbleFollow)
            {
                Vector2 screenPos = Camera.main.WorldToScreenPoint(focusing.position);//Change With Resolution
                float Width = Screen.width;//Change With Resolution
                float MidX = Width / 2;
                Vector2 BubbleAnPos = new Vector2((BubblePositionCurve.Evaluate(screenPos.x / Width) * (screenPos.x - MidX)) + MidX, screenPos.y);
                Vector2 PointerPosition = screenPos;
                Vector2 PointerFinalOffset = PointerOffset;
                Vector2 CubePosition = (Vector2)canvas.transform.position + BubbleRangePosition;//Change With Resolution
                Vector2 CubeSize = new Vector2(Screen.width * BubbleRangeSize.x, Screen.height * BubbleRangeSize.y);//Change With Resolution
                Vector2 BubbleSizeDelta = Bubble.rectTransform.sizeDelta;//Never Change With Resolution
                BubbleSizeDelta *= canvas.transform.localScale;//Convert to Change With Resolution;
                if (BubbleAnPos.x + BubbleSizeDelta.x * (1 - Bubble.rectTransform.pivot.x) > CubePosition.x + (CubeSize.x / 2))//Right
                {
                    BubbleAnPos.x = CubePosition.x + (CubeSize.x / 2) - BubbleSizeDelta.x * (1 - Bubble.rectTransform.pivot.x);
                    PointerPosition = BubbleAnPos;
                    PointerFinalOffset = PointerRightOffset;
                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, 90);
                }
                else if (BubbleAnPos.x - BubbleSizeDelta.x * (Bubble.rectTransform.pivot.x) < CubePosition.x - (CubeSize.x / 2))//Left
                {
                    BubbleAnPos.x = CubePosition.x - (CubeSize.x / 2) + BubbleSizeDelta.x * Bubble.rectTransform.pivot.x;
                    PointerPosition = BubbleAnPos;
                    PointerFinalOffset = PointerLeftOffset;
                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, -90);
                }
                else if (BubbleAnPos.y + BubbleSizeDelta.y * (1 - Bubble.rectTransform.pivot.y) > CubePosition.y + (CubeSize.y / 2))//Up
                {
                    BubbleAnPos.y = CubePosition.y + (CubeSize.y / 2) - BubbleSizeDelta.y * (1 - Bubble.rectTransform.pivot.y);
                    PointerPosition.y = CubePosition.y + (CubeSize.y / 2);
                    PointerFinalOffset = PointerUpOffset;
                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, 180);
                }
                else if (BubbleAnPos.y - BubbleSizeDelta.y * (Bubble.rectTransform.pivot.y) < CubePosition.y - (CubeSize.y / 2))//Down
                {
                    BubbleAnPos.y = CubePosition.y - (CubeSize.y / 2) + BubbleSizeDelta.y * Bubble.rectTransform.pivot.y;
                    PointerPosition = BubbleAnPos;
                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    BubblePointer.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                Bubble.rectTransform.anchoredPosition = (BubbleAnPos / canvas.transform.localScale) - (Scaler.referenceResolution / 2) + BubblePositionOffset;//Anchor Never Change With Resolution
                BubblePointer.rectTransform.anchoredPosition = (PointerPosition / canvas.transform.localScale) - (Scaler.referenceResolution / 2) + PointerFinalOffset;//Anchor Never Change With Resolution
            }
            if (OptionGameObject.Count >= 1)
            {
                if(OptionIndex >=OptionGameObject.Count)
                {
                    OptionIndex = 0;
                }
                foreach (var item in OptionGameObject)
                {
                    item.GetComponent<TMP_Text>().color = DefaultColor;
                }
                OptionGameObject[OptionIndex].TryGetComponent(out TMP_Text trytext);
                if (trytext != null)
                    trytext.color = OptionColor;
            }
        }

        switch (state)
        {
            case STATE.OFF:
                if (justEnter)
                {
                    if(NextIcon)
                        NextIcon.SetActive(false);
                    DisplayText.text = string.Empty;
                    LastFollow = null;
                    justEnter = false;
                }
                DisplayPanel.gameObject.SetActive(false);
                Bubble.enabled = false;
                BubblePointer.enabled = false;
                break;
            case STATE.TYPING:
                if (justEnter)
                {
                    if(NextIcon)
                        NextIcon.SetActive(false);
                    if (IsWaiting)
                    {
                        IsWaiting = false;
                    }
                    else
                    {
                        DisplayText.renderMode = TextRenderFlags.Render;
                        DisplayText.text = "";
                        timervalue = 0;
                        LastTimerValue = -1;
                        WordCount = 0;
                        if(EnableVertexEffect)
                            StopAnimate();
                        LoadContent();
                    }
                    justEnter = false;
                    if(current == Current.Dialogue)
                    {
                        DialogueNode dia = CurrentNode as DialogueNode;
                        EventCenter.GetInstance().EventTriggered("PlayText.NextDialogue");
                        EventCenter.GetInstance().EventTriggered("PlayText." + dia.TalkingPerson + "." + dia.Expression);
                        if (dia.Facing == FacingDirection.FacingPerson)
                        {
                            EventCenter.GetInstance().EventTriggered("PlayText." + dia.TalkingPerson + "." + dia.Facing.ToString(), dia.FacingPerson);
                            EventCenter.GetInstance().EventTriggered("PlayText." + dia.TalkingPerson + "." + dia.Expression + "." + dia.Facing, dia.FacingPerson);
                        }
                        else
                        {
                            EventCenter.GetInstance().EventTriggered("PlayText." + dia.TalkingPerson + "." + dia.Facing.ToString());
                            EventCenter.GetInstance().EventTriggered("PlayText." + dia.TalkingPerson + "." + dia.Expression + "." + dia.Facing);
                        }
                        IsShowBubble = dia.ShowBubble;
                        DisplayPanel.gameObject.SetActive(IsShowBubble);
                        Bubble.enabled = IsShowBubble;
                        if(IsBubbleFollow)
                            BubblePointer.enabled = IsShowBubble;

                        if (dia.Audio != null)
                        {
                            if (!dia.PlayPerChar)
                            {
                                if (IsChangingVolume)
                                    AudioMgr.GetInstance().PlayAudio(dia.Audio, Volume, false);
                                else
                                    AudioMgr.GetInstance().PlayAudio(dia.Audio, 0.35f, false);
                            }
                        }
                    }
                    else if (current == Current.Option)
                    {
                        IsShowBubble = true;
                        DisplayPanel.gameObject.SetActive(IsShowBubble);
                        Bubble.enabled = IsShowBubble;
                        if (IsBubbleFollow)
                            BubblePointer.enabled = IsShowBubble;
                    }
                }

                if (current == Current.Dialogue)
                {
                    DisplayPanel.gameObject.SetActive(IsShowBubble);
                    Bubble.enabled = IsShowBubble;
                    if (IsBubbleFollow)
                        BubblePointer.enabled = IsShowBubble;
                    if(!IsSkipping)
                    {
                        if (IsTypingSpeed)
                        {
                            TypingSpeed = ChangeTypingSpeed;
                        }
                        else
                        {
                            TypingSpeed = DefaultTypingSpeed;
                        }
                    }
                    else
                    {
                        IsWaitingDefaultTime = false;
                        IsWaitingTime = false;
                    }
                    if (IsWaitingDefaultTime)
                    {
                        if (AlreadyWaitTime >= 0.5f)
                        {
                            AlreadyWaitTime = 0f;
                            IsWaitingDefaultTime = false;
                        }
                        else
                        {
                            AlreadyWaitTime += Time.deltaTime;
                        }
                    }
                    else if (IsWaitingTime)
                    {
                        if (AlreadyWaitTime >= WaitingTime)
                        {
                            AlreadyWaitTime = 0f;
                            IsWaitingTime = false;
                        }
                        else
                        {
                            AlreadyWaitTime += Time.deltaTime;
                        }
                    }
                    else
                    {
                        UpdateContentString();
                    }
                    CheckTypingFinished();
                }
                else if(current == Current.Option)
                {
                    GoToState(STATE.PAUSED);
                }
                else if(current == Current.Event)
                {
                    EventNode node = dialogueGraph.current as EventNode;
                    if (!node.IsWaiting)
                    {
                        GoToState(STATE.PAUSED);
                        PlayTextNext();
                    }
                }
                else if(current == Current.ReceiveFromCode)
                {
                    ReceiveFromCode node = dialogueGraph.current as ReceiveFromCode;
                    if (!node.IsWaiting)
                    {
                        GoToState(STATE.PAUSED);
                        PlayTextNext();
                    }
                }
                break;
            case STATE.PAUSED:
                if (justEnter)
                {
                    //ShowTips();
                    justEnter = false;
                    TypingSpeed = DefaultTypingSpeed;
                    if(NextIcon)
                        NextIcon.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    //TODO: IEnumerator Next()

    void LoadContent()
    {
        if (current == Current.Dialogue)
        {
            DialogueNode dia = dialogueGraph.current as DialogueNode;
            string temp = dia.Dialogue[dia.curIndex];
            if (ConvertHalfToFull)
            {
                temp = temp.Replace("，", ",");
                temp = temp.Replace("？", "?");
                temp = temp.Replace("！", "!");
                temp = temp.Replace("；", ";");
                temp = temp.Replace("：", ":");
            }
            DisplayText.maxVisibleCharacters = 0;
            
            UpdateContent(temp, "(?<=\\<)[^\\>]+");
            DisplayText.maxVisibleCharacters = 0;
            if (EnableVertexEffect)
                StartCoroutine(Animate());
        }
    }

    void UpdateContent(string temp, string pattern)
    {
        EventList.Clear();
        waveEffects.Clear();
        jitterEffects.Clear();
        MatchCollection match = Regex.Matches(temp, pattern);
        int StartingIndex = 0;
        int SymbolLength = 0;
        EnableVertexEffect = true;
        for (int i = 0; i < match.Count; i++)
        {
            Match d_match = Regex.Match(temp.Substring(StartingIndex, temp.Length - StartingIndex - 1), pattern);
            if (d_match.Value[0].ToString() == "$")
            {
                string Value = d_match.Value;
                int StringNum = Value.Length + 2;
                int Index = d_match.Index - 1;
                temp = temp.Remove(Index + StartingIndex, StringNum);
                if (dialogueProfile != null)
                {
                    string InsertValue = dialogueProfile.GetValue(Value.Remove(0, 1), Language);
                    temp = temp.Insert(Index + StartingIndex, InsertValue);
                    StartingIndex += Index + InsertValue.Length;
                }
            }
            else
            {
                StartingIndex += d_match.Index + d_match.Value.Length;
                SymbolLength += d_match.Value.Length + 2;
            }
        }

        match = Regex.Matches(temp, pattern);
        StartingIndex = 0;
        SymbolLength = 0;
        int WaveCount = 0;
        int JitterCount = 0;
        EnableVertexEffect = true;

        for (int i = 0; i < match.Count; i++)
        {
            Match d_match = Regex.Match(temp.Substring(StartingIndex, temp.Length - StartingIndex - 1), pattern);
            string Value = d_match.Value;
            if(Value.Contains("p=") || Value.Contains("w=") || Value.Contains("sp=") || Value.Contains("v=") || Value.Equals("w") || Value.Equals("wi") || Value.Equals("c") || Value.Equals("sp") || Value.Equals("/sp") || Value.Equals("/v"))
            {
                int StringNum = Value.Length + 2;
                int Index = d_match.Index - 1;
                temp = temp.Remove(Index + StartingIndex, StringNum);
                if (EventList.ContainsKey(Index + StartingIndex - SymbolLength))
                {
                    EventList[Index + StartingIndex - SymbolLength].Add(Value);
                }
                else
                {
                    EventList.Add(Index + StartingIndex - SymbolLength, new List<string>() { Value });
                }
            }
            else if(Value.Contains("wa="))
            {
                int StringNum = Value.Length + 2;
                int Index = d_match.Index - 1;
                temp = temp.Remove(Index + StartingIndex, StringNum);
                float MatchCurveScale = float.Parse(Regex.Match(Value, @"(?<==).+").Value);
                waveEffects.Add(new WaveEffect {StartIndex = Index + StartingIndex - SymbolLength, EndIndex = Index + StartingIndex - SymbolLength, CurveScale = MatchCurveScale});
            }
            else if(Value.Contains("sh="))
            {
                int StringNum = Value.Length + 2;
                int Index = d_match.Index - 1;
                temp = temp.Remove(Index + StartingIndex, StringNum);
                string AfterE = Regex.Match(Value, @"(?<==).+").Value;
                AfterE = AfterE.Replace("，", ",");
                string[] ThreeValue = Regex.Split(AfterE, ",");
                jitterEffects.Add(new JitterEffect { StartIndex = Index + StartingIndex - SymbolLength, EndIndex = Index + StartingIndex - SymbolLength, AngleMultiplier = float.Parse(ThreeValue[0]), CurveScale = float.Parse(ThreeValue[1])});
            }
            else if(Value.Equals("/wa"))
            {
                int StringNum = Value.Length + 2;
                int Index = d_match.Index - 1;
                temp = temp.Remove(Index + StartingIndex, StringNum);
                if (WaveCount <= waveEffects.Count - 1)
                {
                    waveEffects[WaveCount].EndIndex = Index + StartingIndex - 1 - SymbolLength;
                }
                WaveCount++;
            }
            else if(Value.Equals("/sh"))
            {
                int StringNum = Value.Length + 2;
                int Index = d_match.Index - 1;
                temp = temp.Remove(Index + StartingIndex, StringNum);
                if (JitterCount <= jitterEffects.Count - 1)
                {
                    jitterEffects[JitterCount].EndIndex = Index + StartingIndex - 1 - SymbolLength;
                }
                JitterCount++;
            }
            else if(Value.Contains("sprite="))
            {
                EnableVertexEffect = false;
                StartingIndex += d_match.Index + Value.Length;
                SymbolLength += Value.Length + 1;
            }
            else
            {
                StartingIndex += d_match.Index + Value.Length;
                SymbolLength += Value.Length + 2;
            }
        }
        WordCount = temp.Length - SymbolLength;
        DisplayText.text = temp;
    }

    void UpdateContentString()
    {
        timervalue += Time.deltaTime * TypingSpeed;
        int diff = (int)Mathf.Floor(timervalue) - LastTimerValue;
        for (int g = 0; g < diff; g++)
        {
            int Min2 = (int)Mathf.Floor(LastTimerValue + g + 1);
            if (EventList.ContainsKey(Min2))
            {
                for (int i = 0; i < EventList[Min2].Count; i++)
                {
                    EventTrigger(EventList[Min2][i]);
                }
                EventList.Remove(Min2);
                if (IsWaiting)
                {
                    GoToState(STATE.PAUSED);
                }
            }

            if (((IsWaitingDefaultTime || IsWaitingTime) && !IsSkipping) || IsWaiting)
            {
                LastTimerValue--;
            }
            else
            {
                DialogueNode dia = dialogueGraph.current as DialogueNode;
                DisplayText.maxVisibleCharacters++;
                if (!IsSkipping)
                {
                    if (current == Current.Dialogue)
                    {
                        if(dia.Audio != null)
                        {
                            if (dia.PlayPerChar)
                            {
                                if (IsChangingVolume)
                                    AudioMgr.GetInstance().PlayAudio(dia.Audio, Volume, false);
                                else
                                    AudioMgr.GetInstance().PlayAudio(dia.Audio, 0.35f, false);
                            }
                        }

                        if (dia.Audio == null || dia.PlayTyping)
                        {
                            if (IsChangingVolume)
                                AudioMgr.GetInstance().PlayAudio(GetCurVoice(), Volume, false);
                            else
                                AudioMgr.GetInstance().PlayAudio(GetCurVoice(), 0.35f, false);
                        }
                    }
                }
            }
        }
        int Min = (int)Mathf.Floor(timervalue);
        if (IsWaiting || IsWaitingDefaultTime || IsWaitingTime)
        {
            LastTimerValue = Min - diff;
        }
        else
        {
            LastTimerValue = Min;
        }
    }

    AudioClip GetCurVoice()
    {
        for (int i = 0; i < dialogueProfile.TalkingPerson.Count; i++)
        {
            if(current == Current.Dialogue)
            {
                DialogueNode dia = dialogueGraph.current as DialogueNode;
                if(dialogueProfile.TalkingPerson[i].Name == dia.TalkingPerson)
                {
                    AudioClip Clip = dialogueProfile.TalkingPerson[i].Voice;
                    if (Clip != null)
                        return Clip;
                    else
                        continue;
                }
                else
                {
                    continue;
                }
            }
            else if(current == Current.Option)
            {
                if(i == 0)
                {
                    AudioClip Clip = dialogueProfile.TalkingPerson[i].Voice;
                    if(Clip != null)
                    {
                        return Clip;
                    }
                    else
                    {
                        return dialogueProfile.DefaultVoice;
                    }
                }
            }
        }
        return dialogueProfile.DefaultVoice;
    }

    void EventTrigger(string str)
    {
        switch (str)
        {
            case "w":
                IsWaitingDefaultTime = true;
                break;
            case "wi":
                IsWaiting = true;
                break;
            case "c":
                DisplayText.text = "";
                break;
            case "sp":
                IsTypingSpeed = true;
                ChangeTypingSpeed = DefaultTypingSpeed;
                break;
            case "/sp":
                IsTypingSpeed = false;
                break;
            case "/v":
                IsChangingVolume = false;
                break;
            default:
                break;
        }

        string BeforeComma = @".+(?=,)"; 
        string AfterComma = @"(?<=,).+";

        if (str.Contains("="))
        {
            string Beforepattern = @".+(?==)"; 
            string Afterpattern = @"(?<==).+";
            Match Bmatch = Regex.Match(str, Beforepattern);
            Match Amatch = Regex.Match(str, Afterpattern);
            string BValue = Bmatch.Value.ToLower();
            string AValue = Amatch.Value;
            switch (BValue)
            {
                case "w":
                    IsWaitingTime = true;
                    WaitingTime = float.Parse(AValue);
                    break;
                case "sp":
                    IsTypingSpeed = true;
                    ChangeTypingSpeed = float.Parse(AValue);
                    break;
                case "p":
                    Match m_intensity = Regex.Match(AValue, BeforeComma);
                    Match m_time = Regex.Match(AValue, AfterComma);
                    string intensity = m_intensity.Value;
                    string time = m_time.Value;
                    Root.DOShakePosition(float.Parse(time), float.Parse(intensity), 10, 50, false, true);
                    break;
                case "e":
                    Match m_event = Regex.Match(AValue, BeforeComma);
                    Match m_string = Regex.Match(AValue, BeforeComma);
                    string eventName = m_event.Value;
                    string stringName = m_string.Value;
                    EventCenter.GetInstance().EventTriggered(eventName, stringName);
                    break;
                case "v":
                    IsChangingVolume = true;
                    Volume = float.Parse(AValue);
                    break;
                default:
                    break;
            }
        }
    }

    void CheckTypingFinished()
    {
        if (state == STATE.TYPING)
        {
            if ((int)Mathf.Floor(timervalue) >= WordCount)
            {
                GoToState(STATE.PAUSED);
            }
        }
    }

    void GoToState(STATE next)
    {
        state = next;
        justEnter = true;
    }

    public void StartTalking(DialogueGraph Graph)
    {
        if(dialogueGraph != Graph)
        {
            EventCenter.GetInstance().AddEventListener("PlayText.Stop", StopTalking);
            dialogueGraph = Graph;
        }
        PlayTextNext();
    }

    public void PlayTextNext()
    {
        switch (state)
        {
            case STATE.OFF:
                if(dialogueGraph != null)
                {
                    IsFinished = false;
                    IsSkipping = false;
                    TypingSpeed = DefaultTypingSpeed;
                    dialogueGraph.SetStartPoint(Language);
                    dialogueGraph.Continue();
                    CurrentNode = dialogueGraph.current;
                    DialogueNode diaaa = dialogueGraph.current as DialogueNode;
                    OptionNode opttt = dialogueGraph.current as OptionNode;
                    
                    if (diaaa != null)
                    {
                        IsCameraFollow = diaaa.CameraFollow;
                        current = Current.Dialogue;
                        DisplayText.TryGetComponent(out LayoutElement LayEleText);
                        if (LayEleText != null && diaaa.Width != 0)
                        {
                            LayEleText.preferredWidth = diaaa.Width;
                        }
                        else if (LayEleText != null)
                        {
                            LayEleText.preferredWidth = DefaultWidth;
                        }
                        GoToState(STATE.TYPING);
                    }
                    else if (opttt != null)
                    {
                        IsCameraFollow = opttt.CameraFollow;
                        IsShowBubble = true;
                        current = Current.Option;
                        if (!hasOptionShowed)
                        {
                            OptionIndex = 0;
                            foreach (var item in OptionGameObject)
                            {
                                Destroy(item.gameObject);
                            }
                            OptionGameObject.Clear();
                            OptionPanel.TryGetComponent(out LayoutElement LayEle);
                            if (LayEle != null && opttt.Width != 0)
                            {
                                LayEle.preferredWidth = opttt.Width;
                            }
                            else if (LayEle != null)
                            {
                                LayEle.preferredWidth = DefaultWidth;
                            }
                            DisplayText.TryGetComponent(out LayoutElement LayEleText);
                            if (LayEleText != null && opttt.Width != 0)
                            {
                                LayEleText.preferredWidth = opttt.Width;
                            }
                            else if (LayEleText != null)
                            {
                                LayEleText.preferredWidth = DefaultWidth;
                            }
                            StartCoroutine(OptionShowing(opttt));
                        }
                        else
                        {
                            if (IsChangingVolume)
                                AudioMgr.GetInstance().PlayAudio(GetCurVoice(), Volume, false);
                            else
                                AudioMgr.GetInstance().PlayAudio(GetCurVoice(), 0.6f, false);
                            if (!IsOptionShowing)
                            {
                                foreach (var item in OptionGameObject)
                                {
                                    Destroy(item.gameObject);
                                }
                                OptionPanel.gameObject.SetActive(false);
                                OptionGameObject.Clear();
                                hasOptionShowed = false;
                                dialogueGraph.Continue(OptionIndex);
                                OptionIndex = 0;
                                PlayTextNext();
                            }
                        }
                    }
                    else if (dialogueGraph.current is EventNode)
                    {
                        current = Current.Event;
                        GoToState(STATE.TYPING);
                    }
                    else if (dialogueGraph.current is ReceiveFromCode)
                    {
                        current = Current.ReceiveFromCode;
                        GoToState(STATE.TYPING);
                    }
                    GoToState(STATE.TYPING);
                }
                break;
            case STATE.TYPING:
                if (IsFinished)
                {
                    GoToState(STATE.OFF);
                    break;
                }
                if (LastTimerValue >= 1 && current == Current.Dialogue)
                {
                    TypingSpeed = SkippingTypingSpeed;
                    IsSkipping = true;
                }
                break;
            case STATE.PAUSED:
                IsSkipping = false;
                TypingSpeed = DefaultTypingSpeed;
                if (!IsWaiting && current != Current.Option)
                {
                    dialogueGraph.Continue();
                }
                if(IsFinished)
                {
                    GoToState(STATE.OFF);
                    break;
                }
                CurrentNode = dialogueGraph.current;
                OptionNode opt = dialogueGraph.current as OptionNode;
                DialogueNode dia = dialogueGraph.current as DialogueNode;
                if (opt != null)
                {
                    IsCameraFollow = opt.CameraFollow;
                    IsShowBubble = true;
                    current = Current.Option;
                    if(!hasOptionShowed)
                    {
                        OptionIndex = 0;
                        foreach (var item in OptionGameObject)
                        {
                            Destroy(item.gameObject);
                        }
                        OptionGameObject.Clear();
                        OptionPanel.TryGetComponent(out LayoutElement LayEle);
                        if(LayEle != null && opt.Width != 0)
                        {
                            LayEle.preferredWidth = opt.Width;
                        }
                        else if(LayEle != null)
                        {
                            LayEle.preferredWidth = DefaultWidth;
                        }
                        DisplayText.TryGetComponent(out LayoutElement LayEleText);
                        if (LayEleText != null && opt.Width != 0)
                        {
                            LayEleText.preferredWidth = opt.Width;
                        }
                        else if (LayEleText != null)
                        {
                            LayEleText.preferredWidth = DefaultWidth;
                        }
                        StartCoroutine(OptionShowing(opt));
                    }
                    else
                    {
                        if (IsChangingVolume)
                            AudioMgr.GetInstance().PlayAudio(GetCurVoice(), Volume, false);
                        else
                            AudioMgr.GetInstance().PlayAudio(GetCurVoice(), 0.6f, false);
                        if (!IsOptionShowing)
                        {
                            foreach (var item in OptionGameObject)
                            {
                                Destroy(item.gameObject);
                            }
                            OptionPanel.gameObject.SetActive(false);
                            OptionGameObject.Clear();
                            hasOptionShowed = false;
                            dialogueGraph.Continue(OptionIndex);
                            OptionIndex = 0;
                            PlayTextNext();
                        }
                    }
                }
                else if (dia != null)
                {
                    IsCameraFollow = dia.CameraFollow;
                    current = Current.Dialogue;
                    DisplayText.TryGetComponent(out LayoutElement LayEleText);
                    if (LayEleText != null && dia.Width != 0)
                    {
                        LayEleText.preferredWidth = dia.Width;
                    }
                    else if (LayEleText != null)
                    {
                        LayEleText.preferredWidth = DefaultWidth;
                    }
                    GoToState(STATE.TYPING);
                }
                else if (dialogueGraph.current is EventNode)
                {
                    current = Current.Event;
                    GoToState(STATE.TYPING);
                }
                else if(dialogueGraph.current is ReceiveFromCode)
                {
                    current = Current.ReceiveFromCode;
                    GoToState(STATE.TYPING);
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator OptionShowing(OptionNode opt)
    {
        hasOptionShowed = true;
        IsOptionShowing = true;
        OptionPanel.gameObject.SetActive(true);
        List<OptionClass> Options = opt.GetOptions();
        int a = 0;
        for (int i = 0; i < Options.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            ResMgr.GetInstance().LoadObjectAsyns<GameObject>(OptionObject, OptionObject =>
            {
                OptionObject.transform.SetParent(OptionPanel, false);
                OptionObject.TryGetComponent(out TMP_Text tryText);
                if(tryText == null)
                {
                    tryText = OptionObject.GetComponentInChildren<TMP_Text>();
                }
                string temp = Options[a].Text;
                if (ConvertHalfToFull)
                {
                    temp = temp.Replace("，", ",");
                    temp = temp.Replace("？", "?");
                    temp = temp.Replace("！", "!");
                    temp = temp.Replace("；", ";");
                    temp = temp.Replace("：", ":");
                }
                tryText.name = temp;
                tryText.text = temp;
                DefaultColor = tryText.color;
                OptionGameObject.Add(OptionObject);
                if (IsChangingVolume)
                    AudioMgr.GetInstance().PlayAudio(GetCurVoice(), Volume, false);
                else
                    AudioMgr.GetInstance().PlayAudio(GetCurVoice(), 0.6f, false);
                OptionObject.TryGetComponent(out LayoutElement LayEle);
                if(LayEle != null && opt.Width != 0)
                {
                    LayEle.preferredWidth = opt.Width;
                }
                else if (LayEle != null)
                {
                    LayEle.preferredWidth = DefaultWidth;
                }
                a++;
            });
        }
        IsOptionShowing = false;
    }

    private struct VertexAnim
    {
        public float angleRange;
        public float angle;
        public float speed;
    }

    void ResetGeometry()
    {
        TMP_TextInfo textInfo = DisplayText.textInfo;
       
        if (textInfo.meshInfo.Length >= 1)
        {
            var newVertexPositions = textInfo.meshInfo[0].vertices;

            // Upload the mesh with the revised information
            UpdateMesh(newVertexPositions, 0);
        }

        DisplayText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        DisplayText.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
    }

    private void UpdateMesh(Vector3[] _vertex, int index)
    {
        DisplayText.mesh.SetVertices(_vertex);
        DisplayText.mesh.uv = DisplayText.textInfo.meshInfo[index].uvs0;
        DisplayText.mesh.uv2 = DisplayText.textInfo.meshInfo[index].uvs2;
        DisplayText.mesh.colors32 = DisplayText.textInfo.meshInfo[index].colors32;
    }

    private void StopAnimate()
    {
        StopAllCoroutines();
        ResetGeometry();
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(0.1f);
        VertexCurve.preWrapMode = WrapMode.Loop;
        VertexCurve.postWrapMode = WrapMode.Loop;
        Matrix4x4 matrix;
        Vector3[] vertices;

        Vector3[] newVertexPositions;
        //Matrix4x4 matrix;

        int loopCount = 0;

        // Create an Array which contains pre-computed Angle Ranges and Speeds for a bunch of characters.
        VertexAnim[] vertexAnim = new VertexAnim[4096];
        for (int i = 0; i < 4096; i++)
        {
            vertexAnim[i].angleRange = Random.Range(10f, 25f);
            vertexAnim[i].speed = Random.Range(1f, 3f);
        }
        DisplayText.renderMode = TextRenderFlags.DontRender; // Instructing TextMesh Pro not to upload the mesh as we will be modifying it.
        while (true)
        {
            DisplayText.ForceMeshUpdate();
            vertices = DisplayText.textInfo.meshInfo[0].vertices;

            ResetGeometry();


            TMP_TextInfo textInfo = DisplayText.textInfo;
            List<Vector3[]> TempVertexs = new List<Vector3[]>();
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TempVertexs.Add(textInfo.meshInfo[i].mesh.vertices);
            }
            int characterCount = textInfo.characterCount;

            newVertexPositions = textInfo.meshInfo[0].vertices;

            for (int a = 0; a < waveEffects.Count; a++)
            {
                for (int i = waveEffects[a].StartIndex; i <= waveEffects[a].EndIndex; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible)
                        continue;

                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    float offsetY = VertexCurve.Evaluate((float)i / characterCount + loopCount / 50f) * waveEffects[a].CurveScale; // Random.Range(-0.25f, 0.25f);                    

                    newVertexPositions[vertexIndex + 0].y += offsetY;
                    newVertexPositions[vertexIndex + 1].y += offsetY;
                    newVertexPositions[vertexIndex + 2].y += offsetY;
                    newVertexPositions[vertexIndex + 3].y += offsetY;
                }
            }
            for (int a = 0; a < jitterEffects.Count; a++)
            {
                for (int i = jitterEffects[a].StartIndex; i <= jitterEffects[a].EndIndex; i++)
                {
                    // Setup initial random values
                    VertexAnim vertAnim = vertexAnim[i];
                    TMP_CharacterInfo charInfo = DisplayText.textInfo.characterInfo[i];

                    // Skip Characters that are not visible
                    if (!charInfo.isVisible)
                        continue;

                    int vertexIndex = charInfo.vertexIndex;

                    Vector2 charMidBasline = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, charInfo.baseLine);

                    Vector3 offset = charMidBasline;

                    vertices[vertexIndex + 0] += -offset;
                    vertices[vertexIndex + 1] += -offset;
                    vertices[vertexIndex + 2] += -offset;
                    vertices[vertexIndex + 3] += -offset;

                    vertAnim.angle = Mathf.SmoothStep(-vertAnim.angleRange, vertAnim.angleRange, Mathf.PingPong(loopCount / 25f * vertAnim.speed, 1f));
                    Vector3 jitterOffset = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0);

                    matrix = Matrix4x4.TRS(jitterOffset * jitterEffects[a].CurveScale, Quaternion.Euler(0, 0, Random.Range(-5f, 5f) * jitterEffects[a].AngleMultiplier), Vector3.one);

                    vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
                    vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                    vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                    vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);


                    vertices[vertexIndex + 0] += offset;
                    vertices[vertexIndex + 1] += offset;
                    vertices[vertexIndex + 2] += offset;
                    vertices[vertexIndex + 3] += offset;

                    vertexAnim[i] = vertAnim;
                }
            }

            loopCount += 1;
            
            if (textInfo.meshInfo.Length >= 1)
            {
                textInfo.meshInfo[0].mesh.vertices = textInfo.meshInfo[0].vertices;
                DisplayText.UpdateGeometry(textInfo.meshInfo[0].mesh, 0);
            }
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(canvas!= null && Scaler != null)
        {
            Scaler = canvas.GetComponent<CanvasScaler>();
            if (BubbleRange)
            {
                if (Scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    Gizmos.DrawWireCube((Vector2)canvas.transform.position + BubbleRangePosition, new Vector2(Scaler.referenceResolution.x * canvas.transform.localScale.x * BubbleRangeSize.x, Scaler.referenceResolution.y * canvas.transform.localScale.y * BubbleRangeSize.y));
                }
                else if (Scaler.uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize)
                {
                    Gizmos.DrawWireCube((Vector2)canvas.transform.position + BubbleRangePosition, new Vector2(BubbleRangeSize.x, BubbleRangeSize.y));
                }
            }
        }
    }
}
