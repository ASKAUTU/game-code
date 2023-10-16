using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{

    /// <summary>
    /// 메뉴에 사용될 타입
    /// </summary>
    MenuType.TYPE m_menuType = MenuType.TYPE.Start;

    /// <summary>
    /// 메뉴 표시
    /// </summary>
    public GameObject m_selector = null;

    /// <summary>
    /// 버튼들
    /// </summary>
    public GameObject[] m_buttons = null;

    /// <summary>
    /// 넘어가는 화면
    /// </summary>
    public GameObject m_screen = null;

    /// <summary>
    /// 세팅 메뉴
    /// </summary>
    public GameObject m_SettingMenu = null;

    /// <summary>
    /// 오디오 플레이어
    /// </summary>
    public AudioSource m_audio = null;

    /// <summary>
    /// 다음씬까지 지속될 오디오 플레이어
    /// </summary>
    public AudioSource m_keepAudio = null;

    /// <summary>
    /// 고르는 오디오 클립
    /// </summary>
    public AudioClip m_selectAudio = null;

    /// <summary>
    /// 선택완료 오디오 클립
    /// </summary>
    public AudioClip m_complAudio = null;

    /// <summary>
    /// 다음씬으로 넘어가는 오디오 클립
    /// </summary>
    public AudioClip m_nextSceneAudio = null;

    /// <summary>
    /// 표시의 위치
    /// </summary>
    public Vector3[] m_selectorPos = null;

    /// <summary>
    /// 흔들리는 범위
    /// </summary>
    public float m_shakeRange = 0.0f;

    /// <summary>
    /// 버튼들의 처음위치
    /// </summary>
    public Vector2[] m_startPos = null;

    void Start()
    {
        StartCoroutine(ShakeButton());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            m_audio.PlayOneShot(m_complAudio);
            switch (m_menuType)
            {
                case MenuType.TYPE.Start:
                    StartCoroutine(ToNextScene());
                    break;
                case MenuType.TYPE.Setting:
                    m_SettingMenu.SetActive(true);
                    break;
                case MenuType.TYPE.Leave:
                    Application.Quit();
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && (int)m_menuType < 2)
        {
            m_audio.PlayOneShot(m_selectAudio);
            m_buttons[(int)m_menuType].GetComponent<Text>().color = Color.white;
            m_buttons[(int)m_menuType].transform.localPosition = m_startPos[(int)m_menuType];
            m_menuType++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && (int)m_menuType > 0)
        {
            m_audio.PlayOneShot(m_selectAudio);
            m_buttons[(int)m_menuType].GetComponent<Text>().color = Color.white;
            m_buttons[(int)m_menuType].transform.localPosition = m_startPos[(int)m_menuType];
            m_menuType--;
        }


        m_buttons[(int)m_menuType].GetComponent<Text>().color = Color.gray;

        m_selector.transform.position = m_selectorPos[(int)m_menuType];
    }

    IEnumerator ToNextScene()
    {
        Color _color = m_screen.GetComponent<Image>().color;

        yield return new WaitForSeconds(0.5f);

        m_keepAudio.PlayOneShot(m_nextSceneAudio);

        for (float i = _color.a; i <= 1; i += 0.025f)
        {
            _color.a = i;
            m_screen.GetComponent<Image>().color = _color;
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadScene("Game");
    }

    IEnumerator ShakeButton()
    {
        while (true)
        {
            m_buttons[(int)m_menuType].transform.Translate((Vector2)Random.insideUnitCircle * m_shakeRange * Time.deltaTime);
            yield return new WaitForSeconds(0.05f);
            m_buttons[(int)m_menuType].transform.localPosition = m_startPos[(int)m_menuType];
        }
    }
}
