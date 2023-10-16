using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMessage : MonoBehaviour
{
    /// <summary>
    /// 보여주는 오브젝트
    /// </summary>
    public GameObject m_rootObj = null;

    /// <summary>
    /// 텍스트
    /// 0: 이름, 1: 내용
    /// </summary>
    public Text[] m_text = null;

    /// <summary>
    /// 오디오 플레이어
    /// </summary>
    public AudioSource m_source = null;

    /// <summary>
    /// 대사 소리
    /// </summary>
    public AudioClip m_voiceAudio = null;

    /// <summary>
    /// 메세지 표시 속도
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// 메세지 표시 플래그
    /// </summary>
    //bool m_viewFlag = false;

    /// <summary>
    /// 컨텐츠 문자열
    /// </summary>
    string m_contentsStr = string.Empty;

    /// <summary>
    /// 텍스트 표시 설정
    /// </summary>
    /// <param name="argName">이름</param>
    /// <param name="argContents">내용</param>
    public void SetText(string argName, string argContents)
    {
        //if (m_viewFlag) return;
        //m_viewFlag = true;
        m_text[0].text = argName;

        if (argContents.Length > 110) m_contentsStr = argContents.Substring(0, 110);
        else m_contentsStr = argContents;

        StartCoroutine(OneStepText());
    }

    IEnumerator OneStepText()
    {
        string _nowViewStr = string.Empty;

        for (int i = 0; i < m_contentsStr.Length; i++)
        {
            _nowViewStr = m_contentsStr.Substring(0, i);
            m_text[1].text = _nowViewStr;
            if (m_contentsStr.Substring(i, 1) != " ") m_source.PlayOneShot(m_voiceAudio);
            yield return new WaitForSeconds(m_speed);
        }
        //m_viewFlag = false;
    }
}
