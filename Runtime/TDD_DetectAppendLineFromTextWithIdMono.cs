using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TDD_DetectAppendLineFromTextWithIdMono : MonoBehaviour
{

    [TextArea(0, 10)]
    public string m_preZoneToPush;
    [Header("Push on modify")]
    public string m_id = "lkjajlk";
    [TextArea(0, 10)]
    public string m_text;


    public string m_test;
    public int m_start;
    public int m_end;
    public string m_l;
    public string m_m;
    public string m_r;

    public IdStringEvent m_newTextWithId;
    [System.Serializable]
    public class IdStringEvent : UnityEvent<string, string> { }


    [ContextMenu("Push Pre Text")]
    public void PushPreText()
    {
        m_text = m_preZoneToPush + m_text;
        Push();
    }
    [ContextMenu("Push")]
    public void Push()
    {
        m_newTextWithId.Invoke(m_id, m_text);
    }
    [ContextMenu("PushEmpty")]
    public void PushEmpty()
    {
        m_newTextWithId.Invoke(m_id, "");
    }



    private void OnValidate()
    {
       //Push();

        Eloi.E_StringUtility.SplitInThree( m_test, m_start, m_end,
           out m_l, out m_m, out m_r);
    }

}
