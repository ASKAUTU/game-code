using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMessage : MonoBehaviour
{
    /// <summary>
    /// �����ִ� ������Ʈ
    /// </summary>
    public GameObject m_rootObj = null;

    /// <summary>
    /// �ؽ�Ʈ
    /// 0: �̸�, 1: ����
    /// </summary>
    public Text[] m_text = null;

    /// <summary>
    /// ����� �÷��̾�
    /// </summary>
    public AudioSource m_source = null;

    /// <summary>
    /// ��� �Ҹ�
    /// </summary>
    public AudioClip m_voiceAudio = null;

    /// <summary>
    /// �޼��� ǥ�� �ӵ�
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// �޼��� ǥ�� �÷���
    /// </summary>
    //bool m_viewFlag = false;

    /// <summary>
    /// ������ ���ڿ�
    /// </summary>
    string m_contentsStr = string.Empty;

    /// <summary>
    /// �ؽ�Ʈ ǥ�� ����
    /// </summary>
    /// <param name="argName">�̸�</param>
    /// <param name="argContents">����</param>
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
