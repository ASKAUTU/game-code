using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{

    /// <summary>
    /// �޴��� ���� Ÿ��
    /// </summary>
    MenuType.TYPE m_menuType = MenuType.TYPE.Start;

    /// <summary>
    /// �޴� ǥ��
    /// </summary>
    public GameObject m_selector = null;

    /// <summary>
    /// ��ư��
    /// </summary>
    public GameObject[] m_buttons = null;

    /// <summary>
    /// �Ѿ�� ȭ��
    /// </summary>
    public GameObject m_screen = null;

    /// <summary>
    /// ���� �޴�
    /// </summary>
    public GameObject m_SettingMenu = null;

    /// <summary>
    /// ����� �÷��̾�
    /// </summary>
    public AudioSource m_audio = null;

    /// <summary>
    /// ���������� ���ӵ� ����� �÷��̾�
    /// </summary>
    public AudioSource m_keepAudio = null;

    /// <summary>
    /// ���� ����� Ŭ��
    /// </summary>
    public AudioClip m_selectAudio = null;

    /// <summary>
    /// ���ÿϷ� ����� Ŭ��
    /// </summary>
    public AudioClip m_complAudio = null;

    /// <summary>
    /// ���������� �Ѿ�� ����� Ŭ��
    /// </summary>
    public AudioClip m_nextSceneAudio = null;

    /// <summary>
    /// ǥ���� ��ġ
    /// </summary>
    public Vector3[] m_selectorPos = null;

    /// <summary>
    /// ��鸮�� ����
    /// </summary>
    public float m_shakeRange = 0.0f;

    /// <summary>
    /// ��ư���� ó����ġ
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
