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
    public void PushText(string text) {
        m_newTextWithId.Invoke(m_id, text);
    }
}
