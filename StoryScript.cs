using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryScript : MonoBehaviour
{
    /// <summary>
    /// 검은화면 스크립트
    /// </summary>
    public BlackScreen m_blackScreen = null;

    /// <summary>
    /// 메인 메세지
    /// </summary>
    public MainMessage m_mainMessage = null;

    /// <summary>
    /// 스토리를 보여주는 화면
    /// </summary>
    public GameObject m_storyScreen = null;

    /// <summary>
    /// 오디오 플레이어
    /// </summary>
    public AudioSource m_source = null;

    /// <summary>
    /// 스킵 소리
    /// </summary>
    public AudioClip m_skipAudio = null;

    /// <summary>
    /// 대본
    /// </summary>
    public string[] m_storyWordBook = null;

    /// <summary>
    /// 말하는사람 이름
    /// </summary>
    public string[] m_NameWordBook = null;

    /// <summary>
    /// 스킵 가능한 시간
    /// </summary>
    public float[] m_skipCoolTime = null;

    /// <summary>
    /// 스토리 형식
    /// </summary>
    public StoryType.TYPE m_storyType = StoryType.TYPE.Story;

    /// <summary>
    /// 현재 보고있는 n번째 대사
    /// </summary>
    int m_currentStory = 0;

    /// <summary>
    /// 스킵 플래그
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
    /// 다음 대사로 스킵
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
    /// 스킵 가능 설정
    /// </summary>
    void WaitToSkip()
    {
        m_skipFlag = true;
    }
}
