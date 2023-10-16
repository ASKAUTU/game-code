using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InformationScript : MonoBehaviour
{
    /// <summary>
    /// 메인 메세지
    /// </summary>
    public MainMessage m_mainMessage = null;

    /// <summary>
    /// 넘어가는 화면
    /// </summary>
    public GameObject m_blackScreen = null;

    /// <summary>
    /// 스토리를 보여주는 화면
    /// </summary>
    public GameObject m_storyScreen = null;

    /// <summary>
    /// 화면 배경
    /// </summary>
    public GameObject m_backGround = null;

    /// <summary>
    /// 클릭되는 배경
    /// </summary>
    public GameObject m_clickBackGround = null;

    /// <summary>
    /// 버튼들
    /// </summary>
    public GameObject[] m_btns = null;

    /// <summary>
    /// 버튼의 내용적는곳
    /// </summary>
    public Text[] m_btnTexts = null;

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
    /// 버튼 갯수
    /// </summary>
    public int[] m_btnCount = null;

    /// <summary>
    /// 버튼 내용
    /// </summary>
    public string[] m_btnContants = null;

    /// <summary>
    /// 스킵 플래그
    /// </summary>
    bool m_skipFlag = false;

    /// <summary>
    /// 대화 인덱스 번호
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
    /// 낙사 경고문
    /// </summary>
    public IEnumerator DontFall()
    {
        // 로딩창
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

        // 경고 알림문 시작
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
    /// 스킵 가능 설정
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
    /// 공지 끝내기
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
