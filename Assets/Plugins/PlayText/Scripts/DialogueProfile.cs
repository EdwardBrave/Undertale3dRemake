using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class DialogueProfile : ScriptableObject
{
    public List<TalkingPersonClass> TalkingPerson = new List<TalkingPersonClass>();
    public AudioClip DefaultVoice;
    [SerializeField]
    public List<string> Language;
    public List<ReplaceClass> ReplaceVariable = new List<ReplaceClass>();

    /// <summary>
    /// Get value from ReplaceVariable List by giving Variable name
    /// </summary>
    /// <param name="Variable">Variable name</param>
    /// <returns></returns>
    public string GetValue(string Variable, string Lan)
    {
        int index = ReplaceVariable.FindIndex(VAR => { return VAR.Variable == Variable; });
        int LanIndex = Language.FindIndex(XVAR => { return XVAR == Lan; });
        if (index >= 0 && LanIndex >= 0)
            return ReplaceVariable[index].Value[LanIndex];
        else
            return string.Empty;
    }

    /// <summary>
    /// Set value by giving Variable name and the Value you want to set
    /// </summary>
    /// <param name="Variable">Variable name</param>
    /// <param name="Value">Variable value</param>
    /// <returns>True means set successful. False means set unsuccessful</returns>
    public bool SetValue(string Variable, string Value, string Lan)
    {
        int index = ReplaceVariable.FindIndex(VAR => { return VAR.Variable == Variable; });
        int LanIndex = Language.FindIndex(XVAR => { return XVAR == Lan; });
        if (index >= 0 && LanIndex >= 0)
        {
            ReplaceVariable[index].Value[LanIndex] = Value;
            return true;
        }
        else
            return false;
    }

    public bool SetValueToAllLanguage(string Variable, string Value)
    {
        int index = ReplaceVariable.FindIndex(VAR => { return VAR.Variable == Variable; });
        
        if (index >= 0)
        {
            for (int i = 0; i < Language.Count; i++)
            {
                ReplaceVariable[index].Value[i] = Value;
            }
            return true;
        }
        else
            return false;
    }
}

[System.Serializable]
public class TalkingPersonClass
{
    public string Name;
    public AudioClip Voice;
    [SerializeField]
    public List<string> Expression;
    public TalkingPersonClass()
    {
        Name = string.Empty;
        Voice = null;
        Expression = new List<string>();
    }
}

[System.Serializable]
public class ReplaceClass
{
    public bool showing;
    public string Variable;
    [SerializeField]
    public List<string> Value;
    public ReplaceClass()
    {
        showing = false;
        Variable = string.Empty;
        Value = new List<string>();
    }
}
