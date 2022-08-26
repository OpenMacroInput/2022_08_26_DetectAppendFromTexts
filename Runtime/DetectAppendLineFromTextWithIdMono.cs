using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class DetectAppendLineFromTextWithIdMono : MonoBehaviour
{
    public DetectAppendLineFromTextWithId m_observer;
    public void PushTextToTrack(string textId, string text)
    {
        m_observer.PushTextToTrack(textId, text);
    }

}
[System.Serializable]
public class DetectAppendLineFromTextWithId 
{

    public Eloi.PrimitiveUnityEvent_String m_newLineReceived = new Eloi.PrimitiveUnityEvent_String();
    public Eloi.PrimitiveUnityEvent_Char m_newCharReceived = new Eloi.PrimitiveUnityEvent_Char();
    public Dictionary<string, string> m_receivedGoogleDoc = new Dictionary<string, string>();
    public bool m_notifyFirstSet;
    public TypeAppend m_appendOrderType;
    public enum TypeAppend { StartToEnd, EndToStart, ExpelMiddleOrder };
    public void PushTextToTrack(string textId, string text)
    {
        bool containt = m_receivedGoogleDoc.ContainsKey(textId);

        if (!containt)
        {
            m_receivedGoogleDoc.Add(textId, text);
        }

       
        string previous = m_receivedGoogleDoc[textId];
        m_receivedGoogleDoc[textId] = text;

        if (m_notifyFirstSet && !containt)
        {
            previous = "";
        }else 
        if (!m_notifyFirstSet && !containt)
        {
            return;
        }

        if (m_appendOrderType == TypeAppend.StartToEnd)
        {
            DetectAppendLineFromTextWithIdUtility.DetectAppendFromTexts(in previous, in text, out string[] l, false);
            DetectAppendLineFromTextWithIdUtility.DetectAppendFromTexts(in previous, in text, out char[] c, false);
            Push(l);
            Push(c);

        }
        else if (m_appendOrderType == TypeAppend.EndToStart)
        {
            DetectAppendLineFromTextWithIdUtility.DetectAppendFromTexts(in previous, in text, out string[] l, true);
            DetectAppendLineFromTextWithIdUtility.DetectAppendFromTexts(in previous, in text, out char[] c, true);
            Push(l);
            Push(c);

        }
        else if (m_appendOrderType == TypeAppend.ExpelMiddleOrder)
        {
            DetectAppendLineFromTextWithIdUtility.DetectAppendFromTextsInExpelOrder(in previous, in text, out string[] l, out string[] r);
            DetectAppendLineFromTextWithIdUtility.DetectAppendFromTextsInExpelOrder(in previous, in text, out char[] cl, out char[] cr);
            Push(l);
            Push(r);
            Push(cl);
            Push(cr);
        }

    }

     void Push(in string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            m_newLineReceived.Invoke(lines[i]);
        }
    }
     void Push(in char[] c)
    {
        for (int i = 0; i < c.Length; i++)
        {
            m_newCharReceived.Invoke(c[i]);
        }
    }


}


[System.Serializable]
public class DetectAppendLineFromTextWithIdUtility 
{
    public static void DetectAppendFromTextsInExpelOrder(in string previousText, in string newText, out string[] startAppendLines, out string[] endAppendLines)
    {
        DetectAppendFromTexts( in previousText, in newText, out string startAppend, out string endAppend);
        GetLinesFromText(in startAppend, out  startAppendLines, true);
        GetLinesFromText(in endAppend, out endAppendLines, false);
    }

    public static void DetectAppendFromTextsInExpelOrder( in string previousText, in string newText, out char[] charsStart, out char[] charsEnd)
    {
        DetectAppendFromTexts( in previousText, in newText, out string startAppend, out string endAppend);
        
        GetLinesFromText(in startAppend, out string[]  lines,  true);
        GetCharsFromLines(in lines,  false, out charsStart);
        GetLinesFromText(in endAppend, out lines, false);
        GetCharsFromLines(in lines, true, out charsEnd);
    }

    public static void DetectAppendFromTexts( in string previousText, in string newText, out char[] chars, in bool reverse)
    {
        DetectAppendFromTexts( in previousText, in newText, out string append);
        GetLinesFromText(in append, out string[] lines, in reverse);
        GetCharsFromLines(in lines, in reverse, out chars);
    }

    public static void DetectAppendFromTexts(in string previousText, in string newText, out string [] lines, in bool reverse)
    {

        DetectAppendFromTexts( in previousText, in newText, out string append);
        GetLinesFromText(in append, out lines, in reverse);
    }
    public static void DetectAppendFromTexts( in string previousText, in string newText, out string appendStartAndEnd)
    {
        DetectAppendFromTexts( in previousText, in newText, out string startAppend, out string endAppend);
        appendStartAndEnd = startAppend + endAppend;
    }
    public static void DetectAppendFromTexts(in string previousText ,in string newText, out string startAppend, out string endAppend)
    {
        startAppend = "";
        endAppend = "";

            if (newText.Length == previousText.Length)
            { }
            else if (newText.Length < previousText.Length)
            {
                startAppend = (newText);
            }
            else if (newText.Length > previousText.Length)
            {
                int index = newText.IndexOf(previousText);
                if (index > -1)
                {
                    Eloi.E_StringUtility.SplitInThree(in newText, index, index + previousText.Length,
                       out string l, out string m, out string r);
                    startAppend = l;
                    endAppend=r;
                }
            }
        
    }

    public static void GetLinesFromText(in string text, out string [] lines, in bool reverseLineOrder)
    {
        string[] l = text.Split(new char[] { '\n', '\r' });
        if (l.Length > 0)
        {
            if(reverseLineOrder)
                lines = l.Where(k => !string.IsNullOrEmpty(k.Trim())).Reverse().ToArray();
            else
                lines = l.Where(k => !string.IsNullOrEmpty(k.Trim())).ToArray();
        }
        else
            lines = new string[] { };
    }
    public static void GetCharsFromLines(in string[] arrayLine,in bool reverseWhenJoined, out char [] appendChars)
    {
        List<char> c = new List<char>();
        for (int i = 0; i < arrayLine.Length; i++)
        {
            if (reverseWhenJoined)
                c.AddRange(arrayLine[i].Reverse<char>().ToArray());
            else
                c.AddRange(arrayLine[i].ToCharArray());
        }
        appendChars= c.ToArray();
    }

}