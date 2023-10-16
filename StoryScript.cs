using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryScript : MonoBehaviour
{
    /// <summary>
    /// ����ȭ�� ��ũ��Ʈ
    /// </summary>
    public BlackScreen m_blackScreen = null;

    /// <summary>
    /// ���� �޼���
    /// </summary>
    public MainMessage m_mainMessage = null;

    /// <summary>
    /// ���丮�� �����ִ� ȭ��
    /// </summary>
    public GameObject m_storyScreen = null;

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
    /// ���丮 ����
    /// </summary>
    public StoryType.TYPE m_storyType = StoryType.TYPE.Story;

    /// <summary>
    /// ���� �����ִ� n��° ���
    /// </summary>
    int m_currentStory = 0;

    /// <summary>
    /// ��ŵ �÷���
    /// </summary>
    bool m_skipFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(m_blackScreen.ToNextScene(true));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_storyScreen.activeSelf == true) SkipToNextStory();
    }

    /// <summary>
    /// ���� ���� ��ŵ
    /// </summary>
    void SkipToNextStory()
    {
        if (!m_skipFlag) return;
        m_skipFlag = false;
        m_source.PlayOneShot(m_skipAudio);

        if (m_currentStory >= m_storyWordBook.Length)
        {
            SceneManager.LoadScene("Game");
            return;
        }

        m_mainMessage.SetText(m_NameWordBook[m_currentStory], m_storyWordBook[m_currentStory]);
        Invoke("WaitToSkip", m_skipCoolTime[m_currentStory]);
        m_currentStory++;
    }
    
    /// <summary>
    /// ��ŵ ���� ����
    /// </summary>
    void WaitToSkip()
    {
        m_skipFlag = true;
    }
}
