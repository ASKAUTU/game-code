using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InformationScript : MonoBehaviour
{
    /// <summary>
    /// ���� �޼���
    /// </summary>
    public MainMessage m_mainMessage = null;

    /// <summary>
    /// �Ѿ�� ȭ��
    /// </summary>
    public GameObject m_blackScreen = null;

    /// <summary>
    /// ���丮�� �����ִ� ȭ��
    /// </summary>
    public GameObject m_storyScreen = null;

    /// <summary>
    /// ȭ�� ���
    /// </summary>
    public GameObject m_backGround = null;

    /// <summary>
    /// Ŭ���Ǵ� ���
    /// </summary>
    public GameObject m_clickBackGround = null;

    /// <summary>
    /// ��ư��
    /// </summary>
    public GameObject[] m_btns = null;

    /// <summary>
    /// ��ư�� �������°�
    /// </summary>
    public Text[] m_btnTexts = null;

    /// <summary>
    /// ����� �÷��̾�
    /// </summary>
    public AudioSource m_source = null;

    /// <summary>
    /// ��ŵ �Ҹ�
    /// </summary>
    public AudioClip m_skipAudio = null;

    /// <summary>
    /// �뺻
    /// </summary>
    public string[] m_storyWordBook = null;

    /// <summary>
    /// ���ϴ»�� �̸�
    /// </summary>
    public string[] m_NameWordBook = null;

    /// <summary>
    /// ��ŵ ������ �ð�
    /// </summary>
    public float[] m_skipCoolTime = null;

    /// <summary>
    /// ��ư ����
    /// </summary>
    public int[] m_btnCount = null;

    /// <summary>
    /// ��ư ����
    /// </summary>
    public string[] m_btnContants = null;

    /// <summary>
    /// ��ŵ �÷���
    /// </summary>
    bool m_skipFlag = false;

    /// <summary>
    /// ��ȭ �ε��� ��ȣ
    /// </summary>
    int m_talkIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_skipFlag  == true) StopImformation();
    }



    /// <summary>
    /// ���� ���
    /// </summary>
    public IEnumerator DontFall()
    {
        // �ε�â
        m_blackScreen.SetActive(true);
        Color _color = m_blackScreen.GetComponent<Image>().color;

        _color.a = 1;
        m_blackScreen.GetComponent<Image>().color = _color;

        for (float i = _color.a; i <= 1; i += 0.25f)
        {
            _color.a = i;
            m_blackScreen.GetComponent<Image>().color = _color;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2.0f);

        for (float i = _color.a; i >= 0; i -= 0.01f)
        {
            _color.a = i;
            m_blackScreen.GetComponent<Image>().color = _color;
            yield return new WaitForSeconds(0.01f);
        }
        m_blackScreen.SetActive(false);

        // ��� �˸��� ����
        m_skipFlag = false;
        m_talkIndex = 0;
        m_storyScreen.SetActive(true);
        m_backGround.SetActive(true);
        m_clickBackGround.SetActive(true);
        m_mainMessage.SetText(m_NameWordBook[m_talkIndex], m_storyWordBook[m_talkIndex]);
        Invoke("WaitToSkip", m_skipCoolTime[m_talkIndex]);
    }

    public void CrackVoice()
    {
        m_skipFlag = false;
        m_talkIndex = 1;
        m_storyScreen.SetActive(true);
        m_backGround.SetActive(true);
        m_mainMessage.SetText(m_NameWordBook[m_talkIndex], m_storyWordBook[m_talkIndex]);
        Invoke("WaitForBtn", m_skipCoolTime[m_talkIndex]);
    }

    /// <summary>
    /// ��ŵ ���� ����
    /// </summary>
    void WaitToSkip()
    {
        m_skipFlag = true;
    }

    void WaitForBtn()
    {

        for (int i = 0; i < m_btnCount[m_talkIndex]; i++)
        {
            m_btns[i].SetActive(true);
            m_btnTexts[i].text = m_btnContants[i];
            m_skipFlag = true;
        }
    }

    public void ClickToNextScene()
    {
        StartCoroutine(ToNextScene());
    }

    IEnumerator ToNextScene()
    {
        for (int i = 0; i < m_btnCount[m_talkIndex]; i++)
        {
            m_btns[i].SetActive(false);
        }
        m_blackScreen.SetActive(true);
        m_source.PlayOneShot(m_skipAudio);
        Color _color = m_blackScreen.GetComponent<Image>().color;
        _color.a = 0;
        m_blackScreen.GetComponent<Image>().color = _color;
        for (float i = _color.a; i <= 1; i += 0.025f)
        {
            _color.a = i;
            m_blackScreen.GetComponent<Image>().color = _color;
            yield return new WaitForSeconds(0.025f);
        }
        SceneManager.LoadScene("Forest");
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    public void StopImformation()
    {
        if (!m_skipFlag) return;
        m_skipFlag = false;
        for (int i = 0; i < m_btnCount[m_talkIndex]; i++)
        {
            m_btns[i].SetActive(true);
        }
        m_source.PlayOneShot(m_skipAudio);
        m_mainMessage.SetText(" ", " ");
        m_clickBackGround.SetActive(false);
        m_backGround.SetActive(false);
        m_storyScreen.SetActive(false);
    }
}
